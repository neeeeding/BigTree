using Unity.VisualScripting;
using UnityEngine;

namespace _01Script.Skill
{
    public class ElectricityObj : ObjCheck
    {
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
            IsStart = false;
        }
        
        
        private void WaterCheck(GameObject obj) //물에 대한 확인
        {
            if (obj.TryGetComponent(out WaterObj water))
            {
                water.Electricity();
                Destroy(gameObject);
            }
            
        }
    }
}