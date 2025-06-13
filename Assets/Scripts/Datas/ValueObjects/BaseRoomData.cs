using System;
using System.Collections.Generic;
using Enums;

namespace Datas.ValueObjects
{
    [Serializable]
    public class BaseRoomData
    {
        public List<RoomData> Rooms = new List<RoomData>();
    }
    
    [Serializable]
    public class RoomData
    {
        public PayTypeEnum PayType;
        public int Cost;
        public RoomNameEnum RoomName;
        public bool Isbase;
        public BuyableTurretData buyableTurretData;
    }
    
    [Serializable]
    public class BuyableTurretData
    {
        public PayTypeEnum PayType;
        public int Cost;
    }
}