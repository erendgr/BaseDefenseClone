using UnityEngine;

namespace Abstract
{
    public abstract class HostageBaseStates
    {
        public abstract void EnterState();

        public abstract void UpdateState();

        public abstract void OnTriggerEnterState(Collider other);
    }
}