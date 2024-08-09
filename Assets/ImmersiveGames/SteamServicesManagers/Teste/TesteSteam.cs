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
            
            Invoke(nameof(AddTesteAchievement), 5f);
        }

        private void AddTesteAchievement()
        {
            //_steamAchievementService.UnlockAchievement("ACH_BUY_SKIN");
            //_steamAchievementService.ResetAllAchievements();
        }
    }
}