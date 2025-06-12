using System;
using System.Collections;
using System.Collections.Generic;
using _01Script.Player;
using UnityEngine;

namespace _01Script.ObjUI
{
    public class DrawLine : MonoBehaviour
    {
        [Header("Setting")]
        //지정값
        [SerializeField] private  Transform worldMinPos; // (0, 0)
        [SerializeField] private Transform worldMaxPos; // (x, y)
        [SerializeField] private Transform basicPos; // 기본 위치
        [SerializeField] private LayerMask corner; // 코너 레이어
        [Header("Need")]
        [SerializeField] private GameObject targetObject; //(콘센트)오브제
        [SerializeField] private LineRenderer line; //줄
        [SerializeField] private Attack attack;

        //캔버스 위치
        private Vector2 _canvasMinPos;
        private Vector2 _canvasMaxPos;
        //보정 위치 (0 ~ 1)
        private float _targetPosX;
        private float _targetPosY;
        
        
        private bool f;
        private bool w;
        private bool e;
        
        private bool _isFollow; //true : 선 따라가기 / false : 따라오지 말기

        private void Awake()
        {
            
            f = false;
            w = false;
            e = false;
            
            _canvasMinPos = Vector2.zero;
            _canvasMaxPos = new Vector2(1920, 1080);
            line.useWorldSpace = true;
            _isFollow = false;
            ResetLine();
        }

        private void Update()
        {
            SetLineColor();
            
            if (_isFollow) //마우스 따라가기
            {
                FollowLine();
            }
            else
            {
                ResetLine(); //리셋
            }
            
            if (Input.GetMouseButtonDown(0) && MousePos()) //전선을 잡았는지
            {
                _isFollow = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                _isFollow = false;
            }
        }

        private void SetLineColor() //선색 바꾸기
        {
            if (f)
            {
                line.startColor = Color.red;
                line.endColor = Color.red;   
            }
            else if (w)
            {
                line.startColor = Color.blue;
                line.endColor = Color.blue;   
            }
            else if (e)
            {
                line.startColor = Color.yellow;
                line.endColor = Color.yellow;   
            }
            else
            {
                line.startColor = Color.black;
                line.endColor = Color.black; 
            }
        }
        
        private bool CanAttack( RaycastHit obj) //가능한 공격 판별
        {
            f = false;
            w = false;
            e = false;
            
            if (obj.transform.gameObject.layer == LayerMask.NameToLayer("Fb"))
            {
                f = true;
            }
            if (obj.transform.gameObject.layer == LayerMask.NameToLayer("Wb"))
            {
                w = true;
            }
            if (obj.transform.gameObject.layer == LayerMask.NameToLayer("Eb"))
            {
                e = true;
            }
            
            attack.CanAttacck(f,w,e);
            
            return f || w || e;
        }
        
        private void FollowLine() //선이 마우스 따라오기
        {
            Vector2 mousePos = Input.mousePosition;

            //보정 값 찾기
            float normalizedX = Mathf.InverseLerp(_canvasMinPos.x, _canvasMaxPos.x, mousePos.x);
            float normalizedY = Mathf.InverseLerp(_canvasMinPos.y, _canvasMaxPos.y, mousePos.y);

            //회전 때문에
            Vector3 localMin = worldMinPos.localPosition;
            Vector3 localMax = worldMaxPos.localPosition;

            
            Vector3 targetLocalPos = new Vector3(
                Mathf.Lerp(localMin.x, localMax.x, normalizedX),
                Mathf.Lerp(localMin.y, localMax.y, normalizedY),
               0.5f
            );

            //콘센트 위치 정하기
            Transform commonParent = worldMinPos.parent;
            Vector3 worldPos = commonParent.TransformPoint(targetLocalPos);

            
            targetObject.transform.position = worldPos;

            // 선 위치 설정 (월드 기준)
            line.SetPosition(1, worldPos);
            line.SetPosition(0, line.gameObject.transform.position); //시작점
            
             DrawLineRay(); // 선 감지
            
        }

        private void DrawLineRay() //선의 레이 쏘기
        {   
            // 시작점과 끝점 가져오기
            Vector3 startPos = line.GetPosition(0);
            Vector3 endPos = line.GetPosition(1);
            Vector3 direction = (endPos - startPos).normalized;  // 정규화된 방향벡터
            Ray ray = new Ray(startPos, direction);
            
            Debug.DrawLine(startPos, endPos, Color.red, 1f);
            
            RaycastHit[] hitBuffer = new RaycastHit[10];
            int hitCount = Physics.RaycastNonAlloc(ray, hitBuffer, Vector3.Distance(startPos, endPos), corner);

            if (hitCount <= 0)
            {
                f = false;
                w = false;
                e = false;
                attack.CanAttacck(f,w,e);
                return;
            }

            for(int i = 0; i < hitCount; i++)
            {
                RaycastHit hit = hitBuffer[i];
                if (CanAttack(hit))
                {
                    return;
                }
            }
        }
        
        private void ResetLine() //선 위치 리셋
        {
            line.positionCount = 2;
            
            targetObject.transform.position = basicPos.position;

            line.SetPosition(1, basicPos.position);
            line.SetPosition(0, line.gameObject.transform.position); //시작점
        }
        
        private bool MousePos() //마우스가 전선 눌렀는지
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                return hit.collider.gameObject == targetObject;
            }
        
            return false;
        }

    }
}
