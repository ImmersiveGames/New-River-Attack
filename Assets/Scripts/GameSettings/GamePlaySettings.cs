using System.Collections.Generic;
using UnityEngine;
namespace RiverAttack
{

    [CreateAssetMenu(fileName = "GamePlaySettings", menuName = "RiverAttack/GamePlaySettings", order = 2)]
    public class GamePlaySettings : SingletonScriptableObject<GamePlaySettings>
    {
        [SerializeField]
        public int pathDistance;
        [SerializeField]
        public float shootSpent;
        [SerializeField]
        public int livesSpent;
        [SerializeField]
        public int fuelSpent;
        [SerializeField]
        public int fuelStocked;
        [SerializeField]
        public int bombSpent;
        [SerializeField]
        public int totalScore;
        [SerializeField]
        public float timeSpent;
        [SerializeField]
        public int playerDieWall;
        [SerializeField]
        public int playerDieFuelEmpty;
        [SerializeField]
        public List<EnemiesResults> hitEnemiesResultsList;

        public int GetEnemiesHit(EnemiesScriptable enemy)
        {
            var item = hitEnemiesResultsList.Find(x => x.enemy == enemy);
            return item?.quantity ?? 0;
        }
    }
}
