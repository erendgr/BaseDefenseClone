using System.Collections.Generic;
using _ObjectPooling.Scripts.Data.ValueObjects;
using _ObjectPooling.Scripts.Enums;
using UnityEngine;

namespace _ObjectPooling.Scripts.Data.UnityObjects
{
    [CreateAssetMenu(fileName = "CD_Pool", menuName = "BaseDef/CD_Pool", order = 0)]
    public class CD_Pool : ScriptableObject
    {
        public List<PoolEntry> PoolList = new();

        public Dictionary<PoolType, PoolData> ToDictionary()
        {
            var dict = new Dictionary<PoolType, PoolData>();
            foreach (var entry in PoolList)
            {
                dict[entry.Key] = entry.Value;
            }
            return dict;
        }
    }
}