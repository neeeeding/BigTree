using UnityEngine;

namespace _01Script.Player.States
{
    public class FallState :State
    {
        private bool isGrounded;
        
        public FallState(Player player, Animator animator, int hash) : base(player, animator, hash)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            isGrounded = false;
            _player.InputController.onMovement += Move;
        }

        public override void Update()
        {
            base.Update();
            if (_player.IsGround)
            {
                isGrounded =  true;
            }
        }

        private void Move(Vector2 move)
        {
            if (isGrounded)
            {
                if (move == Vector2.zero) //안 움직임
                {
                    _player.ChangeState("IDLE");
                }
                else
                {
                    _player.ChangeState("MOVE");
                }
            }
        }

        public override void Exit()
        {
            base.Exit();
            _player.InputController.onMovement -= Move;
        }
    }
}