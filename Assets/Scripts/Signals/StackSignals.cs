using System;
using System.Collections.Generic;
using Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace Signals
{
    public class StackSignals : MonoSingleton<StackSignals>
    {
        public UnityAction<GameObject, List<GameObject>> onInteractStackHolder = delegate { };
        public UnityAction<GameObject> onDecreaseStackHolder = delegate { };

        public Func<GameObject, GameObject> onGetHostageTarget = delegate { return default; };
        public Func<List<GameObject>> onGetHostageList = delegate { return default; };
        public UnityAction<bool> onLastGameObjectRemove = delegate {  };

    }
}