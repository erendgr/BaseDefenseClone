using System.Collections.Generic;
using Datas.ValueObjects.Level;
using DG.Tweening;
using UnityEngine;

namespace Command.Stack
{
    public class AddItemToStaticStackCommand
    {
        #region Self Variables

        #region Private Variables

        private List<GameObject> _stackList;
        private GameObject _stackHolder;
        private StaticStackData _data;

        #endregion

        #endregion

        public AddItemToStaticStackCommand(ref List<GameObject> stackList, ref StaticStackData stackData,
            ref GameObject stackHolder)
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