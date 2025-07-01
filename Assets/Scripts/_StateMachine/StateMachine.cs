using System.Collections.Generic;
using UnityEngine;

namespace _StateMachine
{
    public class StateMachine<TStateEnum, TState> where TState : class
    {
        private readonly Dictionary<TStateEnum, TState> _states;
        private TState _currentState;

        public StateMachine(Dictionary<TStateEnum, TState> states)
        {
            _states = states;
        }

        public TState Switch(TStateEnum newState)
        {
            if (_states.TryGetValue(newState, out var nextState))
            {
                _currentState = nextState;
                return nextState;
            }

            Debug.LogWarning($"State not found: {newState}");
            return null;
        }
    }
}