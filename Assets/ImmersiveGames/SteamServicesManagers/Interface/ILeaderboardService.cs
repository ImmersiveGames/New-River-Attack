using System.Threading.Tasks;
using Steamworks.Data;

namespace ImmersiveGames.SteamServicesManagers.Interface
{
    public interface ILeaderboardService
    {
        void UpdateScore(int score, bool force);
        Task<LeaderboardEntry[]> GetScores(int quantity);
        Task<LeaderboardEntry[]> GetScoresFromFriends();
        Task<LeaderboardEntry[]> GetScoresAround(int start, int end);
        void SyncOfflineScores();
        void SaveOfflineScores();
        void LoadOfflineScores();
    }
}