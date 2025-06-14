using Datas.ValueObjects;
using Datas.ValueObjects.Player;
using UnityEngine;

namespace Datas.UnityObjects
{
    [CreateAssetMenu(fileName = "CD_Player", menuName = "BaseDef/CD_Player", order = 0)]
    public class CD_Player : ScriptableObject
    {
        public PlayerData Data;
        public StackData MoneyStackData;
        public StackData AmmoStackData;
    }
}