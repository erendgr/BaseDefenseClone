using Abstract;
using AIBrain;
using UnityEngine;

namespace States.Miners
{
    public class DigMineState : MinerBaseState
    {
        #region Self Variables

        #region Private Variables

        private MinerAIBrain _manager;

        #endregion

        #endregion

        public DigMineState(ref MinerAIBrain manager)
        {
            _manager = manager;
        }

        public override void EnterState()
        {
            _manager.transform.LookAt(_manager.Target.transform);
            _manager.DigWaiter();
        }

        public override void OnTriggerEnterState(Collider other)
        {
        }
    }
}