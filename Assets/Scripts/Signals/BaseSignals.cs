using System;
using System.Collections.Generic;
using Datas.ValueObjects;
using Enums;
using Extentions;
using UnityEngine.Events;

namespace Signals
{
    public class BaseSignals : MonoSingleton<BaseSignals>
    {
        #region Base Area

        public UnityAction<RoomNameEnum,int> onBaseAreaBuyedItem = delegate {  };
        public UnityAction<TurretNameEnum,int> onTurretAreaBuyedItem = delegate {  };
        public UnityAction onGettedBaseData = delegate {  };
        public Func<RoomNameEnum,RoomData> onRoomData = delegate{return  default;};
        public Func<RoomNameEnum,int> onPayedRoomData = delegate{return  default;};
        public Func<TurretNameEnum,BuyableTurretData> onTurretData = delegate{return  default;};
        public Func<TurretNameEnum,int> onPayedTurretData = delegate{return  default;};

        #endregion
        
    }
}