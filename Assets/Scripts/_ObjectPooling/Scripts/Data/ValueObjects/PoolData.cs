using System;
using _ObjectPooling.Scripts.Enums;
using UnityEngine;

namespace _ObjectPooling.Scripts.Data.ValueObjects
{
    [Serializable]
    public class PoolData
    {
        public int Amount;
        public GameObject Prefab;

        // public Attribute Data; 

        public PoolType Type;
    }
}