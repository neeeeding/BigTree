using _01Script.Manager;
using _01Script.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace _01Script.Obj
{
    public class MetalObj : ObjCheck
    {
        [Header("Setting")]
        [SerializeField] private bool isBox; // 자신이 상자인지
        [SerializeField] private KeyManager.CheckKeyType keyType; //열쇠 종류
        [SerializeField] private MetalObj[]  metalObjs; //상자 일 때 채워주기
        [Header("Need")]
        [SerializeField] private GameObject light; //확인 빛
        [FormerlySerializedAs("dialog")] [SerializeField] private DialogManager dialogManager; //대화
        [SerializeField] private KeyManager key;
        
        private bool isAll; //동시에 모두 전기가 통한적이 있었는지
        private bool me;
        protected override void Awake()
        {
            base.Awake();
            me = false;
            isAll =false;
        }

        private void Update()
        {
            light.SetActive(isAll || me);
            
            foreach (MetalObj sc in metalObjs)
            {
                if (!sc.IsMe())
                {
                    return;   
                }
            }
            
            if(isBox)
                isAll = true;
        }
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            if (Check)
            {
                foreach (var obj in Col)
                {
                    WaterCheck(obj.gameObject);
                }
            }
            else
            {
                me = false;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
            {
                if (isBox && isAll)
                {
                    dialogManager.DoDialog(new string[]{"열쇠를 얻었다."});
                    key.GetKey(keyType);
                }
                else if (isBox)
                {
                    dialogManager.DoDialog(new string[]{"여기에 열쇠가 들어있는거 같다."});
                }
                else
                {
                    dialogManager.DoDialog(new string[]{"이게 상자인것 같다.","모든 상자에 전기가 통해야 할 것 같다."});
                }
            }
        }

        public bool CheckAll() //완료 했는지 확인
        {
            return isAll;
        }

        public bool IsMe() //일단 본인은?
        {
            return me;
        }
        
        private void WaterCheck(GameObject obj) //전기 통하는 물 확인
        {
            if (obj.TryGetComponent(out WaterObj water))
            {
                me =water.OtherWaterCheck(WaterObj.WaterCheckTypEnum.On);
            }
        }
    }
}