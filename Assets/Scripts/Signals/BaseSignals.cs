using System;
using Datas.ValueObjects;
using Datas.ValueObjects.Level;
using Enums;
using Extentions;
using UnityEngine.Events;

namespace Signals
{
    public class BaseSignals : MonoSingleton<BaseSignals>
    {
        public UnityAction<RoomNameEnum,int> onBaseAreaBuyedItem = delegate {  };
        public UnityAction<TurretNameEnum,int> onTurretAreaBuyedItem = delegate {  };
        public UnityAction onLoadedBaseData = delegate {  };
        public Func<RoomNameEnum,RoomData> onRoomData = delegate{return  default;};
        public Func<RoomNameEnum,int> onPayedRoomData = delegate{return  default;};
        public Func<TurretNameEnum,BuyableTurretData> onTurretData = delegate{return  default;};
        public Func<TurretNameEnum,int> onPayedTurretData = delegate{return  default;};
        public Func<SoldierAreaData> onGetSoldierAreaData = delegate { return default; };
        public Func<MineAreaData> onGetMineAreaData = delegate { return default;};

    }
}