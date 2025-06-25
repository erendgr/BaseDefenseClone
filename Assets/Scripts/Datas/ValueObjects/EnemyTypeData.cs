using System;

namespace Datas.ValueObjects
{
    [Serializable]
    public class EnemyTypeData
    {
        public int Health;
        public int Damage;
        public float AttackRange;
        public float MoveSpeed;
        public float ChaseSpeed;
        public int PrizeMoney;
    }
}