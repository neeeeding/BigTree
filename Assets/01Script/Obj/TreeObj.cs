using _01Script.UI;
using UnityEngine;

namespace _01Script.Obj
{
    public class TreeObj : ObjCheck
    {
        [Header("Need")]
        [SerializeField] private LayerMask fire;
        [SerializeField] private DialogManager dialogManager; //대화
        private bool isTree; //true : 트리 / false : 전기 트리

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
                    FireCheck(obj.gameObject);
                }
            }

            if (isTree)
            {
                gameObject.layer = LayerMask.NameToLayer("Tree");
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer("ElectricityTree");
            }
            
            IsStart = false;
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
            {
                print("ok");
                dialogManager.DoDialog(new string[]{"이 나무와 물이 닿으면 전기가 안 통하는 것 같다.", "...근데 불이 닿으면 다시 전기가 통하는 것 같기도"});
            }
        }


        private void FireCheck(GameObject obj) //근처 불 확인
        {
            if ((fire.value & (1 << obj.layer)) != 0)
            {
                isTree = true;
            }
        }
    }
}