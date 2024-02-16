using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
namespace RiverAttack
{
    [CreateAssetMenu(fileName = "GamePlayingLog", menuName = "RiverAttack/GamePlayingLog", order = 2)]
    public class GamePlayingLog : SingletonScriptableObject<GamePlayingLog>
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
        public Levels activeMission;
        private List<Levels> finishLevels;
        /*
        public int lastMissionIndex;
        public int lastMissionFinishIndex;*/
        private List<LogResults> hitEnemiesResultsList;

        public int GetEnemiesHit(EnemiesScriptable enemy)
        {
            var item = hitEnemiesResultsList.Find(x => x.enemy == enemy);
            return item?.quantity ?? 0;
        }
        public void AddLevel(Levels level)
        {
            finishLevels.Add(level);
        }
        public void AddResultList(LogResults result)
        {
            hitEnemiesResultsList.Add(result);
        }

        public void ResultRecover(List<LogResults> resultsList)
        {
            hitEnemiesResultsList = new List<LogResults>();
            hitEnemiesResultsList = resultsList;
        }
        public void LevelRecover(List<Levels> levelsList)
        {
            finishLevels = new List<Levels>();
            finishLevels = levelsList;
        }

        public List<LogResults> GetEnemiesResult()
        {
            return hitEnemiesResultsList;
        }
        public List<Levels> GetLevelsResult()
        {
            return finishLevels;
        }
    }
    public enum CollisionType
    {
        Shoot, Bomb, Collider, Collected, None
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
            collisionType = collision;
        }
        public LogResults()
        {
            //throw new System.NotImplementedException();
        }
    }
}
