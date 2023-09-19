using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace RiverAttack
{
    [CreateAssetMenu(fileName = "ObstacleDifficultyListSO", menuName = "RiverAttack/Enemy Set Difficult List", order = 201)]
    public class EnemiesSetDifficultyListSo : ScriptableObject
    {
        public List<EnemiesSetDifficulty> enemiesSetDifficulties;

        public EnemiesSetDifficulty GetDifficultByScore(int score)
        {
            return enemiesSetDifficulties.Find(x => x.scoreToChange >= (score));
        }

        public EnemiesSetDifficulty GetDifficultByEnemyDifficult(EnemiesSetDifficulty.EnemyDifficult difficultName)
        {
            return enemiesSetDifficulties.Find(x => x.enemyDifficult == difficultName);
        }
    }

    [System.Serializable]
    public struct EnemiesSetDifficulty
    {
        public enum EnemyDifficult { Easy, Normal, Hard }
        public EnemyDifficult enemyDifficult;
        public float multiplyScore;
        public int scoreToChange;
        [Range(0, 10)]
        public float multiplyEnemiesSpeedy;
        [Range(0, 10)]
        public float multiplyPlayerDistanceRadiusToShoot;
        [Range(0, 10)]
        public float multiplyPlayerDistanceRadiusToMove;
        [Range(0, 10)]
        public float multiplyEnemiesShootCadence;
        [Range(0, 10)]
        public float multiplyEnemiesShootSpeedy;
    }
}
