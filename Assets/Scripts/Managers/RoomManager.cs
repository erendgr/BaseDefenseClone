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
    public class RoomManager : MonoBehaviour
    {
        #region Self Variables
        
        #region SerializeField Variables

        [SerializeField] private RoomNameEnum roomName;
        [SerializeField] private GameObject area;
        [SerializeField] private GameObject fences;
        [SerializeField] private GameObject invisibleWall;
        [SerializeField] private TextMeshPro tmp;

        #endregion

        #region Private Variables

        private bool _isBase;
        private int _baseLevel;
        private int _payedAmount;
        private int _remainingAmount;
        private BaseData _data;
        private RoomData _roomData;
        private AreaDataParams _areaData;
        private ScoreDataParams _scoreCache;
        private GameObject _textParentGameObject;

        private int PayedAmount
        {
            get => _payedAmount;
            set
            {
                _payedAmount = value;
                _remainingAmount = _roomData.Cost - _payedAmount;
                if (_remainingAmount <= 0)
                {
                    _textParentGameObject.SetActive(false);
                    area.SetActive(true);
                    fences.SetActive(false);
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
            _roomData = _data.BaseRoomData.Rooms[(int)roomName];
            PayedAmount = GetRoomPayedAmount(roomName);
            
            BuyAreaImageChange();
        }
        
        private void GetDatas()
        {
            _baseLevel = LevelSignals.Instance.onGetLevelID();
            _data = Resources.Load<CD_Level>("Data/CD_Level").LevelDatas[_baseLevel].BaseData;
        }

        private int GetRoomPayedAmount(RoomNameEnum room)
        {
            var payedAmount = SaveSignals.Instance.onLoadAreaData().RoomPayedAmount 
                              ?? new Dictionary<RoomNameEnum, int>();

            if (!payedAmount.TryGetValue(room, out int value))
            {
                value = 0;
                payedAmount[roomName] = value;
            }

            return value;
        }
        
        public void BuyAreaEnter()
        {
            _scoreCache = ScoreSignals.Instance.onGetScoreData();
            switch (_roomData.PayType)
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
            BaseSignals.Instance.onBaseAreaBuyedItem?.Invoke(roomName, _payedAmount);
        }

        private IEnumerator Buy()
        {
            var waitForSecond = new WaitForSeconds(0.05f);
            while (_remainingAmount > 0)
            {
                PayedAmount += 10;
                ScoreSignals.Instance.onSetScore?.Invoke(_roomData.PayType, -10);
                yield return waitForSecond;
            }

            BaseSignals.Instance.onBaseAreaBuyedItem?.Invoke(roomName, _payedAmount);
        }
        
        private void SetText(int remainingAmount)
        {
            tmp.text = remainingAmount.ToString();
        }

        private void BuyAreaImageChange()
        {
            _textParentGameObject.transform.GetChild(((int)_roomData.PayType) + 1).gameObject.SetActive(false);
        }
    }
}