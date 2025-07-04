﻿using UnityEngine;

namespace _01Script.Player.States
{
    public class MoveState : State
    {
        public MoveState(Player player, Animator animator, int hash) : base(player, animator, hash)
        {
        }
        public override void Enter()
        {
            base.Enter();
            _player.InputController.onMovement += Move;
            _player.InputController.onJumpPressed += Jump;
            _player.InputController.onAttack += Attack;
        }

        private void Attack()
        {
            _player.ChangeState("ATTACK");   
        }

        private void Jump()
        {
            _player.ChangeState("JUMP");
        }

        private void Move(Vector2 move)
        {
            if (move == Vector2.zero) //안 움직임
            {
                _player.ChangeState("IDLE");
            }
        }

        public override void Exit()
        {
            base.Exit();
            _player.InputController.onMovement -= Move;
            _player.InputController.onJumpPressed -= Jump;
            _player.InputController.onAttack -= Attack;
        }
    }
}