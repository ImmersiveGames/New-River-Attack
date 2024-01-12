using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Steamworks;
using Steamworks.Data;
namespace RiverAttack
{
    public class GameSteamManager: MonoBehaviour
    {
        const int STEAM_ID = 2777110;
        string m_PlayerName;
        static IEnumerable<Achievement> _serverAchievements;
        public bool resetAchievementsOnStart;
        bool m_ApplicationHasQuit;

        public static bool connectedToSteam
        {
            get;
            set;
        }
        #region UNITYMETHODS
        void Awake()
        {
            try
            {
                SteamClient.Init( STEAM_ID);
                if (!SteamClient.IsValid)
                {
                    Debug.Log("Steam client not valid");
                    throw new Exception();
                }
                m_PlayerName = SteamClient.Name;
                connectedToSteam = true;
                _serverAchievements = SteamUserStats.Achievements;
                Debug.Log("Steam initialized: " + m_PlayerName);
                
            }
            catch ( Exception e )
            {
                connectedToSteam = false;

                Debug.Log("Error connecting to Steam");
                Debug.Log(e);
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
            try
            {
                SteamUserStats.SetStat( statName, totals );
                if (instant)
                    SteamUserStats.StoreStats();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public static void SetStat(string statName, int totals, bool instant)
        {
            try
            {
                SteamUserStats.SetStat( statName, totals);
                if (instant)
                    SteamUserStats.StoreStats();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public static void AddStat(string statName, float totals, bool instant)
        {
            try
            {
                SteamUserStats.AddStat( statName, totals);
                if (instant)
                    SteamUserStats.StoreStats();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
        public static void AddStat(string statName, int totals, bool instant)
        {
            try
            {
                SteamUserStats.AddStat( statName, totals);
                if (instant)
                    SteamUserStats.StoreStats();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        /*public static void AddStatTest(string statName, int totals, bool instant)
        {
            int result = SteamUserStats.GetStatInt( statName );
            Debug.Log($"{statName} result: {result}");
            SteamUserStats.AddStat( statName, totals );
            result = SteamUserStats.GetStatInt( statName );
            SteamUserStats.StoreStats();
            Debug.Log($"Novo - {statName} result: {result}");
        }*/
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
            SteamUserStats.StoreStats();
        }
        void GameCleanup()
        {
            if (m_ApplicationHasQuit)
                return;
            m_ApplicationHasQuit = true;
            //leaveLobby();
            SteamClient.Shutdown();
        }

        static void ClearAllStats(bool includeAchievements)
        {
            SteamUserStats.ResetAll( includeAchievements ); // true = wipe achivements too
            SteamUserStats.StoreStats();
            SteamUserStats.RequestCurrentStats();
        }

    }
    
}

