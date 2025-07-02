using System;
using _ObjectPooling.Scripts.Enums;
using Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace _ObjectPooling.Scripts.Signals
{
    public class PoolSignals : MonoSingleton<PoolSignals>
    {
        public Func<PoolType, GameObject> onDequeuePoolableGameObject = delegate { return null; };
        public Func<PoolType, Transform, GameObject> onDequeuePoolableGOWithTransform = delegate { return null; };
        public UnityAction<GameObject, PoolType> onEnqueuePooledGameObject = delegate { };
    }
}