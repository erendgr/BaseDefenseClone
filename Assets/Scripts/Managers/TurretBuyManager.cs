using System;
using System.Collections;
using System.Collections.Generic;
using Datas.UnityObjects;
using Datas.ValueObjects;
using Enums;
using Keys;
using Signals;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class TurretBuyManager : MonoBehaviour
    {
        #region Self Variables

        #region SerializeField Variables

        [SerializeField] private TurretNameEnum turretName;
        [SerializeField] private TextMeshPro tmp;
        [SerializeField] private TurretManager turretManager;
        [SerializeField] private GameObject go;
        
        #endregion

        #region Private Variables
        
        private int _baseLevel;
        private int _payedAmount;
        private int _remainingAmount;
        private BaseData _data;
        private BuyableTurretData _buyableTurretData;
        private ScoreDataParams _scoreCache;
        private GameObject _textParentGameObject;

        private int PayedAmount
        {
            get => _payedAmount;
            set
            {
                _payedAmount = value;
                _remainingAmount = _buyableTurretData.Cost - _payedAmount;
                if (_remainingAmount <= 0)
                {
                    //for test
                    go.SetActive(true);
                    //turretManager.SetSoldier();
                    _textParentGameObject.SetActive(false);
                }
                else
                {
                    SetText(_remainingAmount);
                }
            }
        }
        
        #endregion

        #endregion
        
        
        private void Awake()
        {
            _textParentGameObject = tmp.transform.parent.gameObject;
        }

        private void OnEnable()
        {
            GetDatas();
            SetDatas();
        }
        
        private void SetDatas()
        {
            _buyableTurretData = _data.BaseRoomData.Rooms[(int)turretName].buyableTurretData;
            PayedAmount = GetRoomPayedAmount(turretName);
            
            BuyAreaImageChange();
        }
        
        private void GetDatas()
        {
            _baseLevel = LevelSignals.Instance.onGetLevelID();
            _data = Resources.Load<CD_Level>("Data/CD_Level").LevelDatas[_baseLevel].BaseData;
        }
        
        private int GetRoomPayedAmount(TurretNameEnum turret)
        {
            var payedAmount = SaveSignals.Instance.onLoadAreaData().RoomTurretPayedAmount 
                              ?? new Dictionary<TurretNameEnum, int>();
            if (!payedAmount.TryGetValue(turret, out int value))
            {
                value = 0;
                payedAmount[turret] = value;
            }
            return value;
        }
        
        public void BuyAreaEnter()
        {
            _scoreCache = ScoreSignals.Instance.onGetScoreData();
            switch (_buyableTurretData.PayType)
            {
                case PayTypeEnum.Money:
                    if (_scoreCache.MoneyScore >= _remainingAmount)
                    {
                        StartCoroutine(Buy());
                    }

                    break;
                case PayTypeEnum.Gem:
                    if (_scoreCache.GemScore >= _remainingAmount)
                    {
                        StartCoroutine(Buy());
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void BuyAreaExit()
        {
            StopAllCoroutines();
            BaseSignals.Instance.onTurretAreaBuyedItem?.Invoke(turretName, _payedAmount);
        }

        private IEnumerator Buy()
        {
            var waitForSecond = new WaitForSeconds(0.02f);
            while (_remainingAmount > 0)
            {
                PayedAmount += 10;
                ScoreSignals.Instance.onSetScore?.Invoke(_buyableTurretData.PayType, -10);
                yield return waitForSecond;
            }

            BaseSignals.Instance.onTurretAreaBuyedItem?.Invoke(turretName, _payedAmount);
        }
        
        private void SetText(int remainingAmount)
        {
            tmp.text = remainingAmount.ToString();
        }

        private void BuyAreaImageChange()
        {
            _textParentGameObject.transform.GetChild(((int)_buyableTurretData.PayType) + 1).gameObject.SetActive(false);
        }
    }
}