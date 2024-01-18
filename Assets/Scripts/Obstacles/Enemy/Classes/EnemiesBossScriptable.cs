using UnityEngine;
namespace RiverAttack
{
    [CreateAssetMenu(fileName = "Boss", menuName = "RiverAttack/Enemy/Boss", order = 1)]
    [System.Serializable]
    public class EnemiesBossScriptable: EnemiesScriptable
    {
        [Header("Boss Settings")]
        public int maxHp;
        public int maxCycles;
    }
}
