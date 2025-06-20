using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _ObjectPooling.Scripts.Enums;
using _ObjectPooling.Scripts.Signals;
using Datas.ValueObjects.Level;
using Managers;
using UnityEngine;

namespace Controllers.Turret
{
    public class TurretAmmoAreaController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private TurretManager manager;

        #endregion

        #region Private Variables

        private Vector3 _direction;
        private Coroutine _addStack;
        private StaticStackData _data;
        private WaitForSeconds _delay;
        private bool _isInteractPlayer;
        private List<GameObject> _ammoList;
        private List<GameObject> _managerStackList;

        #endregion

        #endregion

        public void SetData(StaticStackData data, List<GameObject> managerStackList)
        {
            _managerStackList = managerStackList;
            _data = data;
            _delay = new WaitForSeconds(data.Delay);
        }

        public void AddAmmoToStack(List<GameObject> ammoBoxes)
        {
            _ammoList = ammoBoxes;
            if (_addStack != null) return;
            _isInteractPlayer = true;
            _addStack = StartCoroutine(AddAmmo());
        }

        public void RemoveAmmoFromStack()
        {
            var ammoBox = _managerStackList.Last();
            PoolSignals.Instance.onEnqueuePooledGameObject(ammoBox, PoolType.AmmoBox);
            _managerStackList.Remove(ammoBox);
        }

        private IEnumerator AddAmmo()
        {
            while (_managerStackList.Count < _data.Capacity && (_isInteractPlayer /*|| _isInteractAmmoWorker*/))
            {
                if (_ammoList.Count == 0) break;
                var ammoBox = _ammoList.Last();
                _direction = manager.GetItemPosCommand.Execute(_direction);
                manager.AddToStackCommand.Execute(ammoBox, _direction);
                _ammoList.Remove(ammoBox);
                yield return _delay;
            }

            _addStack = null;
        }

        public void PlayerNotInteractAmmoArea()
        {
            _isInteractPlayer = false;
        }
    }
}