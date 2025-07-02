using System;
using System.Collections.Generic;
using Datas.ValueObjects;
using Enums;
using Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace Signals
{
    public class WorkerSignals : MonoSingleton<WorkerSignals>
    {
        public UnityAction onLoadedWorkerData = delegate { };
        public Func<AmmoWorkerBuyData> onGetAmmoWorkerData = delegate { return default; };
        public Func<int> onGetPayedAmmoWorkerData = delegate { return default; };
        public UnityAction<int> onAmmoWorkerAreaBuyedItems = delegate { };

        public Func<MoneyWorkerBuyData> onGetMoneyWorkerData = delegate { return default; };
        public Func<int> onGetPayedMoneyWorkerData = delegate { return default; };
        public UnityAction<int> onMoneyWorkerAreaBuyedItems = delegate { };

        public Func<GameObject> onGetTurretArea = delegate { return default; };

        public UnityAction<GameObject> onAmmoAreaFull = delegate { };

        public UnityAction<GameObject, List<GameObject>> onTurretAmmoAreas = delegate { };

        public UnityAction<GameObject> onAddMoneyToList = delegate { };
        public UnityAction<GameObject> onRemoveMoneyFromList = delegate { };
        public Func<GameObject> onGetMoneyGameObject = delegate { return default; };
        public UnityAction onChangeDestination = delegate { };
        
        public Func<GameObject> onGetBaseCenter = delegate { return null;};

        public UnityAction onSoldierDeath = delegate {  };
        public UnityAction onSoldierAttack = delegate {  };

    }
}