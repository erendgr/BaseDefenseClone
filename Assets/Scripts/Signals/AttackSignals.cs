using System;
using Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace Signals
{
    public class AttackSignals : MonoSingleton<AttackSignals>
    {
        public UnityAction<bool> onPlayerHasTarget = delegate { };
        public UnityAction<int> onDamageToPlayer = delegate { };
        public Func<GameObject> onGetPlayerTarget = delegate { return default; };
        public Func<Vector3> onGetBulletDirection = delegate { return default;};

        public UnityAction<GameObject,int> onDamegeToSoldier = delegate { };
        public UnityAction<GameObject> onSoldierDead = delegate {  };
        
        public UnityAction<GameObject> onEnemyDead = delegate { };
        
        public Func<int> onGetSoldierDamage = delegate { return 0; };
        public Func<int> onGetAmmoDamage = delegate { return 0; };
        public Func<int> onGetWeaponDamage = delegate { return 0;};
    }
}