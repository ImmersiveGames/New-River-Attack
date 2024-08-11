namespace ImmersiveGames.SteamServicesManagers.Interface
{
    public interface IAchievementService
    {
        void UnlockAchievement(string achievementId);
        bool IsAchievementUnlocked(string achievementId);
        void LoadAchievements();
        void SyncAchievements();
        void ResetAllAchievements();
    }
}