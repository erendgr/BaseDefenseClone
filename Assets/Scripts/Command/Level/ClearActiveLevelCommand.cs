using UnityEngine;

namespace Command.Level
{
    public class ClearActiveLevelCommand
    {
        public void ClearActiveLevel(Transform levelHolder)
        {
            foreach (Transform child in levelHolder)
            {
                Object.Destroy(child.gameObject);
            }
        }
    }
}