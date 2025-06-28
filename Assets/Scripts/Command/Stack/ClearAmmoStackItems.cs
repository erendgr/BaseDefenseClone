using System.Collections.Generic;
using _ObjectPooling.Scripts.Enums;
using _ObjectPooling.Scripts.Signals;
using DG.Tweening;
using UnityEngine;

namespace Command.Stack
{
    public class ClearAmmoStackItems
    {
        #region Self Variables

        #region Private Variables

        private List<GameObject> _stackList;
        private Transform _warHouse;

        #endregion

        #endregion

        public ClearAmmoStackItems(ref List<GameObject> stackList, Transform warHouse)
        {
            _stackList = stackList;
            _warHouse = warHouse;
        }

        public void Execute()
        {
            for (int i = 0; i < _stackList.Count; i++)
            {
                var ammoBox = _stackList[i];
                ammoBox.transform.SetParent(_warHouse);
                var localPosition = ammoBox.transform.localPosition;
                ammoBox.transform.DOLocalMove(new Vector3(localPosition.x + Random.Range(-1f, 1f),
                    0.8f, localPosition.z + Random.Range(-1f, 1f)), 0.5f);
                ammoBox.transform.DOLocalMove(Vector3.zero, 0.5f).SetDelay(0.5f).OnComplete(() =>
                {
                    PoolSignals.Instance.onEnqueuePooledGameObject(ammoBox, PoolType.AmmoBox);
                });
            }

            _stackList.Clear();
        }
    }
}