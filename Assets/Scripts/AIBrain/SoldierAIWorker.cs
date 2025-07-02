using System.Collections;
using System.Collections.Generic;
using _ObjectPooling.Scripts.Enums;
using _ObjectPooling.Scripts.Signals;
using _StateMachine;
using Abstract;
using Controller;
using Controllers;
using Datas.UnityObjects;
using Datas.ValueObjects.AI;
using DG.Tweening;
using Enums;
using Signals;
using States.Soldier;
using UnityEngine;
using UnityEngine.AI;

namespace AIBrain
{
    public class SoldierAIWorker : AttackControllerBase
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject weapon;
        [SerializeField] private Transform firePoint;

        #endregion

        #region Private Variables

        private int _health;
        private bool _isAttack;
        private bool _firstAttack;
        private SoldierAIData _data;

        private Death _death;
        private Attack _rangedAttack;
        private FindEnemy _searchEnemy;
        private MoveToEnemy _moveToEnemy;
        private SoldierBaseStates _currentState;
        private readonly string _speed = "Speed";
        private MoveToInitPosition _moveToInitPosition;
        private MoveToSearchInitPosition _moveToSearchInitPosition;
        private StateMachine<SoldierStates, SoldierBaseStates> _stateMachine;

        #endregion

        #region Public Variables

        public GameObject SearchInitPosition;
        public GameObject Target;

        #endregion

        #endregion

        private void Awake()
        {
            InitReferences();
        }

        private void InitReferences()
        {
            var brain = this;
            
            _data = Resources.Load<CD_AI>("Data/Cd_AI").SoldierAIData;
            AttackDelay = _data.AttackDelay;

            var stateMap = new Dictionary<SoldierStates, SoldierBaseStates>
            {
                [SoldierStates.MoveToInitPosition] = new MoveToInitPosition(ref brain, ref agent),
                [SoldierStates.MoveToSearchInitPosition] = new MoveToSearchInitPosition(ref brain, ref agent),
                [SoldierStates.FindEnemy] = new FindEnemy(ref brain, ref agent),
                [SoldierStates.MoveToEnemy] = new MoveToEnemy(ref brain, ref agent, ref _data),
                [SoldierStates.Attack] = new Attack(ref brain, ref agent, ref _data),
                [SoldierStates.Dead] = new Death(ref brain, ref agent),
            };
            _stateMachine = new StateMachine<SoldierStates, SoldierBaseStates>(stateMap);
        }

        #region Event Subscription

        protected override void OnEnable()
        {
            base.OnEnable();
            SubscribeEvents();
            IsAttack(true);
            _health = _data.Health;
            _firstAttack = true;
            _currentState = _stateMachine.Switch(SoldierStates.MoveToInitPosition);
            _currentState.EnterState();
        }

        private void SubscribeEvents()
        {
            WorkerSignals.Instance.onSoldierAttack += OnSoldierAttack;
            AttackSignals.Instance.onGetSoldierDamage += OnGetDamage;
            AttackSignals.Instance.onDamegeToSoldier += OnTakeDamage;
        }

        private void UnsubscribeEvents()
        {
            WorkerSignals.Instance.onSoldierAttack -= OnSoldierAttack;
            AttackSignals.Instance.onGetSoldierDamage -= OnGetDamage;
            AttackSignals.Instance.onDamegeToSoldier -= OnTakeDamage;
        }

        protected override void OnDisable()
        {
            AttackSignals.Instance.onSoldierDead?.Invoke(gameObject);
            StopAllCoroutines();
            base.OnDisable();
            UnsubscribeEvents();
        }

        #endregion

        #region Event Methods

        private void OnSoldierAttack()
        {
            if (_firstAttack)
            {
                _firstAttack = false;
                SwitchState(SoldierStates.MoveToSearchInitPosition);
            }
        }
        
        private int OnGetDamage() => _data.Damage;

        private void OnTakeDamage(GameObject target, int damage)
        {
            if (gameObject == target)
            {
                _health -= damage;
            }
        }

        #endregion
        
        private void Update()
        {
            _currentState.UpdateState();
        }
        
        public void SwitchState(SoldierStates state)
        {
            _currentState = _stateMachine.Switch(state);
            _currentState.EnterState();
        }
        
        public bool HealthCheck()
        {
            return _health <= 0;
        }

        public void IsAttack(bool isAttack)
        {
            _isAttack = isAttack;
        }

        public void IsDeath()
        {
            StartCoroutine(Death());
        }

        private IEnumerator Death()
        {
            WaitForSeconds wait = new WaitForSeconds(2f);
            AnimTriggerState(SoldierAnimState.Death);
            transform.DOLocalMoveY(0f, 0.5f);
            yield return wait;
            WorkerSignals.Instance.onSoldierDeath?.Invoke();
            AttackSignals.Instance.onSoldierDead?.Invoke(gameObject);
            PoolSignals.Instance.onEnqueuePooledGameObject(gameObject, PoolType.Soldier);
        }
        
        public void AnimSetFloat(float speed)
        {
            animator.SetFloat(_speed, speed);
        }

        protected override void HandleNewTarget()
        {
            Target = TargetEnemy;
            SwitchState(SoldierStates.MoveToEnemy);
        }

        protected override void ExecuteAttack()
        {
            if (_isAttack)
            {
                var bullet = PoolSignals.Instance.onDequeuePoolableGOWithTransform(PoolType.SoldierBullet, firePoint);
                bullet.GetComponent<SoldierBulletPhysicsController>().SetAddForce(transform.forward * 20);
            }
        }

        protected override void StopAttack()
        {
            AnimTriggerState(SoldierAnimState.AttackEnd);
            SwitchState(SoldierStates.FindEnemy);
        }

        protected override bool TriggerEnter(Collider other)
        {
            return false;
        }

        protected override bool TriggerExit(Collider other)
        {
            return false;
        }

        public void AnimTriggerState(SoldierAnimState states)
        {
            animator.SetTrigger(states.ToString());
        }
    }
}