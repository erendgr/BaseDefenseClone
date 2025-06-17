using System;
using System.Collections;
using System.Collections.Generic;
using _ObjectPooling.Scripts.Enums;
using _ObjectPooling.Scripts.Signals;
using Command.Stack;
using Datas.UnityObjects;
using Datas.ValueObjects;
using Enums;
using UnityEngine;

namespace Managers
{
    public class PlayerStackManager : MonoBehaviour
    {
        private StackType _stackType;
        private StackData _ammoStackData;
        private List<GameObject> _stackList = new();
        private Vector3 _stackPosition;
        private AddItemOnStackCommand _addItemOnStackCommand;
        private GetItemStackPositionCommand _ammoStackPositionCommand;

        
        [SerializeField] private GameObject stackHolder;
        
        private void Awake()
        {
            _ammoStackData = Resources.Load<CD_Player>("Data/CD_Player").AmmoStackData;
            _ammoStackPositionCommand = new GetItemStackPositionCommand(ref _stackList, ref _ammoStackData, ref stackHolder);
            _addItemOnStackCommand = new AddItemOnStackCommand(ref _stackList, ref stackHolder, ref _ammoStackData);
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
                _stackPosition = _ammoStackPositionCommand.Execute(_stackPosition);
                var obj = PoolSignals.Instance.onDequeuePoolableGameObject(PoolType.AmmoBox);
                _addItemOnStackCommand.Execute(obj, _stackPosition);
                yield return delay;
            }
        }
    }
}