using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.Serialization;

namespace Datas.ValueObjects
{
    [Serializable]
    public class LevelData
    {
        public BaseData BaseData;
        public FrontYardData FrontYardData;
        public SupporterData SupporterData;
    }
    
    [Serializable]
    public class BaseData
    {
        public BaseRoomData BaseRoomData;
        public MineAreaData MineAreaData;
        public SoldierAreaData SoldierAreaData;
    }
    
    [Serializable]
    public class FrontYardData
    {
        public List<OutsideData> OutsideLevelData;
        public List<SpawnData> SpawnDatas;
    }
    
    [Serializable]
    public class SupporterData
    {
        public AmmoWorkerBuyData AmmoWorkerBuyData;
        public MoneyWorkerBuyData MoneyWorkerBuyData;
    }
}