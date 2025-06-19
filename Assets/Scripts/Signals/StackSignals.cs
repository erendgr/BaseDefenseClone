using System.Collections.Generic;
using Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace Signals
{
    public class StackSignals : MonoSingleton<StackSignals>
    {
        public UnityAction<GameObject, List<GameObject>> onInteractStackHolder = delegate { };
        public UnityAction<GameObject> onDecreseStackHolder = delegate { };
    }
}