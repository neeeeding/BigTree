using System;
using System.Collections;
using UnityEngine;

namespace _01Script.Skill
{
    public class WaterObj : ObjCheck
    {
        [Header("Setting")]
        [SerializeField] private float delay = 1; //전기가 통하는 시간
        [Header("Need")]
        [SerializeField] private LayerMask electricityTree; //전기 나무
        [SerializeField] private LayerMask fire; //불
        [SerializeField] private GameObject electricity; //전기 (자식)

        private bool _isDelay; // true : 딜레이 / false : 딜레이 안 재고 있음.
        private bool _canElectricity; //true : 전기 통해도 됨 / false : 전기 못 통함
        private bool _isElectricity; //true : 전기 공급됨 / false : 전기 공급 안되고 있음.
        private bool _isMe; //전기 공급 안되는게 자신이 원인인지

        protected override void Awake()
        {
            base.Awake();
            _canElectricity = true;
            _isElectricity = false;
        }
        
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            if (Check)
            {
                bool youCant = false;
                bool youOff = true;
                foreach (var obj in Col)
                {
                    TreeCheck(obj.gameObject, ref youCant, ref youOff);
                }
                
                if (_isMe && _canElectricity &&  !youCant)
                { 
                    _isMe = false;
                }

                if (youOff)
                {
                    _isDelay = false;
                }
            }
            electricity.gameObject.SetActive(_isElectricity);
            
            IsStart = false;
        }

        private IEnumerator ElectricityDelay() //전기 딜레이
        {
            yield return new WaitForSeconds(delay);
            _isElectricity = false;
            _isDelay = true;
        }
        
        private void TreeCheck(GameObject obj, ref bool cant, ref bool off) //전기 나무 확인
        {
            if ((electricityTree.value & (1 << obj.layer)) != 0) //나무
            {
                _isMe = true;
                _canElectricity = false;
            }
            else if(_isMe)
            {
                _canElectricity = true;
            }

            if (IsStart&&(fire.value & (1 << obj.layer)) != 0) //불
            {
                Destroy(obj);
                Destroy(gameObject);
            }
            
            if (obj.TryGetComponent(out WaterObj water))
            {
                if (!_canElectricity) //본인이 전기 통하는게 불가능
                {
                    if (water.OtherWaterCheck(WaterCheckTypEnum.Me) && water.OtherWaterCheck(WaterCheckTypEnum.Can)) //전기 못 통하게 한 원인이 이제 전기 통하는게 가능
                    {
                        _isMe = true;
                        _canElectricity = true;
                    }
                    else // 그냥 일반 물들
                    {
                        _isElectricity = false;
                        water.OtherWater(WaterCheckTypEnum.Can, false);
                    }
                }

                if (_isMe && _canElectricity) //is me 없애려고 작업하기
                {
                    if (!water.OtherWaterCheck(WaterCheckTypEnum.Can))
                    {
                        cant = true;
                    }
                }

                if (_isElectricity) //전기 통하는 중
                {
                    if (water.OtherWaterCheck(WaterCheckTypEnum.Delay))
                    {
                        water.OtherWater(WaterCheckTypEnum.Delay, true);
                        water.OtherWater(WaterCheckTypEnum.On, false);
                    }
                    else
                    {
                        water.OtherWater(WaterCheckTypEnum.On, true);
                    }
                }

                if (_isDelay) //시간 다 됨.
                {
                    if (water.OtherWaterCheck(WaterCheckTypEnum.On))
                    {
                        off = false;
                    }
                }
                
            }
        }

        public void Electricity() //전기 통하기
        {
            OtherWater(WaterObj.WaterCheckTypEnum.On, true);
            _isDelay = false;
            StopAllCoroutines();
            StartCoroutine(ElectricityDelay());
        }

        public enum WaterCheckTypEnum
        {
            Me,
            Can,
            Delay,
            On
        }
        
        public void OtherWater(WaterCheckTypEnum type, bool v)
        {
            switch (type)
            {
                case WaterCheckTypEnum.Me :
                    _isMe = v;
                    break;
                case WaterCheckTypEnum.Can :
                    _canElectricity = v;
                    break;
                case WaterCheckTypEnum.Delay :
                    _isDelay = v;
                    break;
                case WaterCheckTypEnum.On :
                    _isElectricity = v;
                    break;
                default:
                    break;
            }
        }

        public bool OtherWaterCheck(WaterCheckTypEnum type) //전기 통하고 있는지
        {
            return type switch
            {
                WaterCheckTypEnum.Me => _isMe,
                WaterCheckTypEnum.Can => _canElectricity,
                WaterCheckTypEnum.Delay => _isDelay,
                WaterCheckTypEnum.On => _isElectricity,
                _ => false
            };
        }
        
    }
    
    
    
    
}