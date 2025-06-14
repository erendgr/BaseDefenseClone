using Enums;
using Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace Signals
{
    public class PlayerSignals : MonoSingleton<PlayerSignals>
    {
        public UnityAction<bool> onPlayerDeath = delegate { };
        public UnityAction<PlayerStates> onPlayerStateChanged = delegate { };
        public UnityAction<PlayerAnimState> onPlayerAnimStateChanged = delegate { };
        public UnityAction onPlayerOutTurret = delegate { };
    }
}