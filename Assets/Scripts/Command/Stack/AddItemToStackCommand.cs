using System.Collections.Generic;
using Datas.ValueObjects.Player;
using DG.Tweening;
using UnityEngine;

namespace Command.Stack
{
    public class AddItemToStackCommand
    {
        #region Self Variables

        #region Private Variables

        private List<GameObject> _stackList;
        private GameObject _stackHolder;

        #endregion

        #endregion

        public AddItemToStackCommand(ref List<GameObject> stackList, ref GameObject stackHolder)
        {
            _stackList = stackList;
            _stackHolder = stackHolder;
        }

        public void Execute(GameObject obj, Vector3 position, Transform ammoArea = null)
        {
            if (ammoArea != null) obj.transform.position = ammoArea.position;
            obj.transform.SetParent(_stackHolder.transform);
            obj.transform.DOLocalMove(position, 1f);
            obj.transform.DOLocalRotate(Vector3.zero, 0.5f);
            _stackList.Add(obj);
        }
    }
}