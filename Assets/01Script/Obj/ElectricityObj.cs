using UnityEngine;

namespace _01Script.Obj
{
    public class ElectricityObj : ObjCheck
    {
        protected override void Update()
        {
            base.Update();

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