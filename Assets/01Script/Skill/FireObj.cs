using UnityEngine;

namespace _01Script.Skill
{
    public class FireObj : ObjCheck
    {
        [SerializeField] private float upSize = 3; //키울 사이즈
        [Header("Need")] 
        [SerializeField] private LayerMask electricityTree; //전기 나무
        [SerializeField] private LayerMask tree;
        [SerializeField] private LayerMask water; //물
        private bool isTree; // true : 한그루라도 전기 나무가 있음. / flase : 전기 나무 없음.

        protected override void Awake()
        {
            base.Awake();
            isTree = false;
        }
        
        override protected void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            isTree = false;

            if (check)
            {
                foreach (var obj in col)
                {
                    TreeCheck(obj.gameObject);
                }
            }
            
            if (!isTree)
            {
                gameObject.transform.localScale = Vector3.one * upSize;
            }
            else
            {
                gameObject.transform.localScale = Vector3.one;
            }
            isStart = false;
        }


        private void TreeCheck(GameObject obj) //전기 나무 확인
        {
            if ((electricityTree.value & (1 << obj.layer)) != 0 || (tree.value & (1 << obj.layer)) != 0)
            {
                isTree = true;
            }

            if (isStart&&(water.value & (1 << obj.layer)) != 0)
            {
                Destroy(obj);
                Destroy(gameObject);
            }
        }
    }
}