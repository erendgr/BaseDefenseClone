using System;
using Controllers.Player;
using Enums;
using Managers;

namespace Command.Player
{
    public class SetPlayerStateCommand
    {
        #region Self Variables

        #region Private Variables

        private PlayerMovementController _movementController;
        private PlayerAnimationController _animationController;
        private PlayerHealthController _healthController;
        private PlayerManager _manager;

        #endregion

        #endregion

        public SetPlayerStateCommand(ref PlayerManager manager, ref PlayerMovementController movementController,
            ref PlayerAnimationController animationController, ref PlayerHealthController healthController)
        {
            _manager = manager;
            _movementController = movementController;
            _animationController = animationController;
            _healthController = healthController;
        }

        public void Execute(PlayerStates playerState, PlayerAnimState weaponAnimState)
        {
            switch (playerState)
            {
                case PlayerStates.Inside:
                    _animationController.SetBoolAnimState(PlayerAnimState.BaseState, true);
                    _animationController.SetBoolAnimState(weaponAnimState, false);
                    _healthController.PlayerInBase(true);
                    break;
                case PlayerStates.Outside:
                    _animationController.SetBoolAnimState(PlayerAnimState.BaseState, false);
                    _animationController.SetBoolAnimState(weaponAnimState, true);
                    _healthController.PlayerInBase(false);
                    break;
                case PlayerStates.Attack:
                    break;
                case PlayerStates.Turret:
                    break;
                case PlayerStates.Death:
                    _movementController.IsReadyToPlay(false);
                    _animationController.SetBoolAnimState(PlayerAnimState.BaseState, true);
                    _animationController.SetBoolAnimState(weaponAnimState, false);
                    //_manager.PlayerDeath();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();

            }    
        }
    }
}