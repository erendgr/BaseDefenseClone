using Abstract;
using AIBrain;
using Enums;
using UnityEngine;
using UnityEngine.AI;

namespace States.Miners
{
    public class MoveToGemHolder : MinerBaseState
    {
        #region Self Variables

        #region Private Variables

        private MinerAIBrain _manager;
        private NavMeshAgent _agent;
        private readonly string _gemHolder = "GemHolder";

        #endregion

        #endregion

        public MoveToGemHolder(ref MinerAIBrain manager, ref NavMeshAgent agent)
        {
            _manager = manager;
            _agent = agent;
        }

        public override void EnterState()
        {
            _manager.DiamondController(true);
            _manager.AnimState(MinerAnimState.Transport);
            _agent.SetDestination(_manager.GemAreaHolder.transform.position);
        }

        public override void OnTriggerEnterState(Collider other)
        {
            if (other.CompareTag(_gemHolder))
            {
                _manager.GemHolderWaiter();
            }
        }
    }
}