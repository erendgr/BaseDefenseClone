using Command.Player;
using Controllers.Player;
using Datas.UnityObjects;
using Datas.ValueObjects.Player;
using Enums;
using Keys;
using Signals;
using UnityEngine;

namespace Managers
{
    public class PlayerManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private PlayerMovementController movementController;
        [SerializeField] private PlayerAnimationController animationController;
        [SerializeField] private PlayerPhysicsController physicsController;
        [SerializeField] private PlayerHealthController healthController;

        #endregion

        #region Private Variables

        private PlayerData _data;
        private Transform _currentParent;
        private const string PlayerDataPath = "Data/CD_Player";
        private PlayerStates _playerState;
        private PlayerAnimState _weaponAnimState;
        private PlayerAnimState _weaponAttackAnimState;
        private SetIdleInputValuesCommand _setIdleInputValuesCommand;
        private SetPlayerStateCommand _setPlayerStateCommand;

        #endregion

        #endregion

        private void Awake()
        {
            var manager = this;
            _currentParent = transform.parent;
            _playerState = PlayerStates.Inside;
            _data = GetPlayerData();

            SendPlayerDataToControllers();

            _setIdleInputValuesCommand = new SetIdleInputValuesCommand(ref movementController, ref animationController);
            _setPlayerStateCommand = new SetPlayerStateCommand(ref manager, ref movementController,
                ref animationController , ref healthController);
        }

        private PlayerData GetPlayerData() => Resources.Load<CD_Player>(PlayerDataPath).Data;

        private void SendPlayerDataToControllers() => movementController.SetMovementData(_data.MovementData);

        #region Event Subscriptions

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onPlay += OnPlay;
            CoreGameSignals.Instance.onReset += OnReset;
            InputSignals.Instance.onJoystickDragged += OnSetIdleInputValues;
            AttackSignals.Instance.onDamageToPlayer += OnTakeDamage;
            AttackSignals.Instance.onPlayerHasTarget += OnPlayerHasTarget;
            IdleSignals.Instance.onInteractPlayerWithTurret += OnPlayerInTurret;
        }

        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onReset -= OnReset;
            InputSignals.Instance.onJoystickDragged -= OnSetIdleInputValues;
            AttackSignals.Instance.onDamageToPlayer -= OnTakeDamage;
            AttackSignals.Instance.onPlayerHasTarget -= OnPlayerHasTarget;
            IdleSignals.Instance.onInteractPlayerWithTurret -= OnPlayerInTurret;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        #region Event Methods

        private void Start()
        {
            CoreGameSignals.Instance.onSetPlayerPosition?.Invoke(transform);
            healthController.GetHealth(_data.PlayerHealth);
        }

        private void OnPlay()
        {
            movementController.IsReadyToPlay(true);
            animationController.SetBoolAnimState(PlayerAnimState.BaseState,true);
        }

        private void OnReset()
        {
            gameObject.SetActive(true);
            movementController.OnReset();
        }

        private void OnSetIdleInputValues(IdleInputParams inputParams)
        {
            _setIdleInputValuesCommand.Execute(inputParams, _playerState);
        }

        private void OnTakeDamage(int value)
        {
            healthController.TakeDamage(value);
        }

        private void OnPlayerHasTarget(bool hasTarget)
        {
            if (hasTarget)
            {
                movementController.IsLockTarget(true);
                _weaponAttackAnimState = IdleSignals.Instance.onGetSelectedWeaponAttackAnimState();
                animationController.SetAnimState(_weaponAttackAnimState);
                _playerState = PlayerStates.Attack;
            }
            else
            {
                animationController.SetAnimState(PlayerAnimState.AttackEnd);
                movementController.IsLockTarget(false);
                _playerState = PlayerStates.Outside;
            }
        }

        private void OnPlayerInTurret()
        {
            _playerState = PlayerStates.Turret;
            animationController.SetAnimState(PlayerAnimState.PlayerInTurret);
            movementController.IsReadyToPlay(false);
        }

        #endregion

        public void PlayerOutTurret()
        {
            transform.rotation = Quaternion.identity;
            _playerState = PlayerStates.Inside;
            animationController.SetAnimState(PlayerAnimState.PlayerOutTurret);
            movementController.IsReadyToPlay(true);
            transform.SetParent(_currentParent);
        }
        
        public void SetPlayerState(PlayerStates state)
        {
            _playerState = state;
            if (state == PlayerStates.Inside || state == PlayerStates.Death)
            {
                _weaponAnimState = IdleSignals.Instance.onGetSelectedWeaponAnimState();
            }
            _setPlayerStateCommand.Execute(state, _weaponAnimState);
        }
    }
}