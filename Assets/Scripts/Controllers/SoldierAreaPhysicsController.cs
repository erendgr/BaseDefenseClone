using Managers;
using UnityEngine;

namespace Controllers
{
    public class SoldierAreaPhysicsController : MonoBehaviour
    {
        [SerializeField] private SoldierAreaManager manager;
        private readonly string _player = "Player";

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_player))
            {
                manager.PlayerEntrySoldierArea();
            }
        }
    }
}