using _ObjectPooling.Scripts.Enums;
using _ObjectPooling.Scripts.Signals;
using Enums;
using Signals;
using UnityEngine;

namespace Controller
{
    public class SoldierBulletPhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private Rigidbody rb;

        #endregion

        #region Private Variables

        private readonly string _soldierAttackRadius = "SoldierAttackRadius";
        private readonly string _enemy = "Enemy";

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
                PoolSignals.Instance.onEnqueuePooledGameObject?.Invoke(gameObject, PoolType.SoldierBullet);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_soldierAttackRadius))
            {
                PoolSignals.Instance.onEnqueuePooledGameObject?.Invoke(gameObject, PoolType.SoldierBullet);
            }
        }
    }
}