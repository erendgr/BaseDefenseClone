using System.Collections;
using DG.Tweening;
using Keys;
using Managers;
using UnityEngine;

namespace Controllers.Turret
{
    public class TurretMovementController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private TurretManager manager;

        #endregion

        #region Private Variables

        private Coroutine _rotate;
        private bool _playerUseIt;
        private float _turretRotation;
        private WaitForSeconds _rotateDelay;

        #endregion

        #endregion

        public void SetRotateDelay(float rotateDelay)
        {
            _rotateDelay = new WaitForSeconds(rotateDelay);
        }

        public void LockTarget(bool isAttack)
        {
            if (isAttack)
            {
                _rotate ??= StartCoroutine(Rotate());
            }
            else
            {
                if (_rotate != null)
                {
                    StopCoroutine(_rotate);
                    _rotate = null;
                    SetDefaultPosition();
                }
            }
        }

        private IEnumerator Rotate()
        {
            while (manager.EnemyList.Count > 0)
            {
                var direct = manager.Target.transform.position - transform.position;
                var lookRotation = Quaternion.LookRotation(direct, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    lookRotation, 0.2f);
                yield return _rotateDelay;
            }
        }

        public void SetInputValue(IdleInputParams data)
        {
            _turretRotation += data.ValueX;
            _turretRotation = Mathf.Clamp(_turretRotation, -35, 35);
            if (_turretRotation is > -35 and < 35)
            {
                transform.rotation = Quaternion.Slerp(Quaternion.Euler(0, transform.rotation.y, 0),
                    Quaternion.Euler(0, _turretRotation, 1f), 0.5f);
            }
        }

        private void SetDefaultPosition()
        {
            transform.DORotate(Vector3.zero, 0.5f);
        }
    }
}