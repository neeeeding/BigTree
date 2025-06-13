using System;
using _01Script.Player.States;
using UnityEngine;
using UnityEngine.XR;
using StateMachine = _01Script.Player.States.StateMachine;

namespace _01Script.Player
{
    public class Player : MonoBehaviour
    {
        [Header("Show")]
        [SerializeField] private string curState; //현재 상태 (확인용)
        [Header("Need")]
        [SerializeField] private StateSO[] states; //상태들

        [field: SerializeField] public LayerMask attackLayer { get; private set; } // 어택 가능한
        [field:SerializeField]public PlayerInputSO InputController { get; private set; } // 인 풋
        [field:SerializeField]public Attack Attack{ get; private set; } //공격에 대한 정보(이펙트)

        public bool IsGround => _controller.isGrounded; //땅 닿았는지
        private CharacterController _controller; //땅 체크를 위해
        
        private Animator _animator; //애니메이션
        private StateMachine _stateMachine; //상태 바꿔줌
        
        private void Awake()
        {
            _controller = GetComponentInChildren<CharacterController>();
            _animator = GetComponentInChildren<Animator>();
        //     _stateMachine = new StateMachine(this,_animator,states);
        //     _stateMachine.Init("IDLE");=
        
        
        InputController.onAttack += DoAttack;
        }

        private bool isAttack;
        private Vector3? mousePos;
        private void DoAttack()
        { 
            isAttack = false;
            MousePos();
            if (mousePos != null)
            {
                Attack.AttackEffect( mousePos.Value);
                    
            }
            isAttack = true;
        }

        private void MousePos() //마우스 위치 찾기
        {           
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            float maxDistance = 100f; // 광선 최대 거리

            if (Physics.Raycast(ray, out hit, maxDistance,~0, QueryTriggerInteraction.Ignore))
            {
                if (((1 << hit.collider.gameObject.layer) & attackLayer) != 0)
                {
                    Vector3 contactPoint = hit.point; // 실제 충돌 위치
        
                    Debug.DrawLine(ray.origin, contactPoint, Color.red, 1f);
                    mousePos = contactPoint;
                    return;
                }
            }


            mousePos = null;
        }
        
        public void ChangeState(String stateName)
        {
            _stateMachine.ChanageState(stateName);
        }
        
        // private void Update()
        // {
        //     curState = _stateMachine.stateName;
        //     _stateMachine.UpdateState();
        // }
    }
}
