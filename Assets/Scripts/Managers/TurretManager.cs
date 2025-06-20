using System;
using System.Collections;
using System.Collections.Generic;
using _ObjectPooling.Scripts.Enums;
using _ObjectPooling.Scripts.Signals;
using Command.Stack;
using Controllers;
using Controllers.Turret;
using Datas.UnityObjects;
using Datas.ValueObjects;
using Enums;
using Signals;
using UnityEngine;

namespace Managers
{
    public class TurretManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public List<GameObject> EnemyList;
        public GameObject Target;

        public GetStaticStackItemPosCommand GetItemPosCommand;
        public AddItemToStaticStackCommand AddToStackCommand;

        #endregion

        #region Serialized Variables

        [SerializeField] private GameObject ammoHolder;
        [SerializeField] private GameObject barrelPoint;
        [SerializeField] private TurretAmmoAreaController turretAmmoAreaController;
        [SerializeField] private GameObject turretSoldier;
        [SerializeField] private TurretMovementController movementController;

        #endregion

        #region Private Variables

        private List<GameObject> _ammoStack = new();
        private TurretData _data;
        private TurretStateEnum _turretState;
        private float _attackDelay;
        private GameObject _ammoPrefab;
        private int _ammoAmount;
        private GameObject _targetEnemy;

        #endregion

        #endregion

        private void Awake()
        {
            _ammoAmount = 0;
            _data = GetTurretData();
            SendDatasToControllers();
            _attackDelay = _data.AttackDelay;
            GetItemPosCommand =
                new GetStaticStackItemPosCommand(ref _ammoStack, ref _data.TurretStackData, ref ammoHolder);
            AddToStackCommand =
                new AddItemToStaticStackCommand(ref _ammoStack, ref _data.TurretStackData, ref ammoHolder);
            movementController.SetRotateDelay(_data.RotateDelay);
        }

        private TurretData GetTurretData() => Resources.Load<CD_Turret>("Data/CD_Turret").TurretData;

        private void SendDatasToControllers()
        {
            turretAmmoAreaController.SetData(_data.TurretStackData, _ammoStack);
        }

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            AttackSignals.Instance.onGetAmmoDamage += OnGetAmmoDamage;
            StackSignals.Instance.onInteractStackHolder += OnPlayerInteractWithAmmoArea;
            StackSignals.Instance.onDecreaseStackHolder += OnNotInteractPlayerWithAmmoArea;
            // InputSignals.Instance.onJoystickDragged += OnSetInputValue;
        }

        private void UnsubscribeEvents()
        {
            AttackSignals.Instance.onGetAmmoDamage -= OnGetAmmoDamage;
            StackSignals.Instance.onInteractStackHolder -= OnPlayerInteractWithAmmoArea;
            StackSignals.Instance.onDecreaseStackHolder -= OnNotInteractPlayerWithAmmoArea;
            // InputSignals.Instance.onJoystickDragged -= OnSetInputValue;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        #region Event Methods
        
        private int OnGetAmmoDamage() => _data.AmmoDamage;

        private void OnPlayerInteractWithAmmoArea(GameObject holder, List<GameObject> ammoStack)
        {
            if (holder != ammoHolder) return;
            turretAmmoAreaController.AddAmmoToStack(ammoStack);
        }

        private void OnNotInteractPlayerWithAmmoArea(GameObject holder)
        {
            if (holder != ammoHolder) return;
            turretAmmoAreaController.PlayerNotInteractAmmoArea();
        }

        #endregion

        public void StartAttack()
        {
            switch (_turretState)
            {
                case TurretStateEnum.WithBot:
                    movementController.LockTarget(_ammoStack.Count != 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            StartCoroutine(Attack());
        }

        private IEnumerator Attack()
        {
            WaitForSeconds wait = new WaitForSeconds(_attackDelay);
            yield return wait;

            while (EnemyList.Count > 0)
            {
                if (_ammoStack.Count == 0) yield break;
                var closestDistance = float.MaxValue;
                foreach (var enemy in EnemyList)
                {
                    var enemyTransform = enemy.transform;
                    var distance = Vector3.Distance(transform.position, enemyTransform.position);
                    if (!(distance < closestDistance)) continue;
                    closestDistance = distance;
                    _targetEnemy = enemy;
                }

                Target = _targetEnemy;
                Fire();

                yield return wait;
            }

            StopAttack();
        }

        private void Fire()
        {
            _ammoPrefab = PoolSignals.Instance.onDequeuePoolableGameObject(PoolType.Ammo);
            _ammoPrefab.GetComponent<AmmoPhysicsController>().SetAddForce(transform.forward * 20);
            _ammoPrefab.transform.position = barrelPoint.transform.position;
            _ammoPrefab.transform.rotation = transform.rotation;
            _ammoAmount++;
            if (_ammoAmount != 4) return;
            turretAmmoAreaController.RemoveAmmoFromStack();
            _ammoAmount = 0;
        }

        private void StopAttack()
        {
            movementController.LockTarget(false);
            StopAllCoroutines();
        }

        public void ActivateSoldier()
        {
            turretSoldier.SetActive(true);
            _turretState = TurretStateEnum.WithBot;
            Debug.Log(_turretState);
            if (EnemyList.Count <= 0) return;
            StartCoroutine(Attack());
        }
    }
}