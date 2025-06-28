using Enums;
using Managers;
using UnityEngine;

namespace Controllers.Player
{
    public class PlayerPhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region SerializeField Variables

        [SerializeField] private PlayerManager manager;
        [SerializeField] private PlayerAttackManager playerAttackManager;

        #endregion

        #region Private Variables

        private readonly string _barrierInSide = "BarrierInSide";
        private readonly string _barrierOutSide = "BarrierOutSide";
        private readonly string _playerOutSideLayer = "PlayerOutSideLayer";
        private readonly string _default = "Default";

        #endregion

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_barrierInSide))
            {
                BarrierInSide();
                return;
            }

            if (other.CompareTag(_barrierOutSide))
            {
                BarrierOutSide();
            }
        }

        private void BarrierOutSide()
        {
            gameObject.layer = LayerMask.NameToLayer(_playerOutSideLayer);
            manager.SetPlayerState(PlayerStates.Outside);
            playerAttackManager.PlayerExitBase();
        }

        private void BarrierInSide()
        {
            gameObject.layer = LayerMask.NameToLayer(_default);
            manager.SetPlayerState(PlayerStates.Inside);
            playerAttackManager.OnPlayerDied();
        }
    }
}