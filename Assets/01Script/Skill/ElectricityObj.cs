using Unity.VisualScripting;
using UnityEngine;

namespace _01Script.Skill
{
    public class ElectricityObj : ObjCheck
    {
        override protected void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            if (check)
            {
                foreach (var obj in col)
                {
                    WaterCheck(obj.gameObject);
                }
            }
            isStart = false;
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