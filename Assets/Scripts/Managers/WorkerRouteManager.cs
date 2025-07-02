using System.Collections.Generic;
using Datas.UnityObjects;
using Datas.ValueObjects.Level;
using Extentions;
using Signals;
using UnityEngine;

namespace Managers
{
    public class WorkerRouteManager : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables

        private GameObject _turretArea;
        private GameObject _targetMoney;
        private StaticStackData _turretData;
        private List<GameObject> _moneyList = new();
        private Dictionary<GameObject,List<GameObject>> _turretAmmoAreas = new Dictionary<GameObject, List<GameObject>>();

        #endregion
        #endregion
        private void Awake()
        {
            _turretData = GetTurretData();
        }
        
        private StaticStackData GetTurretData() => Resources.Load<CD_Turret>("Data/CD_Turret").TurretData.TurretStackData;
        
        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            WorkerSignals.Instance.onTurretAmmoAreas += OnTurretAmmoAreas;
            WorkerSignals.Instance.onGetTurretArea += OnGetTurretArea;
            WorkerSignals.Instance.onAddMoneyToList += OnAddMoneyToList;
            WorkerSignals.Instance.onRemoveMoneyFromList += OnRemoveMoneyFromList;
            WorkerSignals.Instance.onGetMoneyGameObject += OnGetMoneyGameObject;
        }

        private void UnsubscribeEvents()
        {
            WorkerSignals.Instance.onTurretAmmoAreas -= OnTurretAmmoAreas;
            WorkerSignals.Instance.onGetTurretArea -= OnGetTurretArea;
            WorkerSignals.Instance.onAddMoneyToList -= OnAddMoneyToList;
            WorkerSignals.Instance.onRemoveMoneyFromList -= OnRemoveMoneyFromList;
            WorkerSignals.Instance.onGetMoneyGameObject -= OnGetMoneyGameObject;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private void OnTurretAmmoAreas(GameObject ammoArea, List<GameObject> ammoAreaStackList)
        {
            _turretAmmoAreas.Add(ammoArea,ammoAreaStackList);
        }

        private GameObject OnGetTurretArea()
        {
            var maxCapacity = 0;
            GameObject result = null;

            foreach (var targetArea in _turretAmmoAreas)
            {
                var remainingCapacity = _turretData.Capacity - targetArea.Value.Count;
                if (remainingCapacity <= 0) continue;
        
                if (remainingCapacity > maxCapacity)
                {
                    maxCapacity = remainingCapacity;
                    result = targetArea.Key;
                }
            }

            return result;
        }


        private void OnAddMoneyToList(GameObject money)
        {
            if (!_moneyList.Contains(money))
            {
                _moneyList.Add(money);
            }
        }

        private void OnRemoveMoneyFromList(GameObject money)
        {
            _moneyList.Remove(money);
            _moneyList.TrimExcess();
            if (money == _targetMoney)
            {
                WorkerSignals.Instance.onChangeDestination?.Invoke();
            }
        }

        private GameObject OnGetMoneyGameObject()
        {
            if (_moneyList.IsNullOrEmpty()) return null;
            _targetMoney = _moneyList[0];
            return _targetMoney;
        }
    }
}