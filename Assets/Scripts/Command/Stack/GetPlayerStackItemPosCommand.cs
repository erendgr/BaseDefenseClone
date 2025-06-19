using System.Collections.Generic;
using Datas.ValueObjects.Player;
using UnityEngine;

namespace Command.Stack
{
    public class GetPlayerStackItemPosCommand
    {
        #region Self Variables

        #region Private Variables

        private List<GameObject> _stackList;
        private GameObject _stackHolder;
        private StackData _data;
        private int _stackSize;

        #endregion

        #endregion

        public GetPlayerStackItemPosCommand(ref List<GameObject> stackList, ref StackData data,
            ref GameObject stackHolder)
        {
            _stackList = stackList;
            _data = data;
            _stackHolder = stackHolder;
            _stackSize = _data.StackCountX * _data.StackCountY;
        }

        public Vector3 Execute(Vector3 position)
        {
            position = _stackHolder.transform.localPosition + _data.InitPosition;
            position.x += _stackList.Count % _data.StackCountX / _data.StackOffset.x;
            position.y += (int)(_stackList.Count / _data.StackCountX) % _data.StackCountY / _data.StackOffset.y;
            position.z -= (int)(_stackList.Count / _stackSize) / _data.StackOffset.z;
            return position;
        }
    }
}