using Managers;
using UnityEngine;

namespace Controllers.Player
{
    public class PlayerStackPhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables

        public bool isEnable = true;
        private readonly string _money = "Money";
        private readonly string _barrierInSide = "BarrierInSide";
        private readonly string _ammoReloadArea = "AmmoReloadArea";
        private readonly string _turretAmmoArea = "TurretAmmoArea";

        #endregion

        #region Serialized Variables

        [SerializeField] private PlayerStackManager stackManager;

        #endregion

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (!isEnable) return;

            // if (other.CompareTag(_barrierInSide))
            // {
            //     stackManager.InteractBarrierArea();
            //     return;
            // }

            // if (other.CompareTag(_money))
            // {
            //     stackManager.InteractMoney(other.gameObject);
            //     return;
            // }

            if (other.CompareTag(_ammoReloadArea))
            {
                stackManager.InteractWareHouseArea(other.transform, true);
                return;
            }

            if (other.CompareTag(_turretAmmoArea))
            {
                // stackManager.InteractTurretAmmoArea(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_ammoReloadArea))
            {
                stackManager.InteractWareHouseArea(default, false);
            }

            if (other.CompareTag(_turretAmmoArea))
            {
                // StackSignals.Instance.onDecreseStackHolder?.Invoke(other.gameObject);
                // stackManager.CheckAmmoStack();
            }
        }
    }
}