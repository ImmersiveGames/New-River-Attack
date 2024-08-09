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
        private bool ConnectedToSteam => SteamConnectionManager.ConnectedToSteam;
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
            if (SteamConnectionManager.ConnectedToSteam)
            {
                _serverAchievements = SteamUserStats.Achievements
                    .Where(a => a.State)
                    .Select(a => a.Identifier)
                    .ToHashSet();
            }
        }

        public bool IsAchievementUnlocked(string achievementId)
        {
            return _serverAchievements.Contains(achievementId) || _offlineAchievements.Contains(achievementId);
        }

        public void SyncAchievements()
        {
            //primeiro sincronizar offline.
            SaveOfflineAchievements();

            if (!SteamConnectionManager.ConnectedToSteam)
                return;

            foreach (var achievementId in _offlineAchievements.ToList())
            {
                UnlockAchievement(achievementId);
            }

            _offlineAchievements.Clear();
            SteamUserStats.StoreStats();
        }
        public void UnlockAchievement(string achievementId, bool notify = true)
        {
            if (IsAchievementUnlocked(achievementId))
                return;
            var achievement = new Achievement(achievementId);
            try
            {
                _offlineAchievements.Add(achievementId);
                SaveOfflineAchievements();
                if (ConnectedToSteam)
                {
                    achievement.Trigger();
                    DebugManager.Log<SteamAchievementService>(
                        $"{achievement.Name} ({achievementId}) foi desbloqueado!");

                    _serverAchievements.Add(achievementId);
                    SteamUserStats.StoreStats();
                }
                else
                {
                    notify = true;
                }

                if (notify && NotifyOnAchievementUnlock)
                {
                    NotifyAchievementUnlocked(achievement);
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
            SaveOfflineAchievements();
            DebugManager.Log<SteamAchievementService>($"Conquistas Resetadas!");
        }

        #endregion

        #region Metodos Save Offline

        public void SaveOfflineAchievements()
        {
            PlayerPrefs.SetString("OfflineAchievements", string.Join(";", _offlineAchievements));
            PlayerPrefs.Save();
        }

        public void LoadOfflineAchievements()
        {
            var savedAchievements = PlayerPrefs.GetString("OfflineAchievements", "");
            if (!string.IsNullOrEmpty(savedAchievements))
            {
                _offlineAchievements = savedAchievements.Split(';').ToHashSet();
            }
        }

        #endregion

        #region Auxiliares

        private static void NotifyAchievementUnlocked(Achievement achievement)
        {
            var notificationData = new NotificationData
            {
                message = $"Conquista Desbloqueada: {achievement.Name}",
                panelPrefab = NotificationManager.instance.notificationPanelPrefab,
                ConfirmAction = () =>
                    DebugManager.Log<SteamAchievementService>($"Conquista Confirmada: {achievement.Name}")
            };

            NotificationManager.instance.AddNotification(notificationData);
        }

        #endregion
    }
}