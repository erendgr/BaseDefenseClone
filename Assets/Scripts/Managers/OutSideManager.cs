using System.Collections.Generic;
using Datas.UnityObjects;
using Datas.ValueObjects;
using Enums;
using Signals;
using UnityEngine;

namespace Managers
{
    public class OutSideManager : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables

        private int _baseLevel;
        private FrontYardData _data;
        private Dictionary<OutSideStateLevels,int> _payedStageAreas;
        
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
            OutSideSignals.Instance.onGetOutsideData += OnGetOutsideData;
            OutSideSignals.Instance.onGetPayedStageData += OnGetOutsideStagePayedAmount;
            OutSideSignals.Instance.onOutsideBuyedItems += OnSetPayedStageData;
            SaveSignals.Instance.onGetSavedOutsideData += OnGetSavedOutsideData;
        }

        private void UnsubscribeEvents()
        {
            OutSideSignals.Instance.onGetOutsideData -= OnGetOutsideData;
            OutSideSignals.Instance.onGetPayedStageData -= OnGetOutsideStagePayedAmount;
            OutSideSignals.Instance.onOutsideBuyedItems -= OnSetPayedStageData;
            SaveSignals.Instance.onGetSavedOutsideData -= OnGetSavedOutsideData;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private void Start()
        {
            OutSideSignals.Instance.onGettedOutSideData?.Invoke();
        }

        private void GetReferences()
        {
            _baseLevel = LevelSignals.Instance.onGetLevelID();
            _data = Resources.Load<CD_Level>("Data/CD_Level").LevelDatas[_baseLevel].FrontYardData;
            _payedStageAreas = SaveSignals.Instance.onLoadOutsideData();
        }

        private OutsideData OnGetOutsideData(OutSideStateLevels level) => _data.OutsideLevelData[(int)level];

        private int OnGetOutsideStagePayedAmount(OutSideStateLevels level)
        {
            _payedStageAreas.TryAdd(level, 0);
            return _payedStageAreas[level];
        }
        
        private void OnSetPayedStageData(OutSideStateLevels level,int payedAmount)
        {
            _payedStageAreas[level] = payedAmount;
            OutsideDataSave();
        }
        
        private void OutsideDataSave()
        {
            SaveSignals.Instance.onOutSideDataSave?.Invoke();
        }

        private Dictionary<OutSideStateLevels, int> OnGetSavedOutsideData() => _payedStageAreas;
        
    }
}