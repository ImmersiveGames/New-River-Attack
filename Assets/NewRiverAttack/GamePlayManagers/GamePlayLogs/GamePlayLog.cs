using System.Collections.Generic;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.Utils;
using NewRiverAttack.LogManagers;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;
using UnityEngine;

namespace NewRiverAttack.GamePlayManagers.GamePlayLogs
{
    [CreateAssetMenu(fileName = "GamePlayLog", menuName = "ImmersiveGames/GamePlayLog", order = 2)]
    public class GamePlayLog : SingletonScriptable<GamePlayLog>
    {
        private GamePlayLog()
        {
            SetResourcePath("SavesSO/GamePlayLog");
        }

        [Header("Players Archive Stats")] 
        public int playersDieWall;
        public int playersDieEnemyBullets;
        public int playerDieFuelEmpty;
        public int playersShoots;
        public int playersBombs;
        public int fuelSpent;
        public int amountDistance;

        private List<LogPlayerResult> _listEnemyHit = new List<LogPlayerResult>();
        public List<LogPlayerResult> GetEnemiesResult()
        {
            return _listEnemyHit;
        }
        public int GetEnemiesHit(ObjectsScriptable enemy)
        {
            var item = _listEnemyHit.Find(x => x.enemy == enemy);
            DebugManager.Log<GamePlayLog>($"Foram Encontrados {item?.quantity ?? 0} em {enemy.name}");
            return item?.quantity ?? 0;
        }
        public void RecoverLogPlayerResult(List<LogPlayerResult> resultsList)
        {
            _listEnemyHit = new List<LogPlayerResult>();
            _listEnemyHit = resultsList;
        }

        //public float playersMaxDistance;
    }
}