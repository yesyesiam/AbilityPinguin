using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM._Scripts
{
    public class Fsm
    {
        private FsmState StateCurrent { get; set; }

        private Dictionary<Type, FsmState> _states = new Dictionary<Type, FsmState>();

        public FsmState GetCurrentState()
        {
            return StateCurrent;
        }
        public void AddState(FsmState state)
        {
            _states.Add(state.GetType(), state);
        }

        public void SetState<T>() where T : FsmState
        {
            var type = typeof(T);
            if(StateCurrent!=null && StateCurrent.GetType() == type)
            {
                return;
            }

            if(_states.TryGetValue(type, out var newState))
            {
                StateCurrent?.Exit();

                StateCurrent = newState;

                StateCurrent.Enter();
            }
        }

        public T GetState<T>() where T : FsmState
        {
            var type = typeof(T);
            if (_states.TryGetValue(type, out var newState))
            {
                return newState as T;
            }
            return null;
        }
        public void Update()
        {
            StateCurrent?.Update();
        }
    }
}