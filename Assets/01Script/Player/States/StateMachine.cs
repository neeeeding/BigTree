using System;
using System.Collections.Generic;
using UnityEngine;

namespace _01Script.Player.States
{
    public class StateMachine
    {
        public State curState { get; set; }
        private Dictionary<string, State> _stateList; //상태들

        public StateMachine(Animator anim,StateSO[] list)
        {
            _stateList = new Dictionary<string, State>();
            foreach (var state in list)
            {
                Type type = Type.GetType(state.className);
                Debug.Assert(type != null, $"Finding type is null : {state.className}");
                State st = Activator.CreateInstance(type, anim, state.animationHash) as State;
                _stateList.Add(state.stateName, st);
            }
        }

        public void ChanageState(string stateName)
        {
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