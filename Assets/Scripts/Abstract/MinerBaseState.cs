using UnityEngine;

namespace Abstract
{
    public abstract class MinerBaseState
    {
        public abstract void EnterState();

        public abstract void OnTriggerEnterState(Collider other);
    }
}