using System;
using _ObjectPooling.Scripts.Enums;
using _ObjectPooling.Scripts.Signals;
using Signals;
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
        private readonly string _player = "Player"; // for test

        #endregion

        #endregion

        private void Awake()
        {
            throw new NotImplementedException();
        }

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
            if (other.CompareTag(_player))
            {
                AttackSignals.Instance.onDamageToPlayer?.Invoke(10); // for test 10 damage
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