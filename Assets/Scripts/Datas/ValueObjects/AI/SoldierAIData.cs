using System;

namespace Datas.ValueObjects.AI
{
    [Serializable]
    public class SoldierAIData
    {
        public int Health;
        public int Damage;
        public float AttackDelay;
        public float AttackRange;
    }
}