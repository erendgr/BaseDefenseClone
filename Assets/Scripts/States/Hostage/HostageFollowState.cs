using Abstract;
using AIBrain;
using Enums;
using Signals;
using UnityEngine;
using UnityEngine.AI;

namespace States.Hostage
{
    public class HostageFollowState : HostageBaseStates
    {
        #region Self Variables

        #region Private Variables

        private HostageAIBrain _manager;
        private NavMeshAgent _agent;

        #endregion

        #endregion

        public HostageFollowState(ref HostageAIBrain manager, ref NavMeshAgent agent)
        {
            _manager = manager;
            _agent = agent;
        }

        public override void EnterState()
        {
            _manager.Target = StackSignals.Instance.onGetHostageTarget(_manager.gameObject);
            _manager.AnimTriggerState(HostageAnimState.Idle);
            _agent.SetDestination(_manager.Target.transform.position);
        }

        public override void UpdateState()
        {
            FlowPlayer(_manager);
        }

        public override void OnTriggerEnterState(Collider other)
        {
        }

        private void FlowPlayer(HostageAIBrain manager)
        {
            _agent.SetDestination(manager.Target.transform.position);
            manager.AnimFloatState(_agent.velocity.magnitude);
        }
    }
}