using System;
using AYellowpaper.SerializedCollections;
using Enums;

namespace Datas.ValueObjects.AI
{
    [Serializable]
    public class EnemyAIData
    {
        public SerializedDictionary<EnemyType, EnemyTypeData> EnemyTypeDatas;
    }
}