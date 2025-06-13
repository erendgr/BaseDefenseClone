using System;
using Enums;

namespace Datas.ValueObjects
{
    [Serializable]
    public class MoneyWorkerBuyData
    {
        public PayTypeEnum PayType;
        public int Cost;
    }
}