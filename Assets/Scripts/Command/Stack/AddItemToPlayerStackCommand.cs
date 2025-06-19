using System.Collections.Generic;
using Datas.ValueObjects;
using Datas.ValueObjects.Player;
using DG.Tweening;
using UnityEngine;

namespace Command.Stack
{
    public class AddItemToPlayerStackCommand
    {
        #region Self Variables

        #region Private Variables

        private List<GameObject> _stackList;
        private GameObject _stackHolder;
        private StackData _data;

        #endregion

        #endregion

        public AddItemToPlayerStackCommand(ref List<GameObject> stackList, ref GameObject stackHolder,
            ref StackData stackData)
        {
            _stackList = stackList;
            _stackHolder = stackHolder;
            _data = stackData;
        }

        public void Execute(GameObject obj, Vector3 position, Transform ammoArea)
        {
            obj.transform.position = ammoArea.position;
            obj.transform.SetParent(_stackHolder.transform);
            obj.transform.DOLocalMove(position, 1f);
            obj.transform.DOLocalRotate(Vector3.zero, 0.5f);
            _stackList.Add(obj);
        }
    }
}