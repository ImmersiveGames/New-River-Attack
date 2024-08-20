using System;
using System.Collections.Generic;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.Utils;
using NewRiverAttack.LogManagers;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;
using UnityEngine;

namespace NewRiverAttack.GamePlayManagers.GamePlayLogs
{
    [CreateAssetMenu(fileName = "GemeStatisticsDataLog", menuName = "ImmersiveGames/GemeStatisticsDataLog", order = 2)]
    public class GemeStatisticsDataLog : SingletonScriptable<GemeStatisticsDataLog>
    {
        private GemeStatisticsDataLog()
        {
            SetResourcePath("SavesSO/GemeStatisticsDataLog");
        }
        [Header("Players Logs")] 
        public int playerMaxScore;
        public float playerTimeSpent;
        public float playerMaxDistance;
        public float playerTotalDistance;
        [Header("Players Archive Stats")] public int playersDieWall;
        public int playersDieEnemyBullets;
        public int playerDieFuelEmpty;
        public int playersShoots;
        public int playersBombs;
        public int fuelSpent;
        public int amountDistance;

        #region LogFunctions
        public void LogMaxScore(int score)
        {
            if (score > playerMaxScore)
            {
                playerMaxScore = score;
            }
        }
        

        #endregion


        private List<LogPlayerResult> _listEnemyHit = new List<LogPlayerResult>();

        public List<LogPlayerResult> GetEnemiesResult()
        {
            return _listEnemyHit;
        }

        public int GetEnemiesHit(ObjectsScriptable enemy)
        {
            var item = _listEnemyHit.Find(x => x.enemy == enemy);
            DebugManager.Log<GemeStatisticsDataLog>($"Foram Encontrados {item?.quantity ?? 0} em {enemy.name}");
            return item?.quantity ?? 0;
        }

        public void RecoverLogPlayerResult(List<LogPlayerResult> resultsList)
        {
            _listEnemyHit = new List<LogPlayerResult>();
            _listEnemyHit = resultsList;
        }

        public void ResetLogs()
        {
            playerMaxScore = 0;
            playerTimeSpent = 0f;
            
            playerMaxDistance = 0;
            playerTotalDistance = 0;

            playersDieWall = 0;
            playersDieEnemyBullets = 0;
            playerDieFuelEmpty = 0;
            playersShoots = 0;
            playersBombs = 0;
            fuelSpent = 0;
            amountDistance = 0;
        }

        

        

        public void LogMaxDistance(float distance)
        {
            playerMaxDistance = Mathf.Max(playerMaxDistance, distance);
        }
        public void LogTotalDistance(float distance)
        {
            playerTotalDistance += distance;
        }

        //public float playersMaxDistance;
    }
}