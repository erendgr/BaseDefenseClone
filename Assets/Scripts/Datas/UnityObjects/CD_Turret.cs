using Datas.ValueObjects;
using UnityEngine;

namespace Datas.UnityObjects
{
    [CreateAssetMenu(fileName = "CD_Turret", menuName = "BaseDef/CD_Turret", order = 0)]
    public class CD_Turret : ScriptableObject
    {
        public TurretData TurretData;
    }
}