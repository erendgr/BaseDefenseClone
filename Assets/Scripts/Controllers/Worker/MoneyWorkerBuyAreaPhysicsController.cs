using Managers;
using UnityEngine;

namespace Controllers.Worker
{
    public class MoneyWorkerBuyAreaPhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region SerializeField Variables

        [SerializeField] private MoneyWorkerBuyAreaManager areaManager;

        #endregion
        
        #region Private Variables

        private readonly string _player = "Player";

        #endregion

        #endregion
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_player))
            {
                areaManager.BuyAreaEnter();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_player))
            {
                areaManager.BuyAreaExit();
            }
        }
    }
}