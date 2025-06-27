using Managers;
using UnityEngine;

namespace Controllers.Turret
{
    public class TurretControlPhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private TurretManager manager;
        
        #endregion

        #region Private Variables

        private readonly string _player = "Player";

        #endregion
        
        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_player))
            {
                GameObject player = other.transform.parent.gameObject;
                manager.InteractPlayerWithTurret(player);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_player))
            {
                manager.NotInteractPlayerWithTurret();
            }
        }
    }
}