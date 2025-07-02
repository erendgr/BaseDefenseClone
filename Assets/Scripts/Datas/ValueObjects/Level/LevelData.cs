using System;
using System.Collections.Generic;
using Enums;

namespace Datas.ValueObjects.Level
{
    [Serializable]
    public class LevelData
    {
        public BaseData BaseData;
        public FrontYardData FrontYardData;
        public WorkersData WorkersData;
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
    public class WorkersData
    {
        public AmmoWorkerBuyData AmmoWorkerBuyData;
        public MoneyWorkerBuyData MoneyWorkerBuyData;
    }
}