using System;
using ImmersiveGames.DebugManagers;
using NewRiverAttack.GameStatisticsSystem;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace ImmersiveGames.SteamServicesManagers
{
    public class SteamServerUpdater : MonoBehaviour
    {
        private SteamConnectionManager _steamConnectionManager;
        private SteamAchievementService _steamAchievementService;
        private SteamStatsService _steamStatsService;

        private GameStatisticManager _gameStatisticManager;

        #region Unity Methods

        private void Start()
        {
            SetInitialReferences();
            _gameStatisticManager.EventLogScore += SetHighScore;
        }

        private void OnApplicationQuit()
        {
            
        }

        private void OnDisable()
        {
            _gameStatisticManager.EventLogScore -= SetHighScore;
        }

        #endregion

        #region Update States
        private void SetHighScore(int intValue)
        {
            DebugManager.Log<SteamServerUpdater>($"Set Stat 'highScore': {intValue}");
            _steamStatsService.SetStat("highScore",intValue);
        }

        private void SetDistance()
        {
            
        }
        #endregion
        
        
        private void SetInitialReferences()
        {
            _steamConnectionManager = SteamConnectionManager.Instance;
            _steamAchievementService = SteamAchievementService.Instance;
            _steamStatsService = SteamStatsService.Instance;
            _gameStatisticManager = GameStatisticManager.instance;
        }
    }
}