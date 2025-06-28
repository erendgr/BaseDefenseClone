using System;
using _ObjectPooling.Scripts.Enums;
using _ObjectPooling.Scripts.Signals;
using Controllers;
using Datas.UnityObjects;
using Enums;
using Signals;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    public class PlayerAttackManager : AttackControllerBase
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private GameObject weaponHolder;
        [SerializeField] private GameObject bodyDirection;

        #endregion

        #region Private Variables

        private GameObject _weapon;
        private Transform _firePoint;
        private bool _isPlayerOnBase;
        private CD_Weapon _data;
        private WeaponType _selectedWeaponType;
        private WeaponData _selectedWeaponData = new();
        private readonly string _enemy = "Enemy";
        private readonly Quaternion _weaponRotation = Quaternion.Euler(270, 94.6f, 183.9f);

        #endregion

        #endregion

        private void Awake()
        {
            _data = GetData();
            _isPlayerOnBase = true;
        }

        private CD_Weapon GetData() => Resources.Load<CD_Weapon>("Data/CD_Weapon");

        #region Event Subscription

        protected override void OnEnable()
        {
            base.OnEnable();
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            AttackSignals.Instance.onGetWeaponDamage += OnGetWeaponDamage;
            IdleSignals.Instance.onGetSelectedWeaponAnimState += OnGetWeaponAnimState;
            IdleSignals.Instance.onGetSelectedWeaponAttackAnimState += OnGetWeaponAttackAnimState;
            AttackSignals.Instance.onGetPlayerTarget += OnGetTarget;
            AttackSignals.Instance.onGetBulletDirection += OnGetBulletDirection;
            IdleSignals.Instance.onPlayerDead += OnPlayerDied;
        }

        private void UnsubscribeEvents()
        {
            AttackSignals.Instance.onGetWeaponDamage -= OnGetWeaponDamage;
            IdleSignals.Instance.onGetSelectedWeaponAnimState -= OnGetWeaponAnimState;
            IdleSignals.Instance.onGetSelectedWeaponAttackAnimState -= OnGetWeaponAttackAnimState;
            AttackSignals.Instance.onGetPlayerTarget -= OnGetTarget;
            AttackSignals.Instance.onGetBulletDirection -= OnGetBulletDirection;
            IdleSignals.Instance.onPlayerDead -= OnPlayerDied;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            UnsubscribeEvents();
        }

        #endregion

        #region Event Methods

        private Vector3 OnGetBulletDirection() => bodyDirection.transform.forward * _selectedWeaponData.BulletSpeed;

        private GameObject OnGetTarget() => TargetEnemy;

        private int OnGetWeaponDamage() => _selectedWeaponData.Damage;

        private PlayerAnimState OnGetWeaponAnimState() => _selectedWeaponData.WeaponAnimState;

        private PlayerAnimState OnGetWeaponAttackAnimState() => _selectedWeaponData.WeaponAttackAnimState;

        public void OnPlayerDied()
        {
            if (_isPlayerOnBase) return;
            
            PoolType poolType = (PoolType) Enum.Parse(typeof(PoolType), _selectedWeaponType.ToString());
            PoolSignals.Instance.onEnqueuePooledGameObject?.Invoke(_weapon, poolType);
            RadiusCollider.enabled = false;
            
            if (AttackCoroutine != null)
            {
                StopCoroutine(AttackCoroutine);
                AttackCoroutine = null;
                Enemies.Clear();
                ShouldSelectNewTarget = true;
                AttackSignals.Instance.onPlayerHasTarget?.Invoke(false);
            }

            _weapon = null;
            _isPlayerOnBase = true;
        }

        #endregion

        private void Start()
        {
            SetWeaponData();
        }
        
        private void SetWeaponData()
        {
            _selectedWeaponType = IdleSignals.Instance.onSelectedWeapon();
            _selectedWeaponData = _data.Weapons[_selectedWeaponType];
            AttackDelay = _selectedWeaponData.AttackDelay;
        }

        public void PlayerExitBase()
        {
            if (!_isPlayerOnBase) return;
            RadiusCollider.enabled = true;
            
            PoolType poolType = (PoolType) Enum.Parse(typeof(PoolType), _selectedWeaponType.ToString());
            _weapon = PoolSignals.Instance.onDequeuePoolableGameObjectWithTransform(poolType, weaponHolder.transform);
            _weapon.transform.SetParent(weaponHolder.transform);
            _weapon.transform.localRotation = _weaponRotation;
            _firePoint = _weapon.transform.Find("FirePoint");
            _isPlayerOnBase = false;
        }
        
        protected override void HandleNewTarget()
        {
            AttackSignals.Instance.onPlayerHasTarget?.Invoke(true);
        }

        protected override void ExecuteAttack()
        {
            PoolSignals.Instance.onDequeuePoolableGameObjectWithTransform(PoolType.Bullet, _firePoint);
        }

        protected override void StopAttack()
        {
            AttackSignals.Instance.onPlayerHasTarget?.Invoke(false);
        }

        protected override bool TriggerEnter(Collider other)
        {
            if (!_isPlayerOnBase) return false;
            
            if (other.CompareTag(_enemy))
            {
                Enemies.Add(other.gameObject);
            }

            return true;
        }

        protected override bool TriggerExit(Collider other)
        {
            if (!_isPlayerOnBase) return false;
            
            if (other.CompareTag(_enemy))
            {
                Enemies.Remove(other.gameObject);
            }

            return true;
        }
    }
}