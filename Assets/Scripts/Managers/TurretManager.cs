using System;
using System.Collections.Generic;
using _ObjectPooling.Scripts.Enums;
using _ObjectPooling.Scripts.Signals;
using Command.Stack;
using Controllers;
using Controllers.Turret;
using Datas.UnityObjects;
using Datas.ValueObjects;
using DG.Tweening;
using Enums;
using Keys;
using Signals;
using UnityEngine;

namespace Managers
{
    public class TurretManager : AttackControllerBase
    {
        #region Self Variables
        
        #region Serialized Variables

        [SerializeField] private GameObject ammoHolder;
        [SerializeField] private GameObject barrelPoint;
        [SerializeField] private TurretAmmoAreaController turretAmmoAreaController;
        [SerializeField] private GameObject turretBot;
        [SerializeField] private TurretMovementController movementController;

        #endregion

        #region Private Variables

        private int _ammoAmount;
        private TurretData _data;
        private readonly int _ammoBoxLimit = 4;
        private GameObject _ammoPrefab;
        private GameObject _targetEnemy;
        private TurretStateEnum _turretState;
        private List<GameObject> _ammoStack = new();

        #endregion
        
        #region Public Variables

        public List<GameObject> EnemyList;
        public GameObject Target;

        public GetStaticStackItemPosCommand GetItemPosCommand;
        public AddItemToStaticStackCommand AddToStackCommand;

        #endregion

        #endregion

        private void Awake()
        {
            _ammoAmount = 0;
            EnemyList = Enemies;
            _data = GetTurretData();
            SendDatasToControllers();
            AttackDelay = _data.AttackDelay;
            GetItemPosCommand =
                new GetStaticStackItemPosCommand(ref _ammoStack, ref _data.TurretStackData, ref ammoHolder);
            AddToStackCommand =
                new AddItemToStaticStackCommand(ref _ammoStack, ref _data.TurretStackData, ref ammoHolder);
        }

        private TurretData GetTurretData() => Resources.Load<CD_Turret>("Data/CD_Turret").TurretData;

        private void SendDatasToControllers()
        {
            turretAmmoAreaController.SetData(_data.TurretStackData, _ammoStack);
            movementController.SetRotateDelay(_data.RotateDelay);
        }

        #region Event Subscription

        protected override void OnEnable()
        {
            SubscribeEvents();
            base.OnEnable();
        }

        private void SubscribeEvents()
        {
            AttackSignals.Instance.onGetAmmoDamage += OnGetAmmoDamage;
            StackSignals.Instance.onInteractStackHolder += OnPlayerInteractWithAmmoArea;
            StackSignals.Instance.onDecreaseStackHolder += OnNotInteractPlayerWithAmmoArea;
            InputSignals.Instance.onJoystickDragged += OnSetInputValue;
        }

        private void UnsubscribeEvents()
        {
            AttackSignals.Instance.onGetAmmoDamage -= OnGetAmmoDamage;
            StackSignals.Instance.onInteractStackHolder -= OnPlayerInteractWithAmmoArea;
            StackSignals.Instance.onDecreaseStackHolder -= OnNotInteractPlayerWithAmmoArea;
            InputSignals.Instance.onJoystickDragged -= OnSetInputValue;
        }

        protected override void OnDisable()
        {
            UnsubscribeEvents();
            base.OnDisable();
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

        private void OnSetInputValue(IdleInputParams input)
        {
            if (_turretState != TurretStateEnum.WithPlayer) return;
            movementController.SetInputValue(input);
        }

        #endregion

        protected override void ExecuteAttack()
        {
            switch (_turretState)
            {
                case TurretStateEnum.None:
                    return;
                case TurretStateEnum.WithBot:
                    movementController.LockTarget(_ammoStack.Count != 0);
                    break;
                case TurretStateEnum.WithPlayer:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (_ammoStack.Count == 0) return;
            Fire();
        }

        protected override void HandleNewTarget()
        {
            Target = TargetEnemy;
        }

        protected override void StopAttack()
        {
            movementController.LockTarget(false);
        }

        protected override bool TriggerEnter(Collider other)
        {
            if (_turretState != TurretStateEnum.None) return false;
            if (other.CompareTag("Enemy"))
            {
                Enemies.Add(other.gameObject);
            }

            return true;
        }

        protected override bool TriggerExit(Collider other)
        {
            if (_turretState != TurretStateEnum.None) return false;
            if (other.CompareTag("Enemy"))
            {
                Enemies.Remove(other.gameObject);
            }

            return true;
        }

        private void Fire()
        {
            _ammoPrefab = PoolSignals.Instance.onDequeuePoolableGameObject(PoolType.Ammo);
            _ammoPrefab.GetComponent<AmmoPhysicsController>().SetAddForce(transform.forward * 20);
            _ammoPrefab.transform.position = barrelPoint.transform.position;
            _ammoPrefab.transform.rotation = transform.rotation;
            _ammoAmount++;
            if (_ammoAmount != _ammoBoxLimit) return;
            turretAmmoAreaController.RemoveAmmoFromStack();
            _ammoAmount = 0;
        }

        public void ActivateSoldier()
        {
            turretBot.SetActive(true);
            _turretState = TurretStateEnum.WithBot;
            if (EnemyList.Count <= 0) return;
            StartCoroutine(Attack());
        }

        public void InteractPlayerWithTurret(GameObject player)
        {
            if (_turretState == TurretStateEnum.WithBot) return;
            _turretState = TurretStateEnum.WithPlayer;
            player.transform.SetParent(transform);
            player.transform.DOLocalMove(turretBot.transform.localPosition, 0.1f).OnComplete(() =>
            {
                player.transform.GetChild(0).rotation = transform.rotation;
                IdleSignals.Instance.onInteractPlayerWithTurret?.Invoke();
            });
            AttackCoroutine ??= StartCoroutine(Attack());
        }

        public void NotInteractPlayerWithTurret()
        {
            if (_turretState != TurretStateEnum.WithPlayer) return;
            _turretState = TurretStateEnum.None;
        }
    }
}