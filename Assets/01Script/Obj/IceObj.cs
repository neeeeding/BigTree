using _01Script.Manager;
using UnityEngine;

namespace _01Script.Obj
{
    public class IceObj : ObjCheck
    {
        [Header("Need")] 
        [SerializeField] private LayerMask water; //물
        [SerializeField] private LayerMask fire; //불
        
        private IceManager _iceManager; //얼음 매니저

        private void OnEnable()
        {
            IsStart = true;
        }
        
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            
            if (Check)
            {
                foreach (var obj in Col)
                {
                    TreeCheck(obj.gameObject);
                }
            }
            IsStart = false;
        }

        public void SetParent(IceManager p)
        {
            _iceManager = p;
        }
        
        private void TreeCheck(GameObject obj) //전기 나무 확인
        {
            if (IsStart&&(water.value & (1 << obj.layer)) != 0)
            {
                Destroy(obj);
            }

            if ((fire.value & (1 << obj.layer)) != 0)
            {
                Destroy(obj);
                _iceManager.PushIce(this);
            }
        }
    }
}