using System;

namespace Enums
{
    [Serializable]
    public class WeaponData
    {
        public int Damage;
        public float AttackRange;
        public float AttackDelay;
        public float BulletSpeed;
        public PlayerAnimState WeaponAnimState;
        public PlayerAnimState WeaponAttackAnimState;
    }
}