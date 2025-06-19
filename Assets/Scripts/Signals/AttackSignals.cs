using System;
using Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace Signals
{
    public class AttackSignals : MonoSingleton<AttackSignals>
    {
        public UnityAction<int> onDamageToPlayer = delegate { };
        public UnityAction<bool> onPlayerHasTarget = delegate { };
        public Func<GameObject> onGetPlayerTarget = delegate { return default; };
    }
}