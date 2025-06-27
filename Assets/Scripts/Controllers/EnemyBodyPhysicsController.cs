using AIBrain;
using UnityEngine;

namespace Controllers
{
    public class EnemyBodyPhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private EnemyAIBrain enemyBrain;

        #endregion

        #region Private Variables

        private readonly string _bullet = "Bullet";
        private readonly string _turretAmmo = "TurretAmmo";
        private readonly string _soldierBullet = "SoldierBullet";

        #endregion

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_bullet))
            {
                enemyBrain.TakeBulletDamage();
                return;
            }

            if (other.CompareTag(_turretAmmo))
            {
                enemyBrain.TakeAmmoDamage();
            }

            if (other.CompareTag(_soldierBullet))
            {
                enemyBrain.TakeSoldierDamage();
            }
        }
    }
}