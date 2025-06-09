using System;
using System.Collections.Generic;
using _01Script.Manager;
using _01Script.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace _01Script.Skill
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
        private Dictionary<MetalObj, bool> _all; //전체가 들어있는
        protected override void Awake()
        {
            base.Awake();
            
            FillDictionary();
            
            me = false;
            isAll =false;
        }

        private void Update()
        {
            light.SetActive(isAll || me);
            
            foreach (bool sc in _all.Values)
            {
                if (!sc)
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
                    dialogManager.DoDialog(new string[]{"모든 상자를 연결해야 할 것 같다."});
                }
            }
        }

        public bool CheckAll() //완료 했는지 확인
        {
            return isAll;
        }
        
        private void WaterCheck(GameObject obj) //전기 통하는 물 확인
        {
            if (obj.TryGetComponent(out WaterObj water))
            {
                me =water.OtherWaterCheck(WaterObj.WaterCheckTypEnum.On);
                _all[this] = me;
            }
        }

        private void FillDictionary()
        {
            _all = new Dictionary<MetalObj, bool>();

            foreach (MetalObj obj in metalObjs)
            {
                _all.Add(obj, false);
            }
        }
    }
}