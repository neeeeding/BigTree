using System;
using System.Collections.Generic;
using UnityEngine;

namespace _01Script.Player.States
{
    public class StateMachine
    {
        public string stateName;
        public State curState { get; set; }
        private Dictionary<string, State> _stateList; //상태들

        public StateMachine(Player player,Animator anim,StateSO[] list)
        {
            _stateList = new Dictionary<string, State>();
            foreach (var state in list)
            {
                Type type = Type.GetType(state.className);
                Debug.Assert(type != null, $"Finding type is null : {state.className}");
                State st = Activator.CreateInstance(type, player,anim, state.animationHash) as State;
                _stateList.Add(state.stateName, st);
            }
        }

        public void Init(string stateName)
        {
            this.stateName = stateName;
            State nowState =  _stateList[stateName];
            curState = nowState;
            curState.Enter();
        }

        public void ChanageState(string stateName)
        {
            this.stateName = stateName;
            State nowState =  _stateList[stateName];

            if (curState != null && curState != nowState)
            {
                curState.Exit();
                curState = nowState;
                curState.Enter();
            }
        }

        public void UpdateState()
        {
            curState?.Update();
        }
        
    }
}