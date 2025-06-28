using System.Collections.Generic;
using _ObjectPooling.Scripts.Data.UnityObjects;
using _ObjectPooling.Scripts.Enums;
using _ObjectPooling.Scripts.Signals;
using UnityEngine;


namespace _ObjectPooling.Scripts.Managers
{
    public class PoolingManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private CD_Pool data;

        [SerializeField] private Transform poolParent;

        #endregion

        #region Private Variables

        private Dictionary<PoolType, Queue<GameObject>> _poolableObjectList;

        #endregion

        #endregion

        private void Awake()
        {
            data = GetPoolData();
        }

        private CD_Pool GetPoolData()
        {
            return Resources.Load<CD_Pool>("Data/CD_Pool");
        }

        #region Event Subscriptions

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            PoolSignals.Instance.onDequeuePoolableGameObject += OnDequeuePoolableGameObject;
            PoolSignals.Instance.onDequeuePoolableGameObjectWithTransform += OnDequeuePoolableGameObjectWithTransform;
            PoolSignals.Instance.onEnqueuePooledGameObject += OnEnqueuePooledGameObject;
            
        }

        private GameObject OnDequeuePoolableGameObjectWithTransform(PoolType poolType, Transform transform)
        {
            if (!_poolableObjectList.ContainsKey(poolType))
            {
                Debug.LogError($"Dictionary does not contain this key: {poolType}...");
                return null;
            }

            var deQueuedPoolObject = _poolableObjectList[poolType].Dequeue();
            deQueuedPoolObject.transform.position = transform.position;
            if (deQueuedPoolObject.activeSelf) OnDequeuePoolableGameObjectWithTransform(poolType, transform);
            deQueuedPoolObject.SetActive(true);
            return deQueuedPoolObject;
        }

        private GameObject OnDequeuePoolableGameObject(PoolType type)
        {
            if (!_poolableObjectList.ContainsKey(type))
            {
                Debug.LogError($"Dictionary does not contain this key: {type}...");
                return null;
            }

            var deQueuedPoolObject = _poolableObjectList[type].Dequeue();
            if (deQueuedPoolObject.activeSelf) OnDequeuePoolableGameObject(type);
            deQueuedPoolObject.SetActive(true);
            return deQueuedPoolObject;
        }

        private void OnEnqueuePooledGameObject(GameObject poolObject, PoolType type)
        {
            poolObject.transform.parent = poolParent.transform;
            poolObject.transform.localPosition = Vector3.zero;
            poolObject.transform.localEulerAngles = Vector3.zero;

            poolObject.gameObject.SetActive(false);

            _poolableObjectList[type].Enqueue(poolObject);
        }

        private void UnSubscribeEvents()
        {
            PoolSignals.Instance.onDequeuePoolableGameObject -= OnDequeuePoolableGameObject;
            PoolSignals.Instance.onEnqueuePooledGameObject -= OnEnqueuePooledGameObject;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        #endregion

        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            _poolableObjectList = new Dictionary<PoolType, Queue<GameObject>>();

            foreach (var pool in data.PoolList)
            {
                var goParent = new GameObject
                {
                    name = pool.Key.ToString(),
                    transform =
                    {
                        parent = poolParent
                    }
                };
                Queue<GameObject> poolableObjects = new Queue<GameObject>();

                for (int i = 0; i < pool.Value.Amount; i++)
                {
                    var go = Instantiate(pool.Value.Prefab, goParent.transform, true);
                    go.SetActive(false);
                    go.transform.SetParent(goParent.transform);
                    pool.Value.Type = pool.Key;

                    poolableObjects.Enqueue(go);
                }

                _poolableObjectList.Add(pool.Key, poolableObjects);
            }
        }
    }
}