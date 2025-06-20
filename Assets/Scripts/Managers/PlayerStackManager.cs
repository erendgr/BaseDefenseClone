using System.Collections;
using System.Collections.Generic;
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
        private StackData _ammoStackData;
        private List<GameObject> _stackList = new();
        private AddItemToPlayerStackCommand _addItemToPlayerStackCommand;
        private GetPlayerStackItemPosCommand _ammoStackItemPosCommand;

        #endregion

        #endregion

        private void Awake()
        {
            _ammoStackData = Resources.Load<CD_Player>("Data/CD_Player").AmmoStackData;
            _ammoStackItemPosCommand =
                new GetPlayerStackItemPosCommand(ref _stackList, ref _ammoStackData, ref stackHolder);
            _addItemToPlayerStackCommand =
                new AddItemToPlayerStackCommand(ref _stackList, ref stackHolder, ref _ammoStackData);
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
                _addItemToPlayerStackCommand.Execute(obj, _stackPosition, ammoArea);
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
    }
}