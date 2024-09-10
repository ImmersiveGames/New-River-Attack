using System;
using ImmersiveGames.BulletsManagers;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.SteamServicesManagers;
using ImmersiveGames.Utils;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.GamePlayManagers.GamePlayLogs;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;
using NewRiverAttack.PlayerManagers.ScriptableObjects;
using NewRiverAttack.WallsManagers;
using UnityEngine;

namespace NewRiverAttack.GameStatisticsSystem
{
    public sealed class GameStatisticManager : Singleton<GameStatisticManager>
    {
        private float _startTimer;
        private const float Epsilon = 0.1f;
        
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
            if (!_gemeStatisticsDataLog) return;
            _gemeStatisticsDataLog.playersTimeSpent += sessionTime;
            DebugManager.Log<GameStatisticManager>($"Log Offline Timer {_gemeStatisticsDataLog.playersTimeSpent}");
        }

        internal async void LogMaxScore(int score)
        {
            if(_gemeStatisticsDataLog == null) return;
            if (score <= _gemeStatisticsDataLog.playersMaxScore) return;
            _gemeStatisticsDataLog.playersMaxScore = score;
            await SteamLeaderboardService.Instance.UpdateScore(score, true).ConfigureAwait(false);
            DebugManager.Log<GameStatisticManager>($"Log Offline Max Score {_gemeStatisticsDataLog.playersMaxScore}");
        }

        public void LogAmountDistance(float amount)
        {
            if(_gemeStatisticsDataLog == null) return;
            _gemeStatisticsDataLog.SetAmountDistance(amount);
        }

        internal void LogDistance(int distance)
        {
            LogMaxDistance(distance);
            if(_gemeStatisticsDataLog == null) return;
            if (_gemeStatisticsDataLog.playersAmountDistance % 500 == 0)
            {
                OnEventServiceSet("stat_FlightDistance", _gemeStatisticsDataLog.playersAmountDistance);
            }
        }

        private void LogMaxDistance(int intValue)
        {
            if(_gemeStatisticsDataLog == null) return;
            _gemeStatisticsDataLog.playersMaxDistance = Mathf.Max(_gemeStatisticsDataLog.playersMaxDistance, intValue);
            DebugManager.Log<GameStatisticManager>($"Log Offline MAX Distance: {_gemeStatisticsDataLog.playersMaxDistance}");
        }

        public void LogShoots(int intValue)
        {
            if(_gemeStatisticsDataLog == null) return;
            _gemeStatisticsDataLog.IncrementStat(ref _gemeStatisticsDataLog.playersShoots, intValue);
            if (_gemeStatisticsDataLog.playersShoots % 100 == 0)
            {
                OnEventServiceSet("stat_Shoots", _gemeStatisticsDataLog.playersShoots);
            }
        }

        public void LogBombs(int intValue)
        {
            if(_gemeStatisticsDataLog == null) return;
            _gemeStatisticsDataLog.IncrementStat(ref _gemeStatisticsDataLog.playersBombs, intValue);
            if (_gemeStatisticsDataLog.playersBombs % 100 == 0)
            {
                OnEventServiceSet("stat_Bombs", _gemeStatisticsDataLog.playersBombs);
            }
        }
        public void LogFuelSpend(float floatValue)
        {
            if(_gemeStatisticsDataLog == null) return;
            _gemeStatisticsDataLog.IncrementStat(ref _gemeStatisticsDataLog.playersFuelSpent, floatValue);
            if (Mathf.Abs(_gemeStatisticsDataLog.playersFuelSpent % 1000) < Epsilon || Mathf.Abs(1000 - (_gemeStatisticsDataLog.playersFuelSpent % 1000)) < Epsilon)
            {
                OnEventServiceSet("stat_SpendGas", _gemeStatisticsDataLog.playersFuelSpent);
            }
        }
        public void LogFuelCharge(float floatValue)
        {
            if(_gemeStatisticsDataLog == null) return;
            _gemeStatisticsDataLog.IncrementStat(ref _gemeStatisticsDataLog.playersFuelCharge, floatValue);
            if (Mathf.Abs(_gemeStatisticsDataLog.playersFuelCharge % 100) < Epsilon || Mathf.Abs(100 - (_gemeStatisticsDataLog.playersFuelCharge % 100)) < Epsilon)
            {
                OnEventServiceSet("stat_FillGas", _gemeStatisticsDataLog.playersFuelCharge);
            }
        }
        public void LogDeaths(int intValue)
        {
            if(_gemeStatisticsDataLog == null) return;
            _gemeStatisticsDataLog.IncrementStat(ref _gemeStatisticsDataLog.playersDeaths, intValue);
            OnEventServiceSet("stat_Deaths", _gemeStatisticsDataLog.playersDeaths);
        }

        public void LogCollision(Component other)
        {
            if(_gemeStatisticsDataLog == null) return;
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
            if(_gemeStatisticsDataLog == null) return;
            _gemeStatisticsDataLog.IncrementStat(ref _gemeStatisticsDataLog.playersDieFuelOut, intValue);
            OnEventServiceSet("stat_FuelOut", _gemeStatisticsDataLog.playersDieFuelOut);
        }
        public void LogCompletePath(int intValue, GamePlayModes modes)
        {
            switch (modes)
            {
                case GamePlayModes.ClassicMode:
                    _gemeStatisticsDataLog.IncrementStat(ref _gemeStatisticsDataLog.playersClassicPath, intValue);
                    OnEventServiceSet("stat_FinishCPath", _gemeStatisticsDataLog.playersClassicPath);
                    break;
                case GamePlayModes.MissionMode:
                    _gemeStatisticsDataLog.IncrementStat(ref _gemeStatisticsDataLog.playersMissionPath, intValue);
                    OnEventServiceSet("stat_FinishMPath", _gemeStatisticsDataLog.playersMissionPath);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(modes), modes, null);
            }
        }
        public void LogTimeRapidFire(float floatValue)
        {
            if(_gemeStatisticsDataLog == null) return;
            _gemeStatisticsDataLog.IncrementStat(ref _gemeStatisticsDataLog.playersTimeRapidFire, floatValue);
            if (Mathf.Abs(_gemeStatisticsDataLog.playersTimeRapidFire % 100) < Epsilon || Mathf.Abs(100 - (_gemeStatisticsDataLog.playersTimeRapidFire % 100)) < Epsilon)
            {
                OnEventServiceSet("stat_TimeRapidFire", _gemeStatisticsDataLog.playersTimeRapidFire);
            }
        }
        
        public void LogBombsHit(int intValue)
        {
            if(_gemeStatisticsDataLog == null) return;
            _gemeStatisticsDataLog.playersBombHit = Mathf.Max(_gemeStatisticsDataLog.playersBombHit, intValue);
            OnEventServiceSet("stat_BombHit", _gemeStatisticsDataLog.playersBombHit);
        }

        public void LogEnemiesHit(PlayerSettings player, ObjectsScriptable enemy, int quantity, EnumCollisionType collision)
        {
            if(_gemeStatisticsDataLog == null) return;
            _gemeStatisticsDataLog.AddOrUpdateStatisticHit(player,enemy, quantity,collision);
        }

        internal void OnEventServiceUpdate()
        {
            if(_gemeStatisticsDataLog == null) return;
            DebugManager.Log<GameStatisticManager>($"Service Update");
            OnEventServiceSet("highScore", _gemeStatisticsDataLog.playersMaxScore);
            OnEventServiceSet("stat_FlightDistance", _gemeStatisticsDataLog.playersAmountDistance);
            OnEventServiceSet("stat_Shoots", _gemeStatisticsDataLog.playersShoots);
            OnEventServiceSet("stat_Bombs", _gemeStatisticsDataLog.playersBombs);
            OnEventServiceSet("stat_SpendGas", _gemeStatisticsDataLog.playersFuelSpent);
            OnEventServiceSet("stat_FillGas", _gemeStatisticsDataLog.playersFuelCharge);
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
