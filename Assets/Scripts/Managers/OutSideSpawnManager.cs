using System;
using System.Collections.Generic;
using System.Linq;
using _ObjectPooling.Scripts.Enums;
using _ObjectPooling.Scripts.Signals;
using Datas.UnityObjects;
using Datas.ValueObjects;
using Enums;
using Signals;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class OutSideSpawnManager : MonoBehaviour
    {
        #region Self Variables

        #region SerializeField Variables

        [SerializeField] private List<GameObject> enemySpawnPoints;
        [SerializeField] private List<GameObject> hostageSpawnPoint;
        [SerializeField] private List<GameObject> turretPoints;
        [SerializeField] private int spawnTimer;

        #endregion

        #region Private Variables

        private int _currentLevel;
        private int _hostageCache;
        private float _enemyTimer;
        private float _hostageTimer;
        private FrontYardData _data;
        private int _randomSpawnDatas;
        private int _randomTurretPoint;
        private int _randomTurretPoints;
        private SpawnData _spawnDataCache;
        private SpawnData _randomSpawnDataCache;
        private List<SpawnData> _spawnDataList;
        private List<GameObject> _hostageSpawnControlList = new();
        private Dictionary<GameObject, List<SpawnData>> _turretsSpawnDatas = new();

        #endregion

        #endregion

        private void Awake()
        {
            _data = Resources.Load<CD_Level>("Data/CD_Level").LevelDatas[_currentLevel].FrontYardData;
        }

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
            _currentLevel = LevelSignals.Instance.onGetLevelID();
        }

        private void SubscribeEvents()
        {
            IdleSignals.Instance.onEnemyHasTarget += OnSetEnemyTarget;
            IdleSignals.Instance.onHostageCollected += OnRemoveHostageFromList;
            IdleSignals.Instance.onEnemyDead += OnRemoveEnemyFromDict;
        }

        private void UnsubscribeEvents()
        {
            IdleSignals.Instance.onEnemyHasTarget -= OnSetEnemyTarget;
            IdleSignals.Instance.onHostageCollected -= OnRemoveHostageFromList;
            IdleSignals.Instance.onEnemyDead -= OnRemoveEnemyFromDict;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        #region Event Methods

        private GameObject OnSetEnemyTarget() => turretPoints[_randomTurretPoints];

        private void OnRemoveHostageFromList(GameObject hostage)
        {
            _hostageSpawnControlList[_hostageSpawnControlList.IndexOf(hostage)] = null;
        }

        private void OnRemoveEnemyFromDict(GameObject enemyTarget, EnemyType type)
        {
            _spawnDataList = _turretsSpawnDatas[enemyTarget];
            _spawnDataList[(int)type].CurrentCount--;
        }

        #endregion

        private void Start()
        {
            InitSpawnDictionary();
        }

        private void InitSpawnDictionary()
        {
            EnemyDict();
            _hostageSpawnControlList = new List<GameObject>(new GameObject[hostageSpawnPoint.Count]);
        }

        private void Update()
        {
            _enemyTimer += Time.deltaTime;
            _hostageTimer += Time.deltaTime;
            if (_enemyTimer >= spawnTimer)
            {
                if (CheckCurrentEnemy())
                {
                    SpawnEnemy();
                    _enemyTimer = 0;
                }
            }

            if (_hostageTimer >= spawnTimer)
            {
                if (CheckCurrentHostage())
                {
                    SpawnHostage();
                    _hostageTimer = 0;
                }
            }
        }

        private void EnemyDict()
        {
            foreach (var point in turretPoints)
            {
                _spawnDataList = new List<SpawnData>();
                foreach (var spawnData in _data.SpawnDatas)
                {
                    _spawnDataCache = new SpawnData
                    {
                        EnemyType = spawnData.EnemyType,
                        EnemyCount = spawnData.EnemyCount,
                        CurrentCount = spawnData.CurrentCount
                    };
                    _spawnDataList.Add(_spawnDataCache);
                }

                _turretsSpawnDatas.Add(point, _spawnDataList);
            }
        }

        private bool CheckCurrentEnemy() => _turretsSpawnDatas.Values.Any(spawnDatas =>
            spawnDatas.Any(spawnData => spawnData.CurrentCount < spawnData.EnemyCount));

        private bool CheckCurrentHostage() => _hostageSpawnControlList.Any(obj => obj == null);

        private void SpawnEnemy()
        {
            RandomEnemy();
            PoolType poolType = (PoolType) Enum.Parse(typeof(PoolType), _randomSpawnDataCache.EnemyType.ToString());
            var enemy = PoolSignals.Instance.onDequeuePoolableGameObjectWithTransform(poolType,
                enemySpawnPoints[Random.Range(0, enemySpawnPoints.Count)].transform);
        }

        private void SpawnHostage()
        {
            if (_hostageSpawnControlList[_hostageCache] == null)
            {
                _hostageSpawnControlList[_hostageCache] =
                    PoolSignals.Instance.onDequeuePoolableGameObjectWithTransform(PoolType.Hostage,
                        hostageSpawnPoint[_hostageCache].transform);
                return;
            }

            if (_hostageCache == hostageSpawnPoint.Count - 1)
            {
                _hostageCache = 0;
                return;
            }

            _hostageCache++;
        }

        private void RandomEnemy()
        {
            while (true)
            {
                _randomTurretPoints = Random.Range(0, turretPoints.Count);
                _randomSpawnDatas = Random.Range(0, _spawnDataList.Count);
                _randomSpawnDataCache = _turretsSpawnDatas[turretPoints[_randomTurretPoints]][_randomSpawnDatas];
                if (_randomSpawnDataCache.CurrentCount >= _randomSpawnDataCache.EnemyCount) continue;
                _turretsSpawnDatas[turretPoints[_randomTurretPoints]][_randomSpawnDatas].CurrentCount++;
                break;
            }
        }
    }
}