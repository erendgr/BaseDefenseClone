using AYellowpaper.SerializedCollections;
using Enums;
using UnityEngine;


namespace Datas.UnityObjects
{
    [CreateAssetMenu(fileName = "CD_Weapon", menuName = "BaseDef/CD_Weapon", order = 0)]
    public class CD_Weapon : ScriptableObject
    {
        public SerializedDictionary<WeaponType, WeaponData> Weapons;
    }
}