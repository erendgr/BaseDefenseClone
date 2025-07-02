using System.Collections.Generic;
using System.Linq;
using _ObjectPooling.Scripts.Enums;
using _ObjectPooling.Scripts.Signals;
using Command.Stack;
using Datas.ValueObjects.Level;
using DG.Tweening;
using Enums;
using Signals;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class MineAreaManager : MonoBehaviour
    {
        #region Self Variables

        #region SerializeField Variables

        [SerializeField] private TextMeshPro tmp;
        [SerializeField] private GameObject gemAreaHolder;
        [SerializeField] private List<GameObject> mines = new();

        #endregion

        #region Private Variables

        private MineAreaData _data;
        private List<int> _capacity;
        private List<GameObject> _gemHolderGameObjects = new();
        private List<GameObject> _hostageList;
        private List<GameObject> _gemHolderGameObjectsCache;
        private GetStaticStackItemPosCommand _getStackItemPosition;
        private AddItemToStackCommand _addItemToStack;
        private int _random;
        private Vector3 _direction;
        private int _currentMiner;

        #endregion

        #endregion

        #region Event Subscriptions

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            IdleSignals.Instance.onGetMineGameObject += OnGetMineGameObject;
            IdleSignals.Instance.onAddGemToGemHolder += OnGemHolderAddGem;
            IdleSignals.Instance.onGetGemAreaHolder += OnGetGemAreaHolder;
        }

        private void UnsubscribeEvents()
        {
            IdleSignals.Instance.onGetMineGameObject -= OnGetMineGameObject;
            IdleSignals.Instance.onAddGemToGemHolder -= OnGemHolderAddGem;
            IdleSignals.Instance.onGetGemAreaHolder -= OnGetGemAreaHolder;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        #region Event Methods

        private GameObject OnGetMineGameObject()
        {
            while (true)
            {
                _random = Random.Range(0, _capacity.Count);
                if (_capacity[_random] != _data.MaxWorkerFromMine)
                {
                    _capacity[_random]++;
                    break;
                }

                if (_capacity.Any(c => c != _data.MaxWorkerFromMine)) continue;
                throw new System.NotImplementedException();
            }

            return mines[_random];
        }

        private void OnGemHolderAddGem(Transform miner)
        {
            var position = miner.position;
            position = new Vector3(position.x, position.y + 1, position.z);
            miner.position = position;
            GameObject gem = PoolSignals.Instance.onDequeuePoolableGOWithTransform?.Invoke(PoolType.Gem, miner);
            SetGemPosition(gem);
        }

        private GameObject OnGetGemAreaHolder() => gemAreaHolder;

        #endregion

        private void Start()
        {
            _currentMiner = 0;
            _capacity = new List<int>(new int [mines.Count]);
            _data = BaseSignals.Instance.onGetMineAreaData();
            _getStackItemPosition = new GetStaticStackItemPosCommand(ref _gemHolderGameObjects,
                ref _data.StaticStackData, ref gemAreaHolder);
            _addItemToStack = new AddItemToStackCommand(ref _gemHolderGameObjects, ref gemAreaHolder);
            _hostageList = StackSignals.Instance.onGetHostageList();
            SetText();
        }

        public void PlayerTriggerEnter(Transform other)
        {
            _gemHolderGameObjectsCache = new List<GameObject>(_gemHolderGameObjects);
            _gemHolderGameObjects.Clear();
            for (int i = 0; i < _gemHolderGameObjectsCache.Count; i++)
            {
                var random = new Vector3(Random.Range(-3f, 3f), Random.Range(0f, 3f), Random.Range(-3f, 3f));
                var obj = _gemHolderGameObjectsCache[i];
                obj.transform.SetParent(other);
                obj.transform
                    .DOLocalMove(obj.transform.localPosition + random, 0.5f);
                obj.transform.DOLocalMove(Vector3.zero, 0.5f).SetDelay(0.5f).OnComplete(() =>
                {
                    PoolSignals.Instance.onEnqueuePooledGameObject?.Invoke(obj, PoolType.Gem);
                });
            }

            ScoreSignals.Instance.onSetScore?.Invoke(PayTypeEnum.Gem, _gemHolderGameObjectsCache.Count);
            _gemHolderGameObjectsCache.Clear();
            SaveSignals.Instance.onScoreSave?.Invoke();
        }


        private void SetGemPosition(GameObject gem)
        {
            _direction = _getStackItemPosition.Execute(_direction);
            _addItemToStack.Execute(gem, _direction);
        }


        public void PlayerEntryGemArea()
        {
            if (_hostageList.Count <= 0) return;
            while (_currentMiner < _data.MaxWorkerAmound)
            {
                if (_hostageList.Count <= 0) break;
                GameObject miner =
                    PoolSignals.Instance.onDequeuePoolableGOWithTransform(PoolType.Miner,
                        _hostageList.Last().transform);
                miner.transform.rotation = _hostageList.Last().transform.rotation;
                StackSignals.Instance.onLastGameObjectRemove?.Invoke(true);
                _currentMiner++;
                SetText();
            }
        }


        private void SetText()
        {
            tmp.SetText(_currentMiner.ToString() + " / " + _data.MaxWorkerAmound);
        }
    }
}