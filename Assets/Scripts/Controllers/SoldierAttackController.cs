using Signals;
using UnityEngine;

namespace Controllers
{
    public class SoldierAttackController : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables
        
        private readonly string _player = "Player";

        #endregion

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_player))
            {
                WorkerSignals.Instance.onSoldierAttack?.Invoke();
            }
        }
    }
}