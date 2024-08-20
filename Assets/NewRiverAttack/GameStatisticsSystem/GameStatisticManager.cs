using System;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.Utils;
using NewRiverAttack.GamePlayManagers.GamePlayLogs;
using UnityEngine;
using UnityEngine.Serialization;

namespace NewRiverAttack.GameStatisticsSystem
{
    public sealed class GameStatisticManager: Singleton<GameStatisticManager>
    {
        private float _startTimer;
        private GemeStatisticsDataLog _gemeStatisticsDataLog;
        
        #region Delagates
        public delegate void GameLogIntHandler(int intValue);
        public event GameLogIntHandler EventLogScore;
        public event GameLogIntHandler EventServiceScore;
        public delegate void GameLogFloatHandler(float floatValue);
        public event GameLogFloatHandler EventLogTimer;
        
        #endregion

        #region Unity Methods
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

        #endregion

        #region Logs

        private void LogSessionTime(float sessionStartTime)
        {
            var sessionTime = Time.time - sessionStartTime;
            _gemeStatisticsDataLog.playerTimeSpent += sessionTime;
        }

        private void LogMaxScore(int score)
        {
            if (score > _gemeStatisticsDataLog.playerMaxScore)
            {
                _gemeStatisticsDataLog.playerMaxScore = score;
            }
        }

        #endregion


        internal void OnEventLogScore(int intValue)
        {
            LogMaxScore(intValue);
            EventLogScore?.Invoke(_gemeStatisticsDataLog.playerMaxScore);
            DebugManager.Log<GameStatisticManager>($"Log Offline Score: {_gemeStatisticsDataLog.playerMaxScore}");
        }
        internal void OnEventServiceScore(int intValue)
        {
            OnEventLogScore(intValue);
            DebugManager.Log<GameStatisticManager>($"Log Online Score: {_gemeStatisticsDataLog.playerMaxScore}");
            EventServiceScore?.Invoke(_gemeStatisticsDataLog.playerMaxScore);
        }

        internal void OnEventLogTimer(float floatValue)
        {
            LogSessionTime(floatValue);
            EventLogTimer?.Invoke(_gemeStatisticsDataLog.playerTimeSpent);
            DebugManager.Log<GameStatisticManager>($"Log Offline Timer: {_gemeStatisticsDataLog.playerTimeSpent}");
        }

        
    }
}