namespace ImmersiveGames.SteamServicesManagers.Interface
{
    public interface IStatsService
    {
        void SetStat<T>(string statName, T statValue) where T : struct;
        void AddStat<T>(string statName, T value, float threshold, bool instant = true);
        T GetStat<T>(string statName) where T : struct;
        void SyncStates();
        void ResetAllStats();
    }
}