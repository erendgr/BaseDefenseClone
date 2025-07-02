using Managers;
using UnityEngine;

namespace Controllers
{
    public class MineAreaPhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region SerializeField Variables

        [SerializeField] private MineAreaManager manager;

        #endregion

        #region Private Variables

        private readonly string _player = "Player";

        #endregion
        
        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_player))
            {
                manager.PlayerEntryGemArea();
            }
        }
    }
}