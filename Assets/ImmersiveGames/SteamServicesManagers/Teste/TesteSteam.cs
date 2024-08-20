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

            _steamStatsService.GetStat<int>("TESTE_01_INT");
            _steamStatsService.GetStat<float>("TESTE_02_FLOAT");
            
            //_steamStatsService.SetStat("TESTE_02_FLOAT", 3.458f);
            //_steamStatsService.SetStat("TESTE_01_INT", 89);
        }
    }
}