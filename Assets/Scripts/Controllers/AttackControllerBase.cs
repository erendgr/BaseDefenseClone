using System.Collections;
using System.Collections.Generic;
using Signals;
using UnityEngine;

namespace Controllers
{
    public class AttackControllerBase : MonoBehaviour
    {
        #region Self Variables

        #region Protected Variables

        protected List<GameObject> Enemies = new();
        protected float AttackDelay;
        protected Coroutine AttackCoroutine;
        protected bool ShouldSelectNewTarget;
        protected GameObject TargetEnemy;

        #endregion

        #region Private Variables

        private readonly string _enemy = "Enemy";

        #endregion
        
        #region Public Variables

        public Collider RadiusCollider;

        #endregion
        
        #endregion

        #region Event Subscription

        protected virtual void OnEnable()
        {
            ShouldSelectNewTarget = true;
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            AttackSignals.Instance.onEnemyDead += OnEnemyDead;
        }

        private void UnsubscribeEvents()
        {
            AttackSignals.Instance.onEnemyDead -= OnEnemyDead;
        }

        protected virtual void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (TriggerEnter(other))
            {
                return;
            }

            if (other.CompareTag(_enemy))
            {
                Enemies.Add(other.gameObject);
                AttackCoroutine ??= StartCoroutine(Attack());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (TriggerExit(other))
            {
                return;
            }

            if (other.CompareTag(_enemy))
            {
                Enemies.Remove(other.gameObject);
                CheckTargetRemoval(other.gameObject);

                if (Enemies.Count != 0) return;
                if (AttackCoroutine == null) return;
                
                StopAttack();
                StopCoroutine(AttackCoroutine);
                
                AttackCoroutine = null;
                ShouldSelectNewTarget = true;
            }
        }

        private void OnEnemyDead(GameObject enemy)
        {
            if (!Enemies.Contains(enemy)) return;
            
            Enemies.Remove(enemy);
            CheckTargetRemoval(enemy);
        }

        protected IEnumerator Attack()
        {
            WaitForSeconds wait = new WaitForSeconds(AttackDelay);
            yield return wait;

            while (Enemies.Count > 0)
            {
                if (ShouldSelectNewTarget)
                {
                    var closestDistance = float.MaxValue;
                    foreach (var enemy in Enemies)
                    {
                        var enemyTransform = enemy.transform;
                        var distance = Vector3.Distance(transform.position, enemyTransform.position);
                        if (!(distance < closestDistance)) continue;
                        closestDistance = distance;
                        TargetEnemy = enemy;
                    }

                    HandleNewTarget();
                    ShouldSelectNewTarget = false;
                }

                ExecuteAttack();
                yield return wait;
            }

            StopAttack();
            ShouldSelectNewTarget = true;
            AttackCoroutine = null;
        }

        private void CheckTargetRemoval(GameObject enemy)
        {
            if (enemy == TargetEnemy)
            {
                ShouldSelectNewTarget = true;
            }
        }

        protected virtual void ExecuteAttack()
        {
        }

        protected virtual void StopAttack()
        {
        }

        protected virtual void HandleNewTarget()
        {
        }

        protected virtual bool TriggerEnter(Collider other)
        {
            return false;
        }

        protected virtual bool TriggerExit(Collider other)
        {
            return false;
        }
    }
}