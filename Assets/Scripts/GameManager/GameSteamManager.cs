using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Steamworks;
using Steamworks.Data;
namespace RiverAttack
{
    public class GameSteamManager: MonoBehaviour
    {
        private static GameSteamManager _instance;
        private const int SteamID = 2777110;
        private const string LeaderboardName = "River_Attack_HiScore";
        private static IEnumerable<Achievement> _serverAchievements;
        public bool resetAchievementsOnStart;
        public bool demoMode;
        private bool _applicationHasQuit;

        private static Leaderboard? _leaderboard;

        public static bool connectedToSteam
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
                            //Debug.Log("Steam client not valid");
                            throw new Exception();
                        }
                        connectedToSteam = true;
                        _serverAchievements = SteamUserStats.Achievements;
                        _leaderboard = await SteamUserStats.FindLeaderboardAsync(LeaderboardName);

                        //Debug.Log("Leaderboard initialized: " + _leaderboard);
                    }
                    else
                    {
                        connectedToSteam = false;
                    }
                    
                }
                catch ( Exception e )
                {
                    connectedToSteam = false;
                    Debug.Log("Error connecting to Steam");
                    Debug.Log(e);
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
                Debug.Log( $"{a.Name} ({a.State}) {a.Identifier}" );
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

  private void ReconcileMissedAchievements()
        {
            // Aqui ele coloca os achievementes offline para atualizar.
            
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
            if (!connectedToSteam) return;
            if (!SteamClient.IsValid || !SteamClient.IsLoggedOn ) return;
            try
            {
                foreach (string achievement in achievementsToUnlock)
                {
                    UnlockAchievement(achievement);
                }
            }
            catch
            {
                Debug.Log("Unable to set unlocked achievement status on Steam");
            }
        }
        public static void UnlockAchievement(string archiveID)
        {
            if (!connectedToSteam) return;
            try
            {
                if (!_serverAchievements.Any(achievement => achievement.Identifier == archiveID && !achievement.State))
                    return;
                var ach = new Achievement(archiveID);
                ach.Trigger();
            }
            catch
            {
                Debug.Log("Unable to set unlocked achievement status on Steam");
            }
        }

        // se um archvment foi desbloqueado existe essa função para teste
        private static void AchievementChanged( Achievement ach, int currentProgress, int progress )
        {
            if ( ach.State )
            {
                Debug.Log( $"{ach.Name} WAS UNLOCKED!" );   
            }
        }

        public static void SetStat(string statName, float totals, bool instant)
        {
            if (!connectedToSteam) return;
            try
            {
                SteamUserStats.SetStat( statName, totals );
                if (instant)
                    SteamUserStats.StoreStats();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
        public static void SetStat(string statName, int totals, bool instant)
        {
            if (!connectedToSteam) return;
            try
            {
                SteamUserStats.SetStat( statName, totals);
                if (instant)
                    SteamUserStats.StoreStats();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
        public static void AddStat(string statName, float totals, bool instant)
        {
            if (!connectedToSteam) return;
            try
            {
                SteamUserStats.AddStat( statName, totals);
                if (instant)
                    SteamUserStats.StoreStats();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            
        }
        public static void AddStat(string statName, int totals, bool instant)
        {
            if (!connectedToSteam) return;
            try
            {
                SteamUserStats.AddStat( statName, totals);
                if (instant)
                    SteamUserStats.StoreStats();
            }
            catch (Exception e)
            {
                Debug.Log(e);
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
            if (!connectedToSteam) return;
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
            if (!connectedToSteam || _leaderboard == null || score <= 0)
                return;
            LeaderboardUpdate? result;
            if (force)
            {
                result = await _leaderboard.Value.ReplaceScore(score);
            }
            else
            {
                result = await _leaderboard.Value.SubmitScoreAsync(score);
            }
            if (result != null)
                Debug.Log($"Register: {result.Value.Score}");
        }

        public static async Task<LeaderboardEntry[]> GetScores(int quantity)
        {
            if (!connectedToSteam || _leaderboard == null)
                return null;
            return await _leaderboard.Value.GetScoresAsync(quantity);
        }
        public static async Task<LeaderboardEntry[]> GetScoresFromFriends()
        {
            if (!connectedToSteam || _leaderboard == null)
                return null;
            return await _leaderboard.Value.GetScoresFromFriendsAsync();
        }
        public static async Task<LeaderboardEntry[]> GetScoresAround(int start, int end)
        {
            if (!connectedToSteam || _leaderboard == null)
                return null;
            return await _leaderboard.Value. GetScoresAroundUserAsync(start, end);
        }
  #endregion

    }
    
}

