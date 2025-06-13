using System.Collections.Generic;
using Enums;

namespace Keys
{
    public struct AreaDataParams
    {
        public Dictionary<RoomNameEnum, int> RoomPayedAmount;
        public Dictionary<TurretNameEnum, int> RoomTurretPayedAmount;
    }
}