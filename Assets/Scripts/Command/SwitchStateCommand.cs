using System.Collections.Generic;
using Abstract;
using Enums;
using UnityEngine;

namespace Command
{
    public class SwitchStateCommand
    {
        private readonly Dictionary<AmmoWorkerStates, AmmoWorkerBaseState> _states;

        public SwitchStateCommand(Dictionary<AmmoWorkerStates, AmmoWorkerBaseState> states)
        {
            _states = states;
        }

        public AmmoWorkerBaseState Execute(AmmoWorkerStates state)
        {
            if (_states.TryGetValue(state, out var newState))
            {
                return newState;
            }

            Debug.LogWarning($"State not found: {state}");
            return null;
        }
    }
}