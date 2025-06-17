using System.Collections.Generic;
using Datas.ValueObjects;
using DG.Tweening;
using UnityEngine;

namespace Command.Stack
{
    public class AddItemOnStackCommand
    {
        #region Self Variables

        #region Private Variables

        private List<GameObject> _stackList;
        private GameObject _stackHolder;
        private StackData _data;

        #endregion

        #endregion

        public AddItemOnStackCommand(ref List<GameObject> stackList, ref GameObject stackHolder,
            ref StackData stackData)
        {
            _stackList = stackList;
            _stackHolder = stackHolder;
            _data = stackData;
        }

        public void Execute(GameObject obj, Vector3 position)
        {
            obj.transform.SetParent(_stackHolder.transform);
            obj.transform.DOLocalMove(position, 1f);
            obj.transform.DOLocalRotate(Vector3.zero, 0.5f);
            _stackList.Add(obj);
        }
    }
}