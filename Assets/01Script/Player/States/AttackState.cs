using UnityEngine;

namespace _01Script.Player.States
{
    public class AttackState : State
    {
        private bool isAttack; //true : 공격함 / false : 공격 중
        
        public AttackState(Player player, Animator animator, int hash) : base(player, animator, hash)
        {
        }
        public override void Enter()
        {
            base.Enter();

            _player.Attack.AttackEffect(true, false, false);
            
            isAttack = false;
            
            _player.InputController.onMovement += Move;
            _player.InputController.onJumpPressed += Jump;
        }

        private void FinishAttack() //공격을 완료함
        {
            _player.Attack.StopAttack();
            isAttack = true;
        }
        private void Jump()
        {
            if (isAttack)
            {
                _player.ChangeState("JUMP");   
            }

        }

        private void Move(Vector2 move)
        {
            if (isAttack)
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
            FinishAttack();
            base.Exit();
            _player.InputController.onMovement -= Move;
            _player.InputController.onJumpPressed -= Jump;
        }
    }
}