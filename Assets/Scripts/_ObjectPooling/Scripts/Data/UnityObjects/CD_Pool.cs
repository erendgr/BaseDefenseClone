using _ObjectPooling.Scripts.Data.ValueObjects;
using _ObjectPooling.Scripts.Enums;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _ObjectPooling.Scripts.Data.UnityObjects
{
    [CreateAssetMenu(fileName = "CD_Pool", menuName = "BaseDef/CD_Pool", order = 0)]
    public class CD_Pool : ScriptableObject
    {
        public SerializedDictionary<PoolType, PoolData> PoolList = new();
    }
}