using Abstract;
using AIBrain;
using Enums;
using UnityEngine;
using UnityEngine.AI;

namespace States.Miners
{
    public class MoveToMineState : MinerBaseState
    {
        #region Self Variables

        #region Private Variables

        private MinerAIBrain _manager;
        private NavMeshAgent _agent;
        private NavMeshObstacle _obstacle;
        private readonly string _mine = "Mine";

        #endregion

        #endregion

        public MoveToMineState(ref MinerAIBrain manager, ref NavMeshAgent agent, ref NavMeshObstacle obstacle)
        {
            _manager = manager;
            _agent = agent;
            _obstacle = obstacle;
        }

        public override void EnterState()
        {
            _manager.PickaxeController(true);
            _manager.AnimState(MinerAnimState.Run);
            _agent.SetDestination(_manager.Target.transform.position);
        }

        public override void OnTriggerEnterState(Collider other)
        {
            if (other.CompareTag(_mine))
            {
                _agent.enabled = false;
                _obstacle.enabled = true;
                _manager.SwitchState(MinerStates.DigMine);
            }
        }
    }
}