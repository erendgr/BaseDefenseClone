using System.Collections.Generic;
using Datas.ValueObjects.Level;
using UnityEngine;

namespace Command.Stack
{
    public class GetStaticStackItemPosCommand
    {
        #region Self Variables

        #region Private Variables

        private List<GameObject> _stackList;
        private GameObject _stackHolder;
        private StaticStackData _data;
        private int _stackSize;

        #endregion

        #endregion

        public GetStaticStackItemPosCommand(ref List<GameObject> stackList, ref StaticStackData data, 
            ref GameObject stackHolder)
        {
            _stackList = stackList;
            _data = data;
            _stackHolder = stackHolder;
            _stackSize = _data.StackCountX * _data.StackCountZ;
        }

        public Vector3 Execute(Vector3 position)
        {
            position = _stackHolder.transform.localPosition + _data.InitPosition;
            position.x += _stackList.Count % _data.StackCountX / _data.StackOffset.x;
            position.y += _stackList.Count / _stackSize / _data.StackOffset.y;
            position.z -= _stackList.Count / _data.StackCountX % _data.StackCountZ / _data.StackOffset.z;
            return position;
        }
    }
}