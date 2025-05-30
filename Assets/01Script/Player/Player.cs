using System;
using _01Script.Player.States;
using UnityEngine;
using StateMachine = _01Script.Player.States.StateMachine;

namespace _01Script.Player
{
    public class Player : MonoBehaviour
    {
        [Header("Need")]
        [SerializeField] private StateSO[] states;
        
        private Animator _animator;
        private StateMachine _stateMachine;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        //     _stateMachine = new StateMachine(_animator,states);
        //     _stateMachine.ChanageState("IDLE");
        // }
        //
        // private void Update()
        // {
        //     _stateMachine.UpdateState();
        }
    }
}
