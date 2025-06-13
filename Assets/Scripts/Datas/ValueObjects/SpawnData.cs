using System;
using Enums;

namespace Datas.ValueObjects
{
    [Serializable]
    public class SpawnData
    {
        public EnemyType EnemyType;
        public int EnemyCount;
        public int CurrentCount = 0;
    }
}