using UnityEngine;

namespace _01Script.Player.States
{
    public class AttackState : State
    {
        private bool isAttack; //true : 공격함 / false : 공격 중
        private Vector3? mousePos;
        
        public AttackState(Player player, Animator animator, int hash) : base(player, animator, hash)
        {
        }
        public override void Enter()
        {
            isAttack = false;
            base.Enter();

            MousePos();
            if (mousePos != null)
            {
                _player.Attack.AttackEffect( mousePos.Value);
                
            }
            isAttack = true;
            _player.InputController.onMovement += Move;
            _player.InputController.onJumpPressed += Jump;
        }

        private void MousePos() //마우스 위치 찾기
        {           
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            float maxDistance = 100f; // 광선 최대 거리

            if (Physics.Raycast(ray, out hit, maxDistance, _player.attackLayer))
            {
                Vector3 contactPoint = hit.point; // 실제 충돌 위치
                GameObject hitObj = hit.collider.gameObject;
    
                Debug.DrawLine(ray.origin, contactPoint, Color.red, 1f);
                mousePos = contactPoint;
                return;
            }


            mousePos = null;
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
            base.Exit();
            _player.InputController.onMovement -= Move;
            _player.InputController.onJumpPressed -= Jump;
        }
    }
}