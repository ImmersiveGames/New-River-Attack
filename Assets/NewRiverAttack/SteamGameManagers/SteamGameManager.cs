using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImmersiveGames.DebugManagers;
using Steamworks;
using Steamworks.Data;
using UnityEngine;

namespace NewRiverAttack.SteamGameManagers
{
    public class SteamGameManager: MonoBehaviour
    {
        private static SteamGameManager _instance;
        private const int SteamID = 2777110;
        private const string LeaderboardName = "River_Attack_HiScore";
        private static IEnumerable<Achievement> _serverAchievements;
        public bool resetAchievementsOnStart;
        public bool demoMode;
        private bool _applicationHasQuit;

        private static Leaderboard? _leaderboard;

        public static bool ConnectedToSteam
        {
            get;
            private set;
        }
        #region UNITYMETHODS

        private async void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                try
                {
                    if (!demoMode)
                    {
                        SteamClient.Init( SteamID);
                        if (!SteamClient.IsValid)
                        {
                            DebugManager.Log<SteamGameManager>("Steam client not valid");
                            throw new Exception();
                        }
                        ConnectedToSteam = true;
                        _serverAchievements = SteamUserStats.Achievements;
                        _leaderboard = await SteamUserStats.FindLeaderboardAsync(LeaderboardName).ConfigureAwait(false);

                        DebugManager.Log<SteamGameManager>("Leaderboard initialized: " + _leaderboard);
                    }
                    else
                    {
                        ConnectedToSteam = false;
                        DebugManager.LogWarning<SteamGameManager>("Steam not online");
                    }
                    
                }
                catch ( Exception e )
                {
                    ConnectedToSteam = false;
                    DebugManager.Log<SteamGameManager>("Error connecting to Steam");
                    DebugManager.LogError<SteamGameManager>(e);
                }
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if (!SteamClient.IsValid) return;
            if (resetAchievementsOnStart)
            {
                ClearAllStats(true);
            }
            ReconcileMissedAchievements();
            SteamUserStats.OnAchievementProgress += AchievementChanged;
            /*foreach ( var a in SteamUserStats.Achievements )
            {
                DebugManager.Log<SteamGameManager>( $"{a.Name} ({a.State}) {a.Identifier}" );
            }*/
        }

        private void Update()
        {
            SteamClient.RunCallbacks();
        }

        private void OnDisable()
        {
            GameCleanup();
        }
        protected void OnDestroy()
        {
            //base.OnDestroy();
            GameCleanup();
        }

        private void OnApplicationQuit()
        {
            GameCleanup();
        }
  #endregion

  private static void ReconcileMissedAchievements()
        {
            // Aqui ele coloca os achievements offline para atualizar.
            
            /*AchievementStatus localAchievementStatus = SaveSystem.LoadAchievementStatus();
            var steamAchievementStatus = SteamUserStats.Achievements.ToList();
            var achievementsThatWereMissed = new List<string>();

            if (localAchievementStatus.achievementToCheck)
            {
                foreach (var achievement in steamAchievementStatus.Where(achievement => achievement.Name.Equals(achievementToCheck) && !achievement.State))
                {
                    achievementsThatWereMissed.Add(achievementToCheck);
                }
            }

            if (achievementsThatWereMissed.Count > 0)
            {
                UnlockAchievements(achievementsThatWereMissed);
            }*/
        }
        public static void UnlockAchievement(List<string> achievementsToUnlock)
        {
            if (!ConnectedToSteam) return;
            if (!SteamClient.IsValid || !SteamClient.IsLoggedOn ) return;
            try
            {
                foreach (var achievement in achievementsToUnlock)
                {
                    UnlockAchievement(achievement);
                }
            }
            catch
            {
                DebugManager.Log<SteamGameManager>("Unable to set unlocked achievement status on Steam");
            }
        }
        public static void UnlockAchievement(string archiveID)
        {
            if (!ConnectedToSteam) return;
            try
            {
                if (!_serverAchievements.Any(achievement => achievement.Identifier == archiveID && !achievement.State))
                    return;
                var ach = new Achievement(archiveID);
                ach.Trigger();
                DebugManager.Log<SteamGameManager>( $"{ach.Name} WAS UNLOCKED!" );   
            }
            catch
            {
                DebugManager.Log<SteamGameManager>("Unable to set unlocked achievement status on Steam");
            }
        }

        // se um achievement foi desbloqueado existe essa função para teste
        private static void AchievementChanged( Achievement ach, int currentProgress, int progress )
        {
            if ( ach.State )
            {
                DebugManager.Log<SteamGameManager>( $"{ach.Name} WAS UNLOCKED!" );   
            }
        }

        public static void SetStat(string statName, float totals, bool instant)
        {
            if (!ConnectedToSteam) return;
            try
            {
                SteamUserStats.SetStat( statName, totals );
                if (instant)
                    SteamUserStats.StoreStats();
            }
            catch (Exception e)
            {
                DebugManager.LogError<SteamGameManager>(e);
            }
        }
        public static void SetStat(string statName, int totals, bool instant)
        {
            if (!ConnectedToSteam) return;
            try
            {
                SteamUserStats.SetStat( statName, totals);
                if (instant)
                    SteamUserStats.StoreStats();
            }
            catch (Exception e)
            {
                DebugManager.LogError<SteamGameManager>(e);
            }
        }
        public static void AddStat(string statName, float totals, bool instant)
        {
            if (!ConnectedToSteam) return;
            try
            {
                SteamUserStats.AddStat( statName, totals);
                if (instant)
                    SteamUserStats.StoreStats();
            }
            catch (Exception e)
            {
                DebugManager.LogError<SteamGameManager>(e);
            }
            
        }
        public static void AddStat(string statName, int totals, bool instant)
        {
            if (!ConnectedToSteam) return;
            try
            {
                SteamUserStats.AddStat( statName, totals);
                if (instant)
                    SteamUserStats.StoreStats();
            }
            catch (Exception e)
            {
                DebugManager.LogError<SteamGameManager>(e);
            }
        }
        
        public static int GetStatInt(string statName)
        {
            var stat = 0;
            if (SteamClient.IsValid)
            {
                stat = SteamUserStats.GetStatInt(statName);
            }
            return stat;
        }
        public static float GetStatFloat(string statName)
        {
            var stat = 0f;
            if (SteamClient.IsValid)
            {
                stat = SteamUserStats.GetStatFloat(statName);
            }
            return stat;
        }

        public static void StoreStats()
        {
            if (!ConnectedToSteam) return;
                SteamUserStats.StoreStats();
        }

        private void GameCleanup()
        {
            if (_applicationHasQuit)
                return;
            _applicationHasQuit = true;
            //leaveLobby();
            _leaderboard = null;
            SteamClient.Shutdown();
        }

        private static void ClearAllStats(bool includeAchievements)
        {
            SteamUserStats.ResetAll( includeAchievements );
            SteamUserStats.StoreStats();
            SteamUserStats.RequestCurrentStats();
        }

        #region LeaderBoardas Methods


        public static async void UpdateScore(int score, bool force)
        {
            if (!ConnectedToSteam || _leaderboard == null || score <= 0)
                return;
            LeaderboardUpdate? result;
            if (force)
            {
                result = await _leaderboard.Value.ReplaceScore(score).ConfigureAwait(false);
            }
            else
            {
                result = await _leaderboard.Value.SubmitScoreAsync(score).ConfigureAwait(false);
            }
            if (result != null)
                DebugManager.Log<SteamGameManager>($"Register: {result.Value.Score}");
        }

        public static async Task<LeaderboardEntry[]> GetScores(int quantity)
        {
            if (!ConnectedToSteam || _leaderboard == null)
                return null;
            return await _leaderboard.Value.GetScoresAsync(quantity).ConfigureAwait(false);
        }
        public static async Task<LeaderboardEntry[]> GetScoresFromFriends()
        {
            if (!ConnectedToSteam || _leaderboard == null)
                return null;
            return await _leaderboard.Value.GetScoresFromFriendsAsync().ConfigureAwait(false);
        }
        public static async Task<LeaderboardEntry[]> GetScoresAround(int start, int end)
        {
            if (!ConnectedToSteam || _leaderboard == null)
                return null;
            return await _leaderboard.Value. GetScoresAroundUserAsync(start, end).ConfigureAwait(false);
        }
  #endregion

    }
    
}

