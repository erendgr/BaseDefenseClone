using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _ObjectPooling.Scripts.Enums;
using _ObjectPooling.Scripts.Signals;
using Command.Stack;
using Datas.UnityObjects;
using Datas.ValueObjects.Player;
using Enums;
using Signals;
using UnityEngine;

namespace Managers
{
    public class PlayerStackManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private GameObject stackHolder;

        #endregion

        #region Private Variables

        private StackType _stackType;
        private Vector3 _stackPosition;
        private Transform _wareHouseTransform;
        private StackData _ammoStackData;
        private StackData _moneyStackData;
        private List<GameObject> _stackList = new();
        private readonly List<GameObject> _hostageList = new();

        private ClearPlayerStack _clearPlayerStack;
        private ClearAmmoStackItems _clearAmmoStackItems;
        private AddMoneyStackToScore _addMoneyStackToScore;
        private AddItemToStackCommand _addItemToStackCommand;
        private GetPlayerStackItemPosCommand _ammoStackItemPosCommand;
        private GetPlayerStackItemPosCommand _moneyStackItemPosCommand;

        #endregion

        #endregion

        private void Awake()
        {
            var transformParent = transform.parent.transform;
            _moneyStackData = Resources.Load<CD_Player>("Data/CD_Player").MoneyStackData;
            _ammoStackData = Resources.Load<CD_Player>("Data/CD_Player").AmmoStackData;
            _ammoStackItemPosCommand =
                new GetPlayerStackItemPosCommand(ref _stackList, ref _ammoStackData, ref stackHolder);
            _moneyStackItemPosCommand =
                new GetPlayerStackItemPosCommand(ref _stackList, ref _moneyStackData, ref stackHolder);
            _addItemToStackCommand = new AddItemToStackCommand(ref _stackList, ref stackHolder);
            _addMoneyStackToScore = new AddMoneyStackToScore(ref _stackList);
            _clearPlayerStack = new ClearPlayerStack(ref _stackList, ref transformParent);
        }

        #region Event Subscription

        private void OnEnable()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            StackSignals.Instance.onGetHostageTarget += OnGetHostageTarget;
            StackSignals.Instance.onLastGameObjectRemove += OnLastHostageRemove;
            StackSignals.Instance.onGetHostageList += OnGetHostageList;
            IdleSignals.Instance.onPlayerDied += OnPlayerDied;
        }


        private void Unsubscribe()
        {
            StackSignals.Instance.onGetHostageTarget -= OnGetHostageTarget;
            StackSignals.Instance.onLastGameObjectRemove -= OnLastHostageRemove;
            StackSignals.Instance.onGetHostageList -= OnGetHostageList;
            IdleSignals.Instance.onPlayerDied -= OnPlayerDied;
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        #endregion

        #region Event Methods

        private GameObject OnGetHostageTarget(GameObject hostage)
        {
            if (_hostageList.Count == 0)
            {
                _hostageList.Add(hostage);
                return transform.gameObject;
            }

            _hostageList.Add(hostage);
            return _hostageList[^2];
        }

        private List<GameObject> OnGetHostageList() => _hostageList;

        private void OnLastHostageRemove(bool sendPool)
        {
            if (sendPool)
            {
                PoolSignals.Instance.onEnqueuePooledGameObject?.Invoke(_hostageList.Last(), PoolType.Hostage);
            }

            _hostageList.Remove(_hostageList.Last());
        }

        private void OnPlayerDied()
        {
            _clearPlayerStack.Execute();
            _stackType = StackType.None;
        }

        #endregion

        private void Start()
        {
            _wareHouseTransform = IdleSignals.Instance.onGetWareHousePositon();
            _clearAmmoStackItems = new ClearAmmoStackItems(ref _stackList, _wareHouseTransform);
        }

        public void InteractWareHouseArea(Transform ammoArea, bool isTriggerAmmoArea)
        {
            if (_stackType == StackType.Money) return;
            if (isTriggerAmmoArea)
            {
                StartCoroutine(AddAmmoToStack(ammoArea));
            }
            else
            {
                StopAllCoroutines();
            }
        }

        private IEnumerator AddAmmoToStack(Transform ammoArea)
        {
            WaitForSeconds delay = new WaitForSeconds(0.3f);
            while (_stackList.Count < _ammoStackData.Capacity)
            {
                _stackType = StackType.Ammo;
                _stackPosition = _ammoStackItemPosCommand.Execute(_stackPosition);
                var obj = PoolSignals.Instance.onDequeuePoolableGameObject(PoolType.AmmoBox);
                _addItemToStackCommand.Execute(obj, _stackPosition, ammoArea);
                yield return delay;
            }
        }

        public void InteractTurretAmmoArea(GameObject ammoArea)
        {
            if (_stackType == StackType.Ammo)
            {
                StackSignals.Instance.onInteractStackHolder?.Invoke(ammoArea, _stackList);
            }
        }

        public void CheckAmmoStack()
        {
            if (_stackList.Count == 0)
            {
                _stackType = StackType.None;
            }
        }

        public void InteractMoney(GameObject money)
        {
            if (_stackType == StackType.Ammo) return;
            _stackType = StackType.Money;
            if (_stackList.Count >= _moneyStackData.Capacity) return;
            money.GetComponent<BoxCollider>().enabled = false;
            _stackPosition = _moneyStackItemPosCommand.Execute(_stackPosition);
            _addItemToStackCommand.Execute(money, _stackPosition);
            WorkerSignals.Instance.onRemoveMoneyFromList?.Invoke(money);
        }

        public void InteractBarrierArea()
        {
            switch (_stackType)
            {
                case StackType.None:
                    return;
                case StackType.Money:
                    _addMoneyStackToScore.Execute();
                    _stackType = StackType.None;
                    return;
                case StackType.Ammo:
                    _clearAmmoStackItems.Execute();
                    _stackType = StackType.None;
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}