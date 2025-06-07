using UnityEngine;
using UnityEngine.Serialization;

namespace _01Script.Skill
{
    public class WaterObj : ObjCheck
    {
        [Header("Need")]
        [SerializeField] private LayerMask electricityTree; //전기 나무
        [SerializeField] private LayerMask fire; //불
        [SerializeField] private GameObject electricity; //전기 (자식)

        
        private bool canElectricity; //true : 전기 통해도 됨 / false : 전기 못 통함
        private bool isElectricity; //true : 전기 공급됨 / false : 전기 공급 안되고 있음.

        protected override void Awake()
        {
            base.Awake();
            canElectricity = true;
            isElectricity = false;
        }
        
        override protected void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            canElectricity = true;

            if (check)
            {
                foreach (var obj in col)
                {
                    TreeCheck(obj.gameObject);
                }
            }
            electricity.gameObject.SetActive(isElectricity);
            
            isStart = false;
        }
        
        private void TreeCheck(GameObject obj) //전기 나무 확인
        {
            if ((electricityTree.value & (1 << obj.layer)) != 0) //나무
            {
                canElectricity = false;
                Electricity();
            }

            if (isStart&&(fire.value & (1 << obj.layer)) != 0) //불
            {
                Destroy(obj);
                Destroy(gameObject);
            }
            
            if (isElectricity&& obj.TryGetComponent(out WaterObj water)) //물 (전기가 통하는 중일 때)
            {
                if (!water.Electricity()) //상대는 전기가 안통하는 중
                {
                    canElectricity = false;
                    isElectricity = false; //이제 나도 안됨.
                }
            }
        }

        public bool Electricity() //전기 통하고 있는지
        {
            isElectricity = canElectricity;
            return canElectricity;
        }
    }
}