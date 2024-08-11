using System;
using System.Collections.Generic;
using System.Linq;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.MenuManagers.NotificationManager;
using ImmersiveGames.SteamServicesManagers.Interface;
using Steamworks;
using Steamworks.Data;
using UnityEngine;

namespace ImmersiveGames.SteamServicesManagers
{
    public class SteamAchievementService : MonoBehaviour, IAchievementService
    {
        public static SteamAchievementService Instance { get; private set; }
        private HashSet<string> _serverAchievements = new HashSet<string>();
        private HashSet<string> _offlineAchievements = new HashSet<string>();
        private static bool ConnectedToSteam => SteamConnectionManager.ConnectedToSteam;
        private static bool SteamDebug => false;
        private bool NotifyOnAchievementUnlock { get; set; } = true;

        #region Unity Methods

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            LoadAchievements();
            SyncAchievements(); // Tenta sincronizar conquistas ao iniciar
        }
        private void OnApplicationQuit()
        {
            SyncAchievements(); // Sincroniza conquistas ao sair
        }

        #endregion

        #region IAchievementService Methods

        public void LoadAchievements()
        {
            //load Achievement offline
            LoadOfflineAchievements();
            //load Achievement online
            if (ConnectedToSteam)
            {
                _serverAchievements = SteamUserStats.Achievements
                    .Where(a => a.State)
                    .Select(a => a.Identifier)
                    .ToHashSet();
            }
        }

        public bool IsAchievementUnlocked(string achievementId)
        {
            return _serverAchievements.Contains(achievementId);
        }

        public void SyncAchievements()
        {
            //primeiro sincronizar offline.
            SaveOfflineAchievements();
            if (!ConnectedToSteam && !SteamDebug)
                return;
            foreach (var achievementId in _offlineAchievements.ToList())
            {
                UnlockAchievement(achievementId);
            }
            SaveOfflineAchievements();
            if(!SteamDebug) SteamUserStats.StoreStats();
        }
        public void UnlockAchievement(string achievementId)
        {
            if (IsAchievementUnlocked(achievementId))
            {
                DebugManager.Log<SteamAchievementService>($"O ID: {achievementId} já foi desbloqueado");
                if (_offlineAchievements.Contains(achievementId))
                {
                    _offlineAchievements.Remove(achievementId);
                }
                return;
            }

            var achievement = new Achievement(achievementId);
            try
            {
                _offlineAchievements.Add(achievementId);
                string nameAchievement;
                if (ConnectedToSteam || SteamDebug)
                {
                    nameAchievement = SteamDebug ? achievementId : achievement.Name;
                    if(!SteamDebug) achievement.Trigger();
                    _serverAchievements.Add(achievementId);
                    if(!SteamDebug) SteamUserStats.StoreStats();
                    DebugManager.Log<SteamAchievementService>($"Registrou na Steam {nameAchievement}");
                }
                else if (NotifyOnAchievementUnlock)
                {
                    nameAchievement = achievementId;
                    NotifyAchievementUnlocked(nameAchievement);
                }
            }
            catch (Exception e)
            {
                DebugManager.LogError<SteamAchievementService>($"Erro ao desbloquear a conquista: {e}");
            }
        }

        public void ResetAllAchievements()
        {
            if (ConnectedToSteam)
            {
                SteamUserStats.ResetAll(true);
                SteamUserStats.StoreStats();
            }
            _offlineAchievements.Clear();
            _serverAchievements.Clear();
            SaveOfflineAchievements();
            DebugManager.Log<SteamAchievementService>($"Conquistas Resetadas!");
        }

        #endregion

        #region Metodos Save Offline

        public void SaveOfflineAchievements()
        {
            PlayerPrefs.SetString("OfflineAchievements", string.Join(";", _offlineAchievements));
            PlayerPrefs.Save();
            DebugManager.Log<SteamAchievementService>($"Save PlayerPreferences: {PlayerPrefs.GetString("OfflineAchievements")}");
        }

        public void LoadOfflineAchievements()
        {
            var savedAchievements = PlayerPrefs.GetString("OfflineAchievements", "");
            DebugManager.Log<SteamAchievementService>($"Load - PlayerPreferences: {savedAchievements}");
            if (!string.IsNullOrEmpty(savedAchievements))
            {
                _offlineAchievements = savedAchievements.Split(';').ToHashSet();
            }
        }

        #endregion

        #region Auxiliares

        private static void NotifyAchievementUnlocked(string nameAchievement)
        {
            var notificationData = new NotificationData
            {
                message = $"Conquista Desbloqueada: {nameAchievement}",
                ConfirmAction = () =>
                    DebugManager.Log<SteamAchievementService>($"Conquista Confirmada: {nameAchievement}")
            };

            NotificationManager.instance.AddNotification(notificationData);
        }

        #endregion
    }
}