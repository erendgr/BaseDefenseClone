using System;
using Enums;
using Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace Signals
{
    public class IdleSignals : MonoSingleton<IdleSignals>
    {
        public Func<Transform> onGetWareHousePositon = delegate { return default; };

        public UnityAction<GameObject, EnemyType> onEnemyDead = delegate { };
        public Func<GameObject> onEnemyHasTarget = delegate { return default; };
        public UnityAction<GameObject> onHostageCollected = delegate { };

        public UnityAction onInteractPlayerWithTurret = delegate { };

        public Func<WeaponType> onSelectedWeapon = delegate { return WeaponType.Pistol; };
        public Func<PlayerAnimState> onGetSelectedWeaponAnimState = delegate { return 0; };
        public Func<PlayerAnimState> onGetSelectedWeaponAttackAnimState = delegate { return 0; };

        public UnityAction onPlayerDied = delegate { };

        public Func<GameObject> onGetSoldierBarrack = delegate { return default; };
        public UnityAction onHostageEntryBarrack = delegate { };
        public UnityAction<GameObject> onPlayerEntrySoldierArea = delegate { };
        public Func<GameObject> onGetSoldierInitPosition = delegate { return default; };
        public Func<GameObject> onGetSoldierSearchInitPosition = delegate { return default; };

        public Func<GameObject> onGetMineGameObject = delegate { return default; };
        public Func<GameObject> onGetGemAreaHolder = delegate { return default; };
        public UnityAction<Transform> onAddGemToGemHolder = delegate { };
    }
}