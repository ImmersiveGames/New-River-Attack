using System.Collections;
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
        public int livesSpents;
        [SerializeField]
        public int fuelSpents;
        [SerializeField]
        public int bombSpents;
        [SerializeField]
        public int totalScore;
        [SerializeField]
        public float totalTime;
        [SerializeField]
        public List<EnemiesResults> HitEnemys;

        public int GetEnemysHit(EnemiesScriptable enemy)
        {
            EnemiesResults item = HitEnemys.Find(x => x.enemy == enemy);
            if (item != null)
            {
                return item.quantity;
            }
            else
                return 0;
        }
    }
}
