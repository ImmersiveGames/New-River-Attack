using System.Collections.Generic;
using ImmersiveGames.BulletsManagers;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.Utils;
using NewRiverAttack.GamePlayManagers.GamePlayLogs;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.WallsManagers;
using UnityEngine;

namespace NewRiverAttack.GameStatisticsSystem
{
    public sealed class GameStatisticManager : Singleton<GameStatisticManager>
    {
        private float _startTimer;
        
        
        private GemeStatisticsDataLog _gemeStatisticsDataLog;

        public delegate void GameLogIntHandler(string stateName, int intValue);
        public event GameLogIntHandler EventServiceUpdateInt;

        public delegate void GameLogFloatHandler(string stateName, float floatValue);
        public event GameLogFloatHandler EventServiceUpdateFloat;

        protected override void Awake()
        {
            _startTimer = Time.time;
            base.Awake();
        }

        private void OnEnable()
        {
            _gemeStatisticsDataLog = GemeStatisticsDataLog.instance;
        }

        private void OnApplicationQuit()
        {
            LogSessionTime(_startTimer);
        }

        private void LogSessionTime(float sessionStartTime)
        {
            var sessionTime = Time.time - sessionStartTime;
            _gemeStatisticsDataLog.playerTimeSpent += sessionTime;
            DebugManager.Log<GameStatisticManager>($"Log Offline Timer {_gemeStatisticsDataLog.playerTimeSpent}");
        }

        internal void LogMaxScore(int score)
        {
            if (score > _gemeStatisticsDataLog.playerMaxScore)
            {
                _gemeStatisticsDataLog.playerMaxScore = score;
                DebugManager.Log<GameStatisticManager>($"Log Offline Max Score {_gemeStatisticsDataLog.playerMaxScore}");
            }
        }

        public void LogAmountDistance(float amount)
        {
            _gemeStatisticsDataLog.SetAmountDistance(amount);
        }

        internal void LogDistance(int distance)
        {
            LogMaxDistance(distance);
            if (_gemeStatisticsDataLog.amountDistance % 500 == 0)
            {
                OnEventServiceSet("stat_FlightDistance", _gemeStatisticsDataLog.amountDistance);
            }
        }

        private void LogMaxDistance(int intValue)
        {
            _gemeStatisticsDataLog.playerMaxDistance = Mathf.Max(_gemeStatisticsDataLog.playerMaxDistance, intValue);
            DebugManager.Log<GameStatisticManager>($"Log Offline MAX Distance: {_gemeStatisticsDataLog.playerMaxDistance}");
        }

        public void LogShoots(int intValue)
        {
            _gemeStatisticsDataLog.IncrementStat(ref _gemeStatisticsDataLog.playersShoots, intValue);
            if (_gemeStatisticsDataLog.playersShoots % 100 == 0)
            {
                OnEventServiceSet("stat_Shoots", _gemeStatisticsDataLog.playersShoots);
            }
        }

        public void LogBombs(int intValue)
        {
            _gemeStatisticsDataLog.IncrementStat(ref _gemeStatisticsDataLog.playersBombs, intValue);
            if (_gemeStatisticsDataLog.playersBombs % 100 == 0)
            {
                OnEventServiceSet("stat_Bombs", _gemeStatisticsDataLog.playersBombs);
            }
        }
        public void LogFuelSpend(float floatValue)
        {
            _gemeStatisticsDataLog.IncrementStat(ref _gemeStatisticsDataLog.fuelSpent, floatValue);
            if (_gemeStatisticsDataLog.fuelSpent % 1000 == 0)
            {
                OnEventServiceSet("stat_SpendGas", _gemeStatisticsDataLog.fuelSpent);
            }
        }
        public void LogFuelCharge(float floatValue)
        {
            _gemeStatisticsDataLog.IncrementStat(ref _gemeStatisticsDataLog.fuelCharge, floatValue);
            if (_gemeStatisticsDataLog.fuelCharge % 100 == 0)
            {
                OnEventServiceSet("stat_FillGas", _gemeStatisticsDataLog.fuelCharge);
            }
        }
        public void LogDeaths(int intValue)
        {
            _gemeStatisticsDataLog.IncrementStat(ref _gemeStatisticsDataLog.playerDeaths, intValue);
            OnEventServiceSet("stat_Deaths", _gemeStatisticsDataLog.playerDeaths);
        }

        public void LogCollision(Component other)
        {
            // Testa se o componente é do tipo WallMaster
            if (other.GetComponentInParent<WallMaster>())
            {
                _gemeStatisticsDataLog.IncrementStat(ref _gemeStatisticsDataLog.playersDieWall, 1);
                OnEventServiceSet("stat_WallDeaths", _gemeStatisticsDataLog.playersDieWall);
            }
            // Testa se o componente é do tipo ObstacleMaster
            else if (other.GetComponentInParent<ObstacleMaster>())
            {
                _gemeStatisticsDataLog.IncrementStat(ref _gemeStatisticsDataLog.playersDieEnemyCollider, 1);
                OnEventServiceSet("stat_ObstacleDeaths", _gemeStatisticsDataLog.playersDieEnemyCollider);
            }
            // Testa se o componente é do tipo Bullets
            else if (other.GetComponentInParent<Bullets>())
            {
                _gemeStatisticsDataLog.IncrementStat(ref _gemeStatisticsDataLog.playersDieEnemyBullets, 1);
                OnEventServiceSet("stat_BulletsDeaths", _gemeStatisticsDataLog.playersDieEnemyBullets);
            }
        }
        
        public void LogFuelOut(int intValue)
        {
            _gemeStatisticsDataLog.IncrementStat(ref _gemeStatisticsDataLog.playersDieFuelOut, intValue);
            OnEventServiceSet("stat_FuelOut", _gemeStatisticsDataLog.playersDieFuelOut);
        }

        internal void OnEventServiceUpdate()
        {
            DebugManager.Log<GameStatisticManager>($"Service Update");
            OnEventServiceSet("highScore", _gemeStatisticsDataLog.playerMaxScore);
            OnEventServiceSet("stat_FlightDistance", _gemeStatisticsDataLog.amountDistance);
            OnEventServiceSet("stat_Shoots", _gemeStatisticsDataLog.playersShoots);
            OnEventServiceSet("stat_Bombs", _gemeStatisticsDataLog.playersBombs);
            OnEventServiceSet("stat_SpendGas", _gemeStatisticsDataLog.fuelSpent);
            OnEventServiceSet("stat_FillGas", _gemeStatisticsDataLog.fuelCharge);
        }

        private void OnEventServiceSet(string stateName, int intValue)
        {
            EventServiceUpdateInt?.Invoke(stateName, intValue);
        }

        private void OnEventServiceSet(string stateName, float floatValue)
        {
            EventServiceUpdateFloat?.Invoke(stateName, floatValue);
        }
    }
}
