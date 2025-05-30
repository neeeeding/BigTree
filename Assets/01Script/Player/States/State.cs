using UnityEngine;

namespace _01Script.Player.States
{
    public class State
    {
        protected Animator _animator;
        protected int _animHash;

        public State(Animator animator, int hash)
        {
            _animator = animator;
            _animHash = hash;
        }

        public virtual void Enter()
        {
            _animator.SetBool(_animHash, true);
        }

        public virtual void Update()
        {
        }

        public virtual void Exit()
        {
            _animator.SetBool(_animHash, false);
        }
    }
}