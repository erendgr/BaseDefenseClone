using Abstract;
using AIBrain;
using Enums;
using Signals;
using UnityEngine;
using UnityEngine.AI;

namespace States.AmmoWorker
{
    public class MoveToWareHouse : AmmoWorkerBaseState
    {
        #region Self Variables

        #region Private Variables

        private AmmoWorkerAIBrain _manager;
        private NavMeshAgent _agent;
        private readonly string _ammoReloadArea = "AmmoReloadArea";

        #endregion

        #endregion

        public MoveToWareHouse(ref AmmoWorkerAIBrain manager, ref NavMeshAgent agent)
        {
            _manager = manager;
            _agent = agent;
        }

        public override void EnterState()
        {
            _manager.Target = IdleSignals.Instance.onGetWareHousePositon().gameObject;
            _agent.SetDestination(_manager.Target.transform.position);
            _manager.AnimTriggerState(WorkerAnimState.Walk);
        }

        public override void OnTriggerEnterState(Collider other)
        {
            if (other.CompareTag(_ammoReloadArea))
            {
                _manager.SwitchState(AmmoWorkerStates.WaitForFullStack);
            }
        }
    }
}