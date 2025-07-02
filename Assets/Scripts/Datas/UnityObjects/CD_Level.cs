using System.Collections.Generic;
using Datas.ValueObjects;
using Datas.ValueObjects.Level;
using UnityEngine;

namespace Datas.UnityObjects
{
    [CreateAssetMenu(fileName = "CD_Level", menuName = "BaseDef/CD_Level", order = 0)]
    public class CD_Level : ScriptableObject
    {
        public List<LevelData> LevelDatas;
    }
}