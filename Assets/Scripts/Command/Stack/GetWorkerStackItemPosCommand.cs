using System.Collections.Generic;
using Datas.ValueObjects.Player;
using UnityEngine;

namespace Command.Stack
{
    public class GetWorkerStackItemPosCommand
    {
        #region Self Variables

        #region Private Variables
        
        private List<GameObject> _stackList;
        private GameObject _stackHolder;
        private StackData _data;
        private int _stackSize;

        #endregion
        #endregion

        public GetWorkerStackItemPosCommand(ref List<GameObject> stackList,ref StackData stackData, ref GameObject stackHolder)
        {
            _stackList = stackList;
            _data = stackData;
            _stackHolder = stackHolder;
            _stackSize = _data.StackCountX * _data.StackCountY;
        }

        public Vector3 Execute(Vector3 stackPosition)
        {
            stackPosition = _stackHolder.transform.localPosition + _data.InitPosition;
            stackPosition.x += _stackList.Count % _data.StackCountX / _data.StackOffset.x;
            stackPosition.y += (int)(_stackList.Count / _data.StackCountX)  % _data.StackCountY / _data.StackOffset.y;
            stackPosition.z -= (int)(_stackList.Count / _stackSize) / _data.StackOffset.z;
            return stackPosition;
        }
    }
}