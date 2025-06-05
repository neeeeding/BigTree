using System;
using UnityEngine;

namespace _01Script.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField] private float moveSpeed = 8f;
        [SerializeField] private float runSpeed = 15f;
        [SerializeField] private float jumpPower = 15f;
        [SerializeField]private float terminalVelocity = -50f; // 최대 낙하 속도 제한
        [Header("Need")]
        [SerializeField] private PlayerInputSO _input;
    
        private float gravity = -9.8f;

        
        private bool isJump; //true : 점프 해 / false : 점프 가능
        private bool isRun; //true : 달려 / false : 걸어
        private bool IsGround => _controller.isGrounded;
        private float _verticalVelocity;
        private Vector3 _velocity;
        private Vector3 _movment;
        private CharacterController _controller;

        private void OnEnable()
        {
            isJump = false;
            _controller = GetComponent<CharacterController>();
            _input.onMovement += Move;
            _input.onJumpPressed += Jump;
            _input.onRunKey += Run;
        }

        private void Update()
        {
            _input.UpdateMove();
        }

        private void FixedUpdate()
        {
            ApplyGravity();

            _controller.Move(_velocity * Time.fixedDeltaTime);
        }

        private void ApplyGravity()
        {
            if (!isJump&&IsGround && _verticalVelocity < 0)
            {
                _verticalVelocity = -0.03f;
            }
            else if(!isJump)
            {
                _verticalVelocity += gravity * Time.fixedDeltaTime;

                // 너무 빠르게 떨어지지 않도록 제한
                if (_verticalVelocity < terminalVelocity)
                    _verticalVelocity = terminalVelocity;
            }
            if (isJump )
            { 
                _verticalVelocity = Mathf.MoveTowards(_verticalVelocity, jumpPower, runSpeed*Time.fixedDeltaTime);

                if (Mathf.Abs(jumpPower - _verticalVelocity) < 1f)
                {   
                    isJump = false; 
                }
            }

            _velocity = isRun?  _movment * runSpeed : _movment * moveSpeed;
            _velocity.y = _verticalVelocity;
        
        }

        private void Run(bool run)
        {
            isRun = run;
        }

        private void Jump()
        {
            if (IsGround && !isJump)
            {
                isJump = true;
            }
        }

        private void Move(Vector2 move)
        {
            _movment= new Vector3(move.x, 0, move.y);

            _movment = transform.TransformDirection(_movment); // 현재 바라보는 방향으로 변환
        }

        private void OnDisable()
        {
            _input.onMovement -= Move;
            _input.onJumpPressed -= Jump;
            _input.onRunKey -= Run;
        }
    }
}
