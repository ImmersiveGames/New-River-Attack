using ImmersiveGames.DebugManagers;
using NewRiverAttack.GameStatisticsSystem;
using UnityEngine;

namespace ImmersiveGames.SteamServicesManagers
{
    public class SteamServerUpdater : MonoBehaviour
    {
        private SteamStatsService _steamStatsService;
        private GameStatisticManager _gameStatisticManager;

        private void Start()
        {
            SetInitialReferences();

            if (_gameStatisticManager == null) return;
            _gameStatisticManager.EventServiceUpdateInt += UpdateInt;
            _gameStatisticManager.EventServiceUpdateFloat += UpdateFloat;
        }

        private void OnDisable()
        {
            if (_gameStatisticManager == null) return;
            _gameStatisticManager.EventServiceUpdateInt -= UpdateInt;
            _gameStatisticManager.EventServiceUpdateFloat -= UpdateFloat;
        }

        private void UpdateInt(string stateName, int intValue)
        {
            DebugManager.Log<SteamServerUpdater>($"Log Stat Online: '{stateName}': {intValue}");
            _steamStatsService?.SetStat(stateName, intValue);
        }

        private void UpdateFloat(string stateName, float floatValue)
        {
            DebugManager.Log<SteamServerUpdater>($"Log Stat Online: '{stateName}': {floatValue}");
            _steamStatsService?.SetStat(stateName, floatValue);
        }

        private void SetInitialReferences()
        {
            _steamStatsService = SteamStatsService.Instance;
            _gameStatisticManager = GameStatisticManager.instance;
        }
    }
}