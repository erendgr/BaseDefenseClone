using System;
using Enums;

namespace Datas.ValueObjects
{
    [Serializable]
    public class OutsideData
    {
        public OutSideStateLevels levels;
        public PayTypeEnum PayType;
        public int Cost;
    }
}