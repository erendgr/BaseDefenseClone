﻿using System.Collections.Generic;
using Enums;
using Keys;
using Signals;
using UnityEngine;

namespace Managers
{
    public class SaveManager : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables

        private int _levelCache;
        private ScoreDataParams _scoreDataCache;
        private AreaDataParams _areaDataCache;
        private Dictionary<OutSideStateLevels, int> _outsideDataCache;
        private SupporterBuyableDataParams _supporterBuyableDataCache;

        #endregion

        #endregion
        
        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            SaveSignals.Instance.onLevelSave += OnLevelSave;
            SaveSignals.Instance.onScoreSave += OnScoreSave;
            SaveSignals.Instance.onAreaDataSave += OnAreaDataSave;
            SaveSignals.Instance.onOutSideDataSave += OnOutsideStageDataSave;
            SaveSignals.Instance.onWorkerDataSave += OnSupporterDataSave;
            SaveSignals.Instance.onLoadCurrentLevel += OnLoadLevel;
            SaveSignals.Instance.onLoadScoreData += OnLoadScoreData;
            SaveSignals.Instance.onLoadAreaData += OnLoadAreaData;
            SaveSignals.Instance.onLoadOutsideData += OnLoadOutsideStageData;
            SaveSignals.Instance.onLoadSupporterData += OnLoadSupporterData;
        }

        private void UnsubscribeEvents()
        {
            SaveSignals.Instance.onLevelSave -= OnLevelSave;
            SaveSignals.Instance.onScoreSave -= OnScoreSave;
            SaveSignals.Instance.onAreaDataSave -= OnAreaDataSave;
            SaveSignals.Instance.onOutSideDataSave -= OnOutsideStageDataSave;
            SaveSignals.Instance.onLoadCurrentLevel -= OnLoadLevel;
            SaveSignals.Instance.onLoadScoreData -= OnLoadScoreData;
            SaveSignals.Instance.onLoadAreaData -= OnLoadAreaData;
            SaveSignals.Instance.onLoadOutsideData -= OnLoadOutsideStageData;
            SaveSignals.Instance.onWorkerDataSave -= OnSupporterDataSave;
            SaveSignals.Instance.onLoadSupporterData -= OnLoadSupporterData;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion
        
        private void OnLevelSave()
        {
            _levelCache = SaveSignals.Instance.onGetSavedLevelData();
            if (_levelCache != 0) ES3.Save("Level", _levelCache, "Level.es3");
        }
        
        private void OnScoreSave()
        {
            _scoreDataCache = new ScoreDataParams()
            {
                MoneyScore = SaveSignals.Instance.onGetSavedScoreData().MoneyScore,
                GemScore = SaveSignals.Instance.onGetSavedScoreData().GemScore
            };
            if (_scoreDataCache.MoneyScore != 0) ES3.Save("MoneyScore", _scoreDataCache.MoneyScore, "ScoreData.es3");
            if (_scoreDataCache.GemScore != 0) ES3.Save("GemScore", _scoreDataCache.GemScore, "ScoreData.es3");
        }
        
        private void OnAreaDataSave()
        {
            _areaDataCache = new AreaDataParams()
            {
                RoomPayedAmount = SaveSignals.Instance.onGetSavedAreaData().RoomPayedAmount,
                RoomTurretPayedAmount = SaveSignals.Instance.onGetSavedAreaData().RoomTurretPayedAmount
            };
            if (_areaDataCache.RoomPayedAmount != null)
                ES3.Save("RoomPayedAmount",
                    _areaDataCache.RoomPayedAmount, "AreaData.es3");
            if (_areaDataCache.RoomTurretPayedAmount != null)
                ES3.Save("RoomTurretPayedAmount",
                    _areaDataCache.RoomTurretPayedAmount, "AreaData.es3");
        }

        private void OnOutsideStageDataSave()
        {
            _outsideDataCache = SaveSignals.Instance.onGetOutsideData();
            if (_outsideDataCache != null) ES3.Save("OutsidePayedAmount", _outsideDataCache, "OutsideData.es3");
        }

        private void OnSupporterDataSave()
        {
            _supporterBuyableDataCache = new SupporterBuyableDataParams()
            {
                AmmoWorkerPayedAmount = SaveSignals.Instance.onGetSavedWorkerData().AmmoWorkerPayedAmount,
                MoneyWorkerPayedAmount = SaveSignals.Instance.onGetSavedWorkerData().MoneyWorkerPayedAmount
            };
            if (_supporterBuyableDataCache.AmmoWorkerPayedAmount != 0)
                ES3.Save("AmmoWorkerPayedAmount",
                    _supporterBuyableDataCache.AmmoWorkerPayedAmount, "SupporterData.es3");
            if (_supporterBuyableDataCache.MoneyWorkerPayedAmount != 0)
                ES3.Save("MoneyWorkerPayedAmount",
                    _supporterBuyableDataCache.MoneyWorkerPayedAmount, "SupporterData.es3");
        }
        
        private int OnLoadLevel()
        {
            return ES3.KeyExists("Level", "Level.es3")
                ? ES3.Load<int>("Level", "Level.es3")
                : 1;
        }

        private ScoreDataParams OnLoadScoreData()
        {
            return new ScoreDataParams
            {
                MoneyScore = ES3.KeyExists("MoneyScore", "ScoreData.es3")
                    ? ES3.Load<int>("MoneyScore", "ScoreData.es3")
                    : 5000,
                GemScore = ES3.KeyExists("GemScore", "ScoreData.es3")
                    ? ES3.Load<int>("GemScore", "ScoreData.es3")
                    : 1000
            };
        }

        private AreaDataParams OnLoadAreaData()
        {
            return new AreaDataParams
            {
                RoomPayedAmount = ES3.KeyExists("RoomPayedAmount", "AreaData.es3")
                    ? ES3.Load<Dictionary<RoomNameEnum, int>>("RoomPayedAmount", "AreaData.es3")
                    : new Dictionary<RoomNameEnum, int>(),
                RoomTurretPayedAmount = ES3.KeyExists("RoomTurretPayedAmount", "AreaData.es3")
                    ? ES3.Load<Dictionary<TurretNameEnum, int>>("RoomTurretPayedAmount", "AreaData.es3")
                    : new Dictionary<TurretNameEnum, int>()
            };
        }

        private Dictionary<OutSideStateLevels, int> OnLoadOutsideStageData()
        {
            return ES3.KeyExists("OutsidePayedAmount", "OutsideData.es3")
                ? ES3.Load<Dictionary<OutSideStateLevels, int>>("OutsidePayedAmount", "OutsideData.es3")
                : new Dictionary<OutSideStateLevels, int>();
        }

        private SupporterBuyableDataParams OnLoadSupporterData()
        {
            return new SupporterBuyableDataParams()
            {
                AmmoWorkerPayedAmount = ES3.KeyExists("AmmoWorkerPayedAmount", "SupporterData.es3")
                ? ES3.Load<int>("AmmoWorkerPayedAmount", "SupporterData.es3") : 0,
                MoneyWorkerPayedAmount = ES3.KeyExists("MoneyWorkerPayedAmount", "SupporterData.es3")
                ? ES3.Load<int>("MoneyWorkerPayedAmount", "SupporterData.es3") : 0,
            };
        }
    }
}