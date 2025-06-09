using System;
using System.Collections.Generic;
using _01Script.Obj;
using Unity.VisualScripting;
using UnityEngine;

namespace _01Script.Manager
{
    public class IceManager : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField] private int iceCount = 5; //얼음 개수 (총 말고)
        [SerializeField] private float delay = 3f; //딜레이
        [SerializeField] private Vector3 iceSize; //얼음 사이즈
        [SerializeField] private Vector3[] cantIce = {new Vector3(4,0,3),new Vector3(1,1,4), new Vector3(2,2,2), new Vector3(3,3,0), new Vector3(0,4,1)}; //금속 둘 곳
        [Header("Need")]
        [SerializeField] private GameObject icePrefab; //얼음
        [SerializeField] private MetalObj completeCheck; //완료 확인
        [SerializeField] private Transform basicPos; //생성 위치
        [SerializeField] private GameObject wall; //벽
        
        private Dictionary<IceObj, Vector3> _all; //모든 얼음과 위치
        private float _delayTime; //시간 재기
        private Stack<IceObj> _revertIce; //되돌리려는 얼음들

        private void Awake()
        {
            _revertIce = new Stack<IceObj>();
            SetDictionary();
        }

        private void Update()
        {
            CheckComplete();
            
            if (_revertIce.Count > 0)
            {
                _delayTime += Time.deltaTime;
                if (_delayTime >= delay)
                {
                    IceObj ice = _revertIce.Pop();
                    ice.gameObject.SetActive(true);
                    
                    _delayTime = 0;
                }
            }
        }

        public void PushIce(IceObj obj) //얼음 비활성화
        {
            obj.gameObject.SetActive(false);
            _revertIce.Push(obj);
            _delayTime = 0;
        }

        private void CheckComplete() //완료 했는지 확인
        {
            if (completeCheck.CheckAll())
            {
                Destroy(wall.gameObject);
            }
        }
        
        private void SetDictionary() //얼음 세팅
        {
            _all = new Dictionary<IceObj, Vector3>();
            for (int x = 0; x < iceCount; x++)
            {
                for (int y = 0; y < iceCount; y++)
                {
                    for (int z = 0; z < iceCount; z++)
                    {
                        Vector3 pos = new Vector3(x, y, z);
                        bool isBox = false; //박스 위치인지
                        foreach (Vector3 v in cantIce)
                        {
                            if (Vector3.Distance(v, pos) < 0.01f)
                                isBox = true;
                        }

                        if (isBox)
                        {
                            continue;
                        }
                        
                        
                        GameObject ice = Instantiate(icePrefab, transform);
                        ice.transform.SetParent(transform);

                        if (ice.gameObject.transform.GetChild(0).TryGetComponent(out IceObj sc)) //넣기
                        {
                            _all.Add(sc, pos);
                            sc.SetParent(this);
                        }
                        
                        ice.transform.rotation = Quaternion.identity;
                        ice.transform.localPosition = basicPos.transform.localPosition;
                        ice.transform.localPosition += new Vector3(x * iceSize.x, y * iceSize.y, z * iceSize.z); //위치 정하기
                        
                        
                    }
                }
            }
        }
    }
}