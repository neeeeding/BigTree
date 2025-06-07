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
        [SerializeField] private LayerMask end; // 코너 레이어
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


        private BoxCollider col; //자신의 콜라이더
        private bool _isEnd; //끝에 도달 했는지
        private bool _isUp; //true : 위에 걸림 /false : 아래에 걸림
        private bool _isRight; //true : 오른쪽에 걸림 /false : 왼쪽에 걸림
        private bool _isCoroutine; //true :코루틴실행
        private bool _isFollow; //true : 선 따라가기 / false : 따라오지 말기
        private int _lineCount; //선 개수
        private Stack<RaycastHit> _corners; //코너들 (겹치는거 없애려고)
        private Dictionary<RaycastHit, bool> _canAttack; //줄의 위치에 따라

        private void Awake()
        {
            col = targetObject.GetComponent<BoxCollider>();
            _canAttack =  new Dictionary<RaycastHit, bool>();
            _corners = new Stack<RaycastHit>();
            _canvasMinPos = Vector2.zero;
            _canvasMaxPos = new Vector2(1920, 1080);
            line.useWorldSpace = true;
            _lineCount = 1;
            _isFollow = false;
            ResetLine();
        }

        private void Update()
        {
            if (_isFollow)
            {
                FollowLine();
                if (!_isCoroutine)
                {
                    StopAllCoroutines();
                    StartCoroutine(DeleteLine());
                }
            }
            
            if (Input.GetMouseButtonDown(0) && MousePos())
            {
                _isEnd = false;
                _isFollow = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                _isFollow = false;
                CheckEnd();
                if (_isEnd)
                {
                    CanAttack();
                    return;
                }
                ResetLine();
            }
        }

        private void CheckEnd() //끝에 닿았는지
        {
            Vector3 center = col.transform.position;
            Vector3 size = Vector3.Scale(col.size, col.transform.lossyScale);
            
            if (Physics.CheckBox(center, size * 0.5f, col.transform.rotation, end))
            {
                _isEnd = true;
            }
        }
        
        private void CanAttack() //가능한 공격 판별
        {
            bool f = false;
            bool w = false;
            bool e = false;
            foreach (RaycastHit obj in _corners)
            {
                if (obj.transform.gameObject.layer == LayerMask.NameToLayer("Fire") && _canAttack[obj])
                {
                    f = true;
                }
                if (obj.transform.gameObject.layer == LayerMask.NameToLayer("Waterobj")&& _canAttack[obj])
                {
                    w = true;
                }
                if (obj.transform.gameObject.layer == LayerMask.NameToLayer("Electricity")&& _canAttack[obj])
                {
                    e = true;
                }
            }
            attack.CanAttacck(f,w,e);
            
        }

        private void CornerLine(RaycastHit center,Vector3 point) //코너에 닿을 때 (선 개수 늘리기)
        {
            if(point == Vector3.zero)
                return;
            
            _isUp = center.transform.position.y < point.y;
            _isRight = center.transform.position.x < point.x;
            _canAttack.Add(center, !_isUp);
            
            line.SetPosition(_lineCount, point);
            line.positionCount++;
            _lineCount++;
        }

        private IEnumerator DeleteLine() //코너 안 걸림
        {
            _isCoroutine = true;
            yield return new WaitForSeconds(0.1f);

            if (_lineCount >= 2)
            {

                if (CheckLine(_lineCount)) //선 걸리나 안걸리나
                {
                    line.positionCount--;
                    _lineCount--;
                    _corners.Pop();
                }
            }
            if(_lineCount >= 3)
            {
                int start = _lineCount - 2;
                Vector3 startPos  = line.GetPosition(start);  // 짧은 선 없애기 (보류)
                Vector3 endPos = line.GetPosition(_lineCount - 1);
                float direction = (startPos - endPos).magnitude;

                if (Mathf.Abs(direction) < 0.1f)
                {
                    line.positionCount--;
                    _lineCount--;
                }
            }
            _isCoroutine = false;
        }

        private bool CheckLine(int n) //선 확인 하기 (지울 지)
        {
            Vector3 endPos = line.GetPosition(n -1);
            Vector3 myPos = line.GetPosition(n);

            bool isUp = endPos.y < myPos.y;
            bool isRight = endPos.x < myPos.x;
            
            return (_isUp == isUp && _isRight == isRight); 

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
            line.SetPosition(_lineCount, worldPos);
            line.SetPosition(0, line.gameObject.transform.position); //시작점
            
             DrawLineRay();
            
        }

        private void DrawLineRay() //선의 레이 쏘기
        {   
            // 시작점과 끝점 가져오기
            Vector3 startPos = line.GetPosition(_lineCount - 1);
            Vector3 endPos = line.GetPosition(_lineCount);
            Vector3 direction = (endPos - startPos).normalized;  // 정규화된 방향벡터
            Ray ray = new Ray(startPos, direction);
            
            Debug.DrawLine(startPos, endPos, Color.red, 1f);
            
            RaycastHit[] hitBuffer = new RaycastHit[10];
            int hitCount = Physics.RaycastNonAlloc(ray, hitBuffer, 100, corner);

            if (hitCount <= 0)
            {
                return;
            }

            for(int i = 0; i < hitCount; i++)
            {
                RaycastHit hit = hitBuffer[i];
                bool isHit = true;
                foreach (RaycastHit h in _corners)
                {
                    if ( hit.transform == h.transform) //이미 걸린 친구?
                    {
                        isHit = false;
                    }
                    
                }
                
                if ((_corners.Count <= 0 || isHit))
                {
                    _corners.Push(hit);
                    CornerLine(hit,hit.point);
                }
            }
        }
        
        private void ResetLine() //선 위치 리셋
        {
            _lineCount = 1;
            line.positionCount = 2;
            _corners.Clear();
            
            targetObject.transform.position = basicPos.position;

            line.SetPosition(_lineCount, basicPos.position);
            line.SetPosition(0, line.gameObject.transform.position); //시작점
            DrawLineRay();
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
