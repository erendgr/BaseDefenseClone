using Datas.ValueObjects;
using UnityEngine;

namespace Datas.UnityObjects
{
    [CreateAssetMenu(fileName = "CD_Score", menuName = "BaseDef/CD_Score", order = 0)]
    public class CD_Score : ScriptableObject
    {
        public ScoreData ScoreData;
    }
}