using _ObjectPooling.Scripts.Enums;

namespace _ObjectPooling.Scripts.Data.ValueObjects
{
    [System.Serializable]
    public class PoolEntry
    {
        public PoolType Key;
        public PoolData Value;
    }
}