using Managers;
using UnityEngine;

namespace Controllers
{
    public class RoomBuyAreaPhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region SerializeField Variables

        [SerializeField] private RoomManager manager;

        #endregion

        #region Private Variables

        private readonly string _player = "Player";

        #endregion

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_player))
            {
                manager.BuyAreaEnter();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_player))
            {
                manager.BuyAreaExit();
            }
        }
    }
}