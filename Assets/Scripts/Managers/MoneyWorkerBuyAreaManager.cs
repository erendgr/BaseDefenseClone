using System;
using System.Collections;
using _ObjectPooling.Scripts.Enums;
using _ObjectPooling.Scripts.Signals;
using Datas.ValueObjects;
using Enums;
using Keys;
using Signals;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class MoneyWorkerBuyAreaManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private GameObject botPosition;
        [SerializeField] private TextMeshPro tmp;

        #endregion

        #region Private Variables

        private MoneyWorkerBuyData _data;
        private int _payedAmount;
        private Coroutine _buyCoroutine;
        private int _remainingAmount;
        private ScoreDataParams _scoreCache;
        private GameObject _textParentGameObject;

        private int PayedAmount
        {
            get => _payedAmount;
            set
            {
                _payedAmount = value;
                _remainingAmount = _data.Cost - _payedAmount;
                if (_remainingAmount <= 0)
                {
                    if (_buyCoroutine != null)
                    {
                        StopCoroutine(_buyCoroutine);
                        _buyCoroutine = null;
                        WorkerSignals.Instance.onMoneyWorkerAreaBuyedItems?.Invoke(_payedAmount);
                    }

                    var moneyWorker = PoolSignals.Instance.onDequeuePoolableGOWithTransform?.Invoke(PoolType.MoneyWorker, botPosition.transform);
                    moneyWorker.transform.localRotation = botPosition.transform.localRotation;
                    gameObject.SetActive(false);
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

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            WorkerSignals.Instance.onLoadedWorkerData += OnSetData;
        }

        private void UnsubscribeEvents()
        {
            WorkerSignals.Instance.onLoadedWorkerData -= OnSetData;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        #region Event Methods

        private void OnSetData()
        {
            _data = WorkerSignals.Instance.onGetMoneyWorkerData();
            PayedAmount = WorkerSignals.Instance.onGetPayedMoneyWorkerData();
            BuyAreaImageChange();
        }

        #endregion
        
        public void BuyAreaEnter()
        {
            _scoreCache = ScoreSignals.Instance.onGetScoreData();
            switch (_data.PayType)
            {
                case PayTypeEnum.Money:
                    if (_scoreCache.MoneyScore >= _remainingAmount)
                    {
                        _buyCoroutine = StartCoroutine(Buy());
                    }

                    break;
                case PayTypeEnum.Gem:
                    if (_scoreCache.GemScore >= _remainingAmount)
                    {
                        _buyCoroutine = StartCoroutine(Buy());
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void BuyAreaExit()
        {
            if (_buyCoroutine == null) return;
            StopCoroutine(_buyCoroutine);
            _buyCoroutine = null;
            WorkerSignals.Instance.onMoneyWorkerAreaBuyedItems?.Invoke(_payedAmount);
        }

        private IEnumerator Buy()
        {
            var waitForSecond = new WaitForSeconds(0.05f);
            while (_remainingAmount > 0)
            {
                PayedAmount += 10;
                ScoreSignals.Instance.onSetScore?.Invoke(_data.PayType, -10);
                yield return waitForSecond;
            }

            _buyCoroutine = null;
            WorkerSignals.Instance.onMoneyWorkerAreaBuyedItems?.Invoke(_payedAmount);
        }

        private void SetText(int remainingAmount)
        {
            tmp.text = remainingAmount.ToString();
        }

        private void BuyAreaImageChange()
        {
            _textParentGameObject.transform.GetChild(((int)_data.PayType) + 1).gameObject.SetActive(false);
        }
    }
}