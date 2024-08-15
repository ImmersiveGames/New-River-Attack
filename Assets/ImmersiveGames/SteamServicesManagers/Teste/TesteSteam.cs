using ImmersiveGames.DebugManagers;
using UnityEngine;

namespace ImmersiveGames.SteamServicesManagers.Teste
{
    public class TesteSteam : MonoBehaviour
    {
        private SteamConnectionManager _steamConnectionManager;
        private SteamAchievementService _steamAchievementService;
        private SteamStatsService _steamStatsService;
        private void Start()
        {
            _steamConnectionManager = SteamConnectionManager.Instance;
            _steamAchievementService = SteamAchievementService.Instance;
            _steamStatsService = SteamStatsService.Instance;
            
            Invoke(nameof(AddTesteAchievement), 2f);
        }

        private void AddTesteAchievement()
        {
            //_steamAchievementService.UnlockAchievement("ACH_BUY_SKIN");
            //_steamAchievementService.UnlockAchievement("ACH_XXXXXXXXX");
            //_steamAchievementService.ResetAllAchievements();
        }
    }
}