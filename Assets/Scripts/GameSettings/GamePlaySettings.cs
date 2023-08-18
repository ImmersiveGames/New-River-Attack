using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace RiverAttack
{
    [CreateAssetMenu(fileName = "GamePlaySettings", menuName = "RiverAttack/GamePlaySettings", order = 2)]
    public class GamePlaySettings : SingletonScriptableObject<GamePlaySettings>
    {
        public float pathDistance;
        public float maxPathDistance;
        public float shootSpent;
        public int livesSpent;
        public int fuelSpent;
        public int fuelStocked;
        public int bombSpent;
        public int totalScore;
        public float timeSpent;
        public int playerDieWall;
        public int playerDieBullet;
        public int playerDieFuelEmpty;
        public List<LogResults> hitEnemiesResultsList;

        public int GetEnemiesHit(EnemiesScriptable enemy)
        {
            var item = hitEnemiesResultsList.Find(x => x.enemy == enemy);
            return item?.quantity ?? 0;
        }
    }
    public enum CollisionType
    {
        Shoot, Bomb, Collider, Collected
    }
    [System.Serializable]
    public class LogResults
    {
        public PlayerSettings player;
        public EnemiesScriptable enemy;
        public int quantity;
        public CollisionType collisionType;

        public LogResults(PlayerSettings player, EnemiesScriptable enemy, int quantity, CollisionType collision)
        {
            this.player = player;
            this.enemy = enemy;
            this.quantity = quantity;
            this.collisionType = collision;
        }
    }
}
