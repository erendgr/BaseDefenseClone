using System;
using System.Collections;
using System.Collections.Generic;
using _ObjectPooling.Scripts.Enums;
using _ObjectPooling.Scripts.Signals;
using _StateMachine;
using Abstract;
using Datas.UnityObjects;
using Datas.ValueObjects;
using DG.Tweening;
using Enums;
using Signals;
using States.Enemy;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace AIBrain
{
    public class EnemyAIBrain : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public GameObject TurretTarget;
        public GameObject Target;

        #endregion

        #region SerializField Variables

        [SerializeField] private EnemyType enemyType;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject enemyBody;
        [SerializeField] private float checkTimer;

        #endregion

        #region Private Variables

        private EnemyTypeData _data;
        private GameObject _money;
        private Coroutine _attack;
        private int _health;
        private float _timer;
        private StateMachine<EnemyStates, EnemyBaseState> _stateMachine;

        #region Status

        private EnemyBaseState _currentState;
        private MoveToTurret _moveToTurret;
        private ChaseToPlayer _chaseToPlayer;
        private ChaseToSoldier _chaseToSoldier;
        private AttackToPlayer _attackToPlayer;
        private AttackToSoldier _attackToSoldier;
        private EnemyDeath _enemyDeath;

        #endregion

        #endregion

        #endregion

        private void Awake()
        {
            var brain = this;
            _data = Resources.Load<CD_AI>("Data/Cd_AI").EnemyAIData.EnemyTypeDatas[enemyType];
            
            var stateMap = new Dictionary<EnemyStates, EnemyBaseState>
            {
                [EnemyStates.MoveToTurret] = new MoveToTurret(ref brain, ref agent,ref _data),
                [EnemyStates.ChaseToPlayer] = new ChaseToPlayer(ref brain, ref agent, ref _data),
                [EnemyStates.ChaseToSoldier] = new ChaseToSoldier(ref brain, ref agent, ref _data),
                [EnemyStates.AttackToPlayer] = new AttackToPlayer(ref brain, ref agent, ref _data),
                [EnemyStates.AttackToSoldier] = new AttackToSoldier(ref brain, ref agent, ref _data),
                [EnemyStates.EnemyDeath] = new EnemyDeath(ref brain, ref agent, ref _data),
            };
            _stateMachine = new StateMachine<EnemyStates, EnemyBaseState>(stateMap);
        }

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
            _health = _data.Health;
            TurretTarget = IdleSignals.Instance.onEnemyHasTarget();
            _currentState = _stateMachine.Switch(EnemyStates.MoveToTurret);
            _currentState.EnterState();
        }

        private void SubscribeEvents()
        {
            AttackSignals.Instance.onSoldierDead += OnSoldierDeath;
        }

        private void UnsubscribeEvents()
        {
            AttackSignals.Instance.onSoldierDead -= OnSoldierDeath;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            UnsubscribeEvents();
            AttackSignals.Instance.onEnemyDead?.Invoke(enemyBody);
        }

        #endregion
        
        #region Event Methods

        private void OnSoldierDeath(GameObject soldier)
        {
            if (soldier == Target)
            {
                AttackToSoldierStatus(false);
                SwitchState(EnemyStates.MoveToTurret);
            }
        }

        #endregion

        private void Update()
        {
            _timer += Time.deltaTime;
            if (!(_timer >= checkTimer)) return;
            _currentState.UpdateState();
            _timer = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            _currentState.OnTriggerEnterState(other);
        }

        private void OnTriggerExit(Collider other)
        {
            _currentState.OnTriggerExitState(other);
        }

        public void SwitchState(EnemyStates state)
        {
            _currentState = _stateMachine.Switch(state);
            _currentState.EnterState();
        }

        public void AttackToPlayerStatus(bool isAttack)
        {
            if (isAttack)
            {
                _attack = StartCoroutine(AttackToPlayer(true));
            }
            else
            {
                if (_attack == null) return;
                StopCoroutine(_attack);
                _attack = null;
            }
        }

        public void AttackToSoldierStatus(bool isAttack)
        {
            if (isAttack)
            {
                _attack = StartCoroutine(AttackToPlayer(false));
            }
            else
            {
                if (_attack == null) return;
                StopCoroutine(_attack);
                _attack = null;
            }
        }

        public void TakeBulletDamage()
        {
            _health -= AttackSignals.Instance.onGetWeaponDamage();
        }

        public void TakeAmmoDamage()
        {
            _health -= AttackSignals.Instance.onGetAmmoDamage();
        }

        public void TakeSoldierDamage()
        {
            _health -= AttackSignals.Instance.onGetSoldierDamage();
        }

        public bool HealthCheck()
        {
            return _health <= 0;
        }

        private IEnumerator AttackToPlayer(bool isPlayer)
        {
            WaitForSeconds wait = new WaitForSeconds(1.1f);
            while (true)
            {
                AnimTriggerState(EnemyAnimState.Attack);
                yield return wait;
                if (isPlayer)
                {
                    AttackSignals.Instance.onDamageToPlayer?.Invoke(_data.Damage);
                }
                else
                {
                    AttackSignals.Instance.onDamegeToSoldier?.Invoke(Target, _data.Damage);
                }
            }
        }

        private IEnumerator Death()
        {
            WaitForSeconds wait = new WaitForSeconds(1f);
            
            AnimBoolState(EnemyAnimState.Death, true);
            IdleSignals.Instance.onEnemyDead?.Invoke(TurretTarget, enemyType);
            
            yield return wait;
            
            PrizeMoney();
            AttackSignals.Instance.onEnemyDead?.Invoke(enemyBody);
            
            yield return new WaitForSeconds(0.1f);
            
            PoolType poolType = (PoolType) Enum.Parse(typeof(PoolType), enemyType.ToString());
            PoolSignals.Instance.onEnqueuePooledGameObject?.Invoke(gameObject, poolType);
        }

        private void PrizeMoney()
        {
            Vector3 position = transform.position;
            for (int i = 0; i < _data.PrizeMoney; i++)
            {
                _money = PoolSignals.Instance.onDequeuePoolableGameObject(PoolType.Money);
                _money.transform.position = transform.position;
                _money.transform.DOLocalJump(
                    new Vector3(position.x + Random.Range(-1f, 1f), 0.5f, position.z + Random.Range(0f, 1f)),
                    1f, 3, 0.5f);
            }
        }

        public void IsDeath()
        {
            StartCoroutine(Death());
        }

        public void AnimTriggerState(EnemyAnimState states)
        {
            animator.SetTrigger(states.ToString());
        }

        public void AnimBoolState(EnemyAnimState animState, bool isAttack)
        {
            animator.SetBool(animState.ToString(), isAttack);
        }
    }
}