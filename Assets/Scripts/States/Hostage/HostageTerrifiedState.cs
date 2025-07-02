using Abstract;
using AIBrain;
using Enums;
using Signals;
using UnityEngine;

namespace States.Hostage
{
    public class HostageTerrifiedState : HostageBaseStates
    {
        #region Self Variables

        #region Private Variables

        private HostageAIBrain _manager;
        private readonly string _player = "Player";

        #endregion

        #endregion

        public HostageTerrifiedState(ref HostageAIBrain manager)
        {
            _manager = manager;
        }

        public override void EnterState()
        {
            _manager.AnimTriggerState(HostageAnimState.Terrified);
        }

        public override void UpdateState()
        {
        }

        public override void OnTriggerEnterState(Collider other)
        {
            if (other.CompareTag(_player))
            {
                _manager.SwitchState(HostageStates.FollowPlayer);
                IdleSignals.Instance.onHostageCollected?.Invoke(_manager.gameObject);
            }
        }
    }
}