using System;
using Enums;
using Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace Signals
{
    public class IdleSignals : MonoSingleton<IdleSignals>
    {
        public  Func<Transform> onGetWareHousePositon = delegate { return default;};

        public UnityAction<GameObject,EnemyType> onEnemyDead = delegate(GameObject arg0, EnemyType type) {  };
        public Func<GameObject> onEnemyHasTarget = delegate { return default;};
        public UnityAction<GameObject> onHostageCollected = delegate {  };

        public UnityAction onInteractPlayerWithTurret = delegate { };
        
        public Func<WeaponType> onSelectedWeapon = delegate { return WeaponType.Pistol;};
        public Func<PlayerAnimState> onGetSelectedWeaponAnimState = delegate { return 0;};
        public Func<PlayerAnimState> onGetSelectedWeaponAttackAnimState = delegate { return 0;};

        public UnityAction onPlayerDead = delegate {  };

    }
}