using System;
using UnityEngine;

namespace Datas.ValueObjects.Level
{
    [Serializable]
    public class MineAreaData
    {
        public int MaxWorkerAmound;
        public int CurrentWorkerAmound;
        public int MaxWorkerFromMine;
        public StaticStackData StaticStackData;
    }
    
    [Serializable]
    public class StaticStackData
    {
        public int Capacity;
        public Vector3 InitPosition;
        public int StackCountX;
        public int StackCountZ;
        public Vector3 StackOffset;
        public float Delay;
    }
}