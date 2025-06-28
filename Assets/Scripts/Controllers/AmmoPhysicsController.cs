using _ObjectPooling.Scripts.Enums;
using _ObjectPooling.Scripts.Signals;
using UnityEngine;

namespace Controllers
{
    public class AmmoPhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private Rigidbody rb;

        #endregion

        #region Private Variables

        private readonly string _enemy = "Enemy";
        private readonly string _turretAttackRadius = "TurretAttackRadius";

        #endregion

        #endregion

        private void OnDisable()
        {
            rb.linearVelocity = Vector3.zero;
        }

        public void SetAddForce(Vector3 direct)
        {
            rb.AddForce(direct, ForceMode.VelocityChange);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_enemy))
            {
                PoolSignals.Instance.onEnqueuePooledGameObject?.Invoke(gameObject, PoolType.Ammo);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_turretAttackRadius))
            {
                PoolSignals.Instance.onEnqueuePooledGameObject?.Invoke(gameObject, PoolType.Ammo);
            }
        }
    }
}