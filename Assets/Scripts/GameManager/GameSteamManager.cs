using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Steamworks;
using Steamworks.Data;
using Utils;
namespace RiverAttack
{
    public class GameSteamManager: MonoBehaviour
    {
        const int STEAM_ID = 2777110;
        
        string m_PlayerName;
        static IEnumerable<Achievement> _serverAchievements;
        
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
            ReconcileMissedAchievements();
            SteamUserStats.OnAchievementProgress += AchievementChanged;
            foreach ( var a in SteamUserStats.Achievements )
            {
                Debug.Log( $"{a.Name} ({a.State}) {a.Identifier}" );
            }
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
            if (!SteamClient.IsValid) return;
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

        public static void SetState(string statName, float totals)
        {
            if (!SteamClient.IsValid) return;
            SteamUserStats.SetStat( statName, totals );
        }
        public static void SetState(string statName, int totals)
        {
            if (!SteamClient.IsValid) return;
            SteamUserStats.SetStat( statName, totals );
        }
        static void AddState(string statName, float totals)
        {
            if (!SteamClient.IsValid) return;
            SteamUserStats.AddStat( statName, totals );
        }
        public static void AddState(string statName, int totals)
        {
            if (!SteamClient.IsValid) return;
            SteamUserStats.AddStat( statName, totals );
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
            if (!SteamClient.IsValid) return;
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
#if UNITY_EDITOR
        static void ClearAllStats(bool includeAchievements)
        {
            SteamUserStats.ResetAll( includeAchievements ); // true = wipe achivements too
            SteamUserStats.StoreStats();
            SteamUserStats.RequestCurrentStats();
        }
#endif
    }
}

