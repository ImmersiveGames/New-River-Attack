namespace ImmersiveGames.SteamServicesManagers.Interface
{
    public interface IAchievementService
    {
        void UnlockAchievement(string achievementId, bool notify = true);
        bool IsAchievementUnlocked(string achievementId);
        void LoadAchievements();
        void SyncAchievements();
        void ResetAllAchievements();
    }
}