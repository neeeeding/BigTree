using UnityEngine;

namespace _01Script.Obj
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
        
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            isTree = false;

            if (Check)
            {
                foreach (var obj in Col)
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
            IsStart = false;
        }


        private void TreeCheck(GameObject obj) //전기 나무 확인
        {
            if ((electricityTree.value & (1 << obj.layer)) != 0 || (tree.value & (1 << obj.layer)) != 0)
            {
                isTree = true;
            }

            if (IsStart&&(water.value & (1 << obj.layer)) != 0)
            {
                Destroy(obj);
                Destroy(gameObject);
            }
        }
    }
}