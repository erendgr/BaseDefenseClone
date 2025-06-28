using _ObjectPooling.Scripts.Enums;
using _ObjectPooling.Scripts.Signals;
using Signals;
using UnityEngine;

namespace Controllers
{
    public class BulletPhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private Rigidbody rb;

        #endregion

        #region Private Variables

        private Vector3 _direct;

        #endregion

        #endregion

        private void OnEnable()
        {
            _direct = AttackSignals.Instance.onGetBulletDirection();
            rb.AddForce(_direct, ForceMode.VelocityChange);
        }

        private void OnDisable()
        {
            rb.linearVelocity = Vector3.zero;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                PoolSignals.Instance.onEnqueuePooledGameObject?.Invoke(gameObject, PoolType.Bullet);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("WeaponAttackRadius"))
            {
                PoolSignals.Instance.onEnqueuePooledGameObject?.Invoke(gameObject, PoolType.Bullet);
            }
        }
    }
}