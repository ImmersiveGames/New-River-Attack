﻿using System;
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
        static GameSteamManager _instance;
        const int STEAM_ID = 2777110;
        const string LEADERBOARD_NAME = "River_Attack_HiScore";
        static IEnumerable<Achievement> _serverAchievements;
        public bool resetAchievementsOnStart;
        bool m_ApplicationHasQuit;

        static Leaderboard? _leaderboard;

        public static bool connectedToSteam
        {
            get;
            private set;
        }
        #region UNITYMETHODS
        async void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                try
                {
                    SteamClient.Init( STEAM_ID);
                    if (!SteamClient.IsValid)
                    {
                        //Debug.Log("Steam client not valid");
                        throw new Exception();
                    }
                    connectedToSteam = true;
                    _serverAchievements = SteamUserStats.Achievements;
                    _leaderboard = await SteamUserStats.FindLeaderboardAsync(LEADERBOARD_NAME);

                    Debug.Log("Leaderboard initialized: " + _leaderboard);
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
        void Start()
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
        void Update()
        {
            SteamClient.RunCallbacks();
        }
        void OnDisable()
        {
            GameCleanup();
        }
        protected void OnDestroy()
        {
            //base.OnDestroy();
            GameCleanup();
        }
        void OnApplicationQuit()
        {
            GameCleanup();
        }
  #endregion
        
        void ReconcileMissedAchievements()
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
        static void AchievementChanged( Achievement ach, int currentProgress, int progress )
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
            return !SteamClient.IsValid ? 0 : SteamUserStats.GetStatInt( statName );
        }
        public static float GetStatFloat(string statName)
        {
            return !SteamClient.IsValid ? 0 : SteamUserStats.GetStatFloat( statName );
        }

        public static void StoreStats()
        {
            if (!connectedToSteam) return;
            SteamUserStats.StoreStats();
        }
        void GameCleanup()
        {
            if (m_ApplicationHasQuit)
                return;
            m_ApplicationHasQuit = true;
            //leaveLobby();
            _leaderboard = null;
            SteamClient.Shutdown();
        }

        static void ClearAllStats(bool includeAchievements)
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
            Debug.Log($"REGISTRAAAAAAAA {score} AQUI: {_leaderboard}");
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
                Debug.Log($"Registrou: {result.Value.Score}");
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

