using System.Collections.Generic;
using Datas.UnityObjects;
using Datas.ValueObjects;
using Datas.ValueObjects.Level;
using Enums;
using Keys;
using Signals;
using UnityEngine;

namespace Managers
{
    public class BaseManager : MonoBehaviour
    {
        #region Self Variables
        
        #region Serializefield Variables
        
        [SerializeField] private GameObject wareHouse;
        [SerializeField] private GameObject baseCenter;

        #endregion
        
        #region Private Variables

        private AreaDataParams _areaData;
        private Dictionary<RoomNameEnum, int> _payedRoomDatas;
        private Dictionary<TurretNameEnum, int> _payedTurretDatas;
        private BaseData _data;
        private int _baseLevel;
        
        #endregion

        #endregion

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
            _baseLevel = LevelSignals.Instance.onGetLevelID();
            _data = Resources.Load<CD_Level>("Data/CD_Level").LevelDatas[_baseLevel].BaseData;
            GetReferences();
        }

        private void SubscribeEvents()
        {
            BaseSignals.Instance.onBaseAreaBuyedItem += OnSetPayedRoomData;
            BaseSignals.Instance.onTurretAreaBuyedItem += OnSetPayedTurretData;
            SaveSignals.Instance.onGetSavedAreaData += OnGetAreaDatas;
            IdleSignals.Instance.onGetWareHousePositon += OnGetWareHousePosition;
            WorkerSignals.Instance.onGetBaseCenter += OnGetBaseCenter;
            BaseSignals.Instance.onGetSoldierAreaData += OnGetSoldierAreaData;
            BaseSignals.Instance.onGetMineAreaData += OnGetMineAreaData;

        }

        private void UnsubscribeEvents()
        {
            BaseSignals.Instance.onBaseAreaBuyedItem -= OnSetPayedRoomData;
            BaseSignals.Instance.onTurretAreaBuyedItem -= OnSetPayedTurretData;
            SaveSignals.Instance.onGetSavedAreaData -= OnGetAreaDatas;
            IdleSignals.Instance.onGetWareHousePositon -= OnGetWareHousePosition;
            WorkerSignals.Instance.onGetBaseCenter -= OnGetBaseCenter;
            BaseSignals.Instance.onGetSoldierAreaData -= OnGetSoldierAreaData;
            BaseSignals.Instance.onGetMineAreaData -= OnGetMineAreaData;

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
        
        private MineAreaData OnGetMineAreaData() => _data.MineAreaData;

        
        private SoldierAreaData OnGetSoldierAreaData() => _data.SoldierAreaData;
        
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
        
        private Transform OnGetWareHousePosition() => wareHouse.transform;

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
        
        private GameObject OnGetBaseCenter() => baseCenter;
    }
}