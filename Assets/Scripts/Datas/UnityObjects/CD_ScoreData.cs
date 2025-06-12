using Datas.ValueObjects;
using UnityEngine;

namespace Datas.UnityObjects
{
    [CreateAssetMenu(fileName = "CD_ScoreData", menuName = "BaseDef/CD_ScoreData", order = 0)]
    public class CD_ScoreData : ScriptableObject
    {
        public ScoreData ScoreData;
    }
}