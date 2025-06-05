using UnityEngine;

namespace _01Script.Player.States
{
    public class JumpState : State
    {
        private Vector3 curPos; //현재 위치
        public JumpState(Player player, Animator animator, int hash) : base(player, animator, hash)
        {
        }

        public override void Enter()
        {
            base.Enter();
            curPos = _player.transform.position;
        }

        public override void Update()
        {
            base.Update();
            if (curPos.y > _player.transform.position.y)
            {
                _player.ChangeState("FALL");
            }
            curPos = _player.transform.position;
        }
    }
}