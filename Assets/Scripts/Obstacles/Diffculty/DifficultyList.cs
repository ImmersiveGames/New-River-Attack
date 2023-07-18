using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace RiverAttack
{
    [CreateAssetMenu(fileName = "ObstacleDifficultyList", menuName = "RiverAttack/DifficultList", order = 201)]
    public class DifficultyList : ScriptableObject
    {
        public List<EnemySetDifficulty> difficultiesList;

        public List<string> ListDifficultyByName()
        {
            var difficultNewList = difficultiesList.Select(x => x.name).Distinct();
            return difficultNewList.ToList();
        }
        public EnemySetDifficulty GetDifficult(int score)
        {
            return difficultiesList.Find(x => x.scoreToChange >= (score));
        }
    }

    [System.Serializable]
    public struct EnemySetDifficulty
    {
        public string name;
        public float scoreMod;
        public int scoreToChange;
        [Range(0, 10)]
        public float multiplySpeedy;
        [Range(0, 10)]
        public float multiplyPlayerDistance;
    }
}

