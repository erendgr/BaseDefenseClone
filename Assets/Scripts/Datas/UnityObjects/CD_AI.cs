using Datas.ValueObjects.AI;
using UnityEngine;

namespace Datas.UnityObjects
{
    [CreateAssetMenu(fileName = "CD_AI", menuName = "BaseDef/CD_AI", order = 0)]
    public class CD_AI : ScriptableObject
    {
        public EnemyAIData EnemyAIData;
        public AmmoWorkerAIData AmmoWorkerAIData;
        public MoneyWorkerData MoneyWorkerData;
        public SoldierAIData SoldierAIData;
    }
}