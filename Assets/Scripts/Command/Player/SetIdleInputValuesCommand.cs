using System;
using Controllers.Player;
using Enums;
using Keys;
using NUnit.Framework.Constraints;
using Signals;

namespace Command.Player
{
    public class SetIdleInputValuesCommand
    {
        #region Self Variables

        #region Private Variables

        private PlayerMovementController _movementController;
        private PlayerAnimationController _animationController;

        #endregion

        #endregion

        public SetIdleInputValuesCommand(ref PlayerMovementController movementController,
            ref PlayerAnimationController animationController)
        {
            _movementController = movementController;
            _animationController = animationController;
        }

        public void Execute(IdleInputParams inputParams, PlayerStates playerState)
        {
            switch (playerState)
            {
                case PlayerStates.Inside:
                    _movementController.UpdateInputValue(inputParams);
                    _animationController.SetSpeedVariable(inputParams);
                    break;
                case PlayerStates.Outside:
                    _movementController.UpdateInputValue(inputParams);
                    _animationController.SetSpeedVariable(inputParams);
                    _animationController.SetOutSideAnimState(inputParams, default, false);
                    break;
                case PlayerStates.Turret:
                    _movementController.UpdateTurretInputValue(inputParams);
                    break;
                case PlayerStates.Attack:
                    var target = AttackSignals.Instance.onGetPlayerTarget();
                    _movementController.UpdateInputValue(inputParams);
                    _animationController.SetSpeedVariable(inputParams);
                    _animationController.SetOutSideAnimState(inputParams, target.transform, true);
                    break;
                case PlayerStates.Death:
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}