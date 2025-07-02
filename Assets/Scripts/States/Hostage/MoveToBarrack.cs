using _ObjectPooling.Scripts.Enums;
using _ObjectPooling.Scripts.Signals;
using Abstract;
using AIBrain;
using Signals;
using UnityEngine;
using UnityEngine.AI;

namespace States.Hostage
{
    public class MoveToBarrack : HostageBaseStates
    {
        #region Self Variables

        #region Private Variables

        private HostageAIBrain _manager;
        private NavMeshAgent _agent;
        private readonly string _soldierBarrack = "SoldierBarrack";

        #endregion

        #endregion

        public MoveToBarrack(ref HostageAIBrain manager, ref NavMeshAgent agent)
        {
            _manager = manager;
            _agent = agent;
        }

        public override void EnterState()
        {
            _manager.Target = IdleSignals.Instance.onGetSoldierBarrack();
            _agent.SetDestination(_manager.Target.transform.position);
        }

        public override void UpdateState()
        {
            _manager.AnimFloatState(_agent.velocity.magnitude);
        }

        public override void OnTriggerEnterState(Collider other)
        {
            if (other.CompareTag(_soldierBarrack))
            {
                IdleSignals.Instance.onHostageEntryBarrack?.Invoke();
                PoolSignals.Instance.onEnqueuePooledGameObject(_manager.gameObject, PoolType.Hostage);
            }
        }
    }
}