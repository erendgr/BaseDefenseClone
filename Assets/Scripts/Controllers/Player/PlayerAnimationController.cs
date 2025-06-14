using Enums;
using Keys;
using Managers;
using UnityEngine;

namespace Controllers.Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private PlayerManager manager;
        [SerializeField] private Animator animator;

        #endregion

        #region Private Variables

        private readonly string _speed = "Speed";
        private readonly string _speedX = "SpeedX";
        private readonly string _speedZ = "SpeedZ";

        #endregion

        #endregion

        public void SetAnimState(PlayerAnimState animState)
        {
            animator.SetTrigger(animState.ToString());
        }

        public void SetBoolAnimState(PlayerAnimState animState, bool animStateBool)
        {
            animator.SetBool(animState.ToString(), animStateBool);
        }

        public void SetSpeedVariable(IdleInputParams inputParams)
        {
            var speedX = Mathf.Abs(inputParams.ValueX);
            var speedZ = Mathf.Abs(inputParams.ValueZ);
            animator.SetFloat(_speed, Mathf.Clamp(speedX + speedZ, 0, 1));
        }

        public void SetOutSideAnimState(IdleInputParams inputParams, Transform target, bool hasTarget)
        {
            if (hasTarget)
            {
                PlayerHasTarget(inputParams, target);
            }
            else
            {
                var speedZ = Mathf.Abs(inputParams.ValueZ);
                var speedX = Mathf.Abs(inputParams.ValueX);
                animator.SetFloat(_speedZ, Mathf.Clamp(speedX + speedZ, 0, 1));
                animator.SetFloat(_speedX, 0);
            }
        }

        private void PlayerHasTarget(IdleInputParams inputParams, Transform target)
        {
            var playerPosition = transform.parent.position;
            var targetPosition = target.position;
            var distance = targetPosition - playerPosition;
            var speedX = Mathf.Clamp((distance.z + distance.x) * inputParams.ValueX, -1f, 1f);
            var speedZ = Mathf.Clamp((distance.z + distance.x) * inputParams.ValueZ, -1f, 1f);
            animator.SetFloat(_speedX, speedX);
            animator.SetFloat(_speedZ, speedZ);
        }
    }
}