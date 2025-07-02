using Datas.UnityObjects;
using Datas.ValueObjects;
using Datas.ValueObjects.Level;
using Enums;
using Keys;
using Signals;
using UnityEngine;

namespace Managers
{
    public class MoneyAndAmmoWorkerBuyManager : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables

        private WorkersData _data;
        private SupporterBuyableDataParams _payedAmounts;
        private int _baseLevel;
        private int _ammoWorkerPayedAmount;
        private int _moneyWorkerPayedAmount;

        #endregion

        #endregion

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
            GetReferences();
        }

        private void SubscribeEvents()
        {
            WorkerSignals.Instance.onGetAmmoWorkerData += OnGetAmmoWorkerData;
            WorkerSignals.Instance.onGetMoneyWorkerData += OnGetMoneyWorkerData;
            WorkerSignals.Instance.onGetPayedAmmoWorkerData += OnGetAmmoWorkerAreaPayedAmount;
            WorkerSignals.Instance.onGetPayedMoneyWorkerData += OnGetMoneyWorkerAreaPayedAmount; 
            WorkerSignals.Instance.onAmmoWorkerAreaBuyedItems += OnSetPayedAmmoWorkerAreaData;
            WorkerSignals.Instance.onMoneyWorkerAreaBuyedItems += OnSetPayedMoneyWorkerAreaData;
            SaveSignals.Instance.onGetSavedWorkerData += OnGetSupporterData;
        }

        private void UnsubscribeEvents()
        {
            WorkerSignals.Instance.onGetAmmoWorkerData -= OnGetAmmoWorkerData;
            WorkerSignals.Instance.onGetMoneyWorkerData -= OnGetMoneyWorkerData;
            WorkerSignals.Instance.onGetPayedAmmoWorkerData -= OnGetAmmoWorkerAreaPayedAmount;
            WorkerSignals.Instance.onGetPayedMoneyWorkerData -= OnGetMoneyWorkerAreaPayedAmount;
            WorkerSignals.Instance.onAmmoWorkerAreaBuyedItems -= OnSetPayedAmmoWorkerAreaData;
            WorkerSignals.Instance.onMoneyWorkerAreaBuyedItems -= OnSetPayedMoneyWorkerAreaData;
            SaveSignals.Instance.onGetSavedWorkerData -= OnGetSupporterData;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        #region Event Methods

        private AmmoWorkerBuyData OnGetAmmoWorkerData() => _data.AmmoWorkerBuyData;

        private MoneyWorkerBuyData OnGetMoneyWorkerData() => _data.MoneyWorkerBuyData;

        private int OnGetAmmoWorkerAreaPayedAmount()
        {
            return _payedAmounts.AmmoWorkerPayedAmount;
        }

        private int OnGetMoneyWorkerAreaPayedAmount()
        {
            return _payedAmounts.MoneyWorkerPayedAmount;
        }

        private void OnSetPayedAmmoWorkerAreaData(int payedAmount)
        {
            _ammoWorkerPayedAmount = payedAmount;
            SupportAreaDataSave();
        }

        private void OnSetPayedMoneyWorkerAreaData(int payedAmount)
        {
            _moneyWorkerPayedAmount = payedAmount;
            SupportAreaDataSave();
        }

        private SupporterBuyableDataParams OnGetSupporterData() => _payedAmounts;

        #endregion

        private void GetReferences()
        {
            _baseLevel = LevelSignals.Instance.onGetLevelID();
            _data = Resources.Load<CD_Level>("Data/CD_Level").LevelDatas[_baseLevel].WorkersData;
            _payedAmounts = SaveSignals.Instance.onLoadSupporterData();
            _ammoWorkerPayedAmount = _payedAmounts.AmmoWorkerPayedAmount;
            _moneyWorkerPayedAmount = _payedAmounts.MoneyWorkerPayedAmount;
        }

        private void Start()
        {
            WorkerSignals.Instance.onLoadedWorkerData?.Invoke();
        }

        private void SupportAreaDataSave()
        {
            _payedAmounts = new SupporterBuyableDataParams()
            {
                AmmoWorkerPayedAmount = _ammoWorkerPayedAmount,
                MoneyWorkerPayedAmount = _moneyWorkerPayedAmount
            };
            SaveSignals.Instance.onWorkerDataSave?.Invoke();
            SaveSignals.Instance.onScoreSave?.Invoke();
        }
    }
}