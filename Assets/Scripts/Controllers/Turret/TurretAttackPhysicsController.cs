using Managers;
using UnityEngine;

namespace Controllers.Turret
{
    public class TurretAttackPhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        //daha sonra TurretAttackManager olacak
        [SerializeField] private TurretManager manager;

        #endregion

        #region Private Variables

        private readonly string _enemy = "Enemy";
        private readonly string _player = "Player"; //for test

        #endregion

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_player))
            {
                manager.Target = other.gameObject;
                manager.EnemyList.Add(other.gameObject);
                manager.StartAttack();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_player))
            {
                manager.EnemyList.Remove(other.gameObject);
            }
        }
    }
}