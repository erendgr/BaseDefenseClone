using Enums;
using Managers;
using UnityEngine;

namespace Controllers
{
    public class BarrierPhysicController : MonoBehaviour
    {
        #region Self Variables

        #region SerializeField Variables

        [SerializeField] private BarrierManager manager;

        #endregion

        #region Private Variables

        private readonly string _player = "Player";
        private readonly string _soldier = "Soldier";
        private readonly string _moneyWorker = "MoneyWorker";
        private readonly string _hostage = "Hostage";

        #endregion

        #endregion

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(_player))
            {
                manager.BarrierState = BarrierEnum.Open;
            }

            if (other.CompareTag(_soldier))
            {
                manager.BarrierState = BarrierEnum.Open;
            }

            if (other.CompareTag(_moneyWorker))
            {
                manager.BarrierState = BarrierEnum.Open;
            }

            if (other.CompareTag(_hostage))
            {
                manager.BarrierState = BarrierEnum.Open;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_player))
            {
                manager.BarrierState = BarrierEnum.Close;
            }

            if (other.CompareTag(_soldier))
            {
                manager.BarrierState = BarrierEnum.Close;
            }

            if (other.CompareTag(_moneyWorker))
            {
                manager.BarrierState = BarrierEnum.Close;
            }

            if (other.CompareTag(_hostage))
            {
                manager.BarrierState = BarrierEnum.Close;
            }
        }
    }
}