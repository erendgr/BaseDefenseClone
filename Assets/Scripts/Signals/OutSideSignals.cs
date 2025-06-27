using System;
using Datas.ValueObjects;
using Enums;
using Extentions;
using UnityEngine.Events;

namespace Signals
{
    public class OutSideSignals : MonoSingleton<OutSideSignals>
    {
        public UnityAction onGettedOutSideData = delegate {  };
        public Func<OutSideStateLevels,OutsideData> onGetOutsideData = delegate { return default;};
        public Func<OutSideStateLevels,int> onGetPayedStageData = delegate { return default;};
        public UnityAction<OutSideStateLevels,int> onOutsideBuyedItems = delegate {  };
    }
}