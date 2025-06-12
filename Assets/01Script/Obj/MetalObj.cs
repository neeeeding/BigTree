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
        [SerializeField] private KeyManager.ElementType keyType; //열쇠 종류
        [SerializeField] private MetalObj[]  metalObjs; //상자 일 때 채워주기
        [SerializeField] private GameObject keyObject; //열쇠
        [SerializeField] private DialogManager  dialogManager; //대화 (완료 유무)
        [Header("Need")]
        [SerializeField] private new GameObject light; //확인 빛
        [SerializeField] private KeyManager key;

        private bool _isAlarm; //알려줬는지
        private bool _isAll; //동시에 모두 전기가 통한적이 있었는지
        private bool _me;
        protected override void Awake()
        {
            base.Awake();
            _me = false;
            _isAll =false;
            _isAlarm =false;
        }
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
            else
            {
                _me = false;
            }
            if (keyObject)
            {
                keyObject.SetActive(_isAll);
            }
            
            light.SetActive(_isAll || _me);
            
            foreach (MetalObj sc in metalObjs)
            {
                if (!sc.IsMe())
                {
                    return;   
                }
            }
            
            
            if(isBox)
                _isAll = true;

            if (isBox && _isAll && !_isAlarm)
            {
                dialogManager.DoDialog(new []{"모든 상자를 연결 했다!"});
                _isAlarm = true;
            }
        }

        public bool CheckAll() //완료 했는지 확인
        {
            return _isAll;
        }

        public bool IsMe() //일단 본인은?
        {
            return _me;
        }
        
        private void WaterCheck(GameObject obj) //전기 통하는 물 확인
        {
            if (obj.TryGetComponent(out WaterObj water))
            {
                _me =water.OtherWaterCheck(WaterObj.WaterCheckTypEnum.On);
            }
        }
    }
}