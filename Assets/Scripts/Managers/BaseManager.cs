using System.Collections.Generic;
using Datas.ValueObjects;
using Enums;
using Keys;
using Signals;
using UnityEngine;

namespace Managers
{
    public class BaseManager : MonoBehaviour
    {
        #region Self Variables
        
        #region Private Variables

        private AreaDataParams _areaData;
        private Dictionary<RoomNameEnum, int> _payedRoomDatas;
        private Dictionary<TurretNameEnum, int> _payedTurretDatas;
        public BaseData _data;

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
            BaseSignals.Instance.onBaseAreaBuyedItem += OnSetPayedRoomData;
            BaseSignals.Instance.onTurretAreaBuyedItem += OnSetPayedTurretData;
            // BaseSignals.Instance.onTurretData += OnGetTurretData;
            // BaseSignals.Instance.onPayedTurretData += OnGetTurretPayedAmount;
            SaveSignals.Instance.onGetSavedAreaData += OnGetAreaDatas;
        }

        private void UnsubscribeEvents()
        {
            BaseSignals.Instance.onBaseAreaBuyedItem -= OnSetPayedRoomData;
            BaseSignals.Instance.onTurretAreaBuyedItem -= OnSetPayedTurretData;
            // BaseSignals.Instance.onTurretData -= OnGetTurretData;
            // BaseSignals.Instance.onPayedTurretData -= OnGetTurretPayedAmount;
            SaveSignals.Instance.onGetSavedAreaData -= OnGetAreaDatas;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private void GetReferences()
        {
            _payedRoomDatas = SaveSignals.Instance.onLoadAreaData().RoomPayedAmount;
            _payedTurretDatas = SaveSignals.Instance.onLoadAreaData().RoomTurretPayedAmount;
            //SetBaseLevelText();
        }
        
        private void OnSetPayedRoomData(RoomNameEnum room, int payedAmount)
        {
            _payedRoomDatas[room] = payedAmount;
            AreaDataSave();
        }

        private void OnSetPayedTurretData(TurretNameEnum turret, int payedAmount)
        {
            _payedTurretDatas[turret] = payedAmount;
            AreaDataSave();
        }

        private void AreaDataSave()
        {
            _areaData = new AreaDataParams
            {
                RoomPayedAmount = _payedRoomDatas,
                RoomTurretPayedAmount = _payedTurretDatas
            };
            SaveSignals.Instance.onAreaDataSave?.Invoke();
            SaveSignals.Instance.onScoreSave?.Invoke();
        }

        private AreaDataParams OnGetAreaDatas() => _areaData;
    }
}