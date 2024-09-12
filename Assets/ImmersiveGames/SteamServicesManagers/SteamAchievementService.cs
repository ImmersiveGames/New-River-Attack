using System;
using System.Collections.Generic;
using System.Linq;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.MenuManagers.NotificationManager;
using ImmersiveGames.SteamServicesManagers.Interface;
using NewRiverAttack.LevelBuilder;
using UnityEngine;
using Steamworks;
using Steamworks.Data;

namespace ImmersiveGames.SteamServicesManagers
{
    public class SteamAchievementService : MonoBehaviour, IAchievementService
    {
        public static SteamAchievementService Instance { get; private set; }
        private HashSet<string> _serverAchievements = new HashSet<string>();
        private HashSet<string> _offlineAchievements = new HashSet<string>();
        private bool ConnectedToSteam => SteamConnectionManager.ConnectedToSteam;
        private bool NotifyOnAchievementUnlock { get; set; } = false;

        #region Unity Methods

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                //DontDestroyOnLoad(gameObject);
                DebugManager.Log<SteamAchievementService>("Instância criada e marcada para não destruir ao carregar uma nova cena.");
            }
            else
            {
                Destroy(gameObject);
                DebugManager.LogWarning<SteamAchievementService>("Tentativa de criar outra instância evitada e o novo objeto foi destruído.");
            }
        }

        private void Start()
        {
            DebugManager.Log<SteamAchievementService>("Iniciando carregamento e sincronização de conquistas...");
            LoadAchievements();
            SyncAchievements(); // Tenta sincronizar conquistas ao iniciar
        }

        private void OnApplicationQuit()
        {
            DebugManager.Log<SteamAchievementService>("Aplicativo está fechando. Sincronizando conquistas...");
            SyncAchievements(); // Sincroniza conquistas ao sair
        }

        #endregion

        #region IAchievementService Methods

        public void LoadAchievements()
        {
            DebugManager.Log<SteamAchievementService>("Carregando conquistas offline...");
            LoadOfflineAchievements();

            if (ConnectedToSteam)
            {
                DebugManager.Log<SteamAchievementService>("Conectado ao Steam. Carregando conquistas do servidor...");
                _serverAchievements = SteamUserStats.Achievements
                    .Where(a => a.State)
                    .Select(a => a.Identifier)
                    .ToHashSet();
                DebugManager.Log<SteamAchievementService>($"Conquistas carregadas do servidor: {_serverAchievements.Count}");
            }
            else
            {
                DebugManager.LogWarning<SteamAchievementService>("Não está conectado ao Steam. Conquistas do servidor não carregadas.");
            }
        }

        public bool IsAchievementUnlocked(string achievementId)
        {
            bool unlocked = _serverAchievements.Contains(achievementId);
            DebugManager.Log<SteamAchievementService>($"Verificação de conquista '{achievementId}' - Desbloqueada no servidor: {unlocked}");
            return unlocked;
        }

        public void SyncAchievements()
        {
            if (!ConnectedToSteam)
            {
                DebugManager.LogWarning<SteamAchievementService>("Não está conectado ao Steam. Salvando conquistas offline...");
                SaveOfflineAchievements();
                return;
            }

            DebugManager.Log<SteamAchievementService>("Sincronizando conquistas offline com o servidor...");
            foreach (var achievementId in _offlineAchievements.ToList())
            {
                try
                {
                    // Tentativa de sincronização no servidor
                    if (!IsAchievementUnlocked(achievementId))
                    {
                        var achievement = new Achievement(achievementId);
                        achievement.Trigger();
                        DebugManager.Log<SteamAchievementService>($"Conquista '{achievementId}' sincronizada com sucesso.");
                        _serverAchievements.Add(achievementId);
                    }
                    else
                    {
                        DebugManager.Log<SteamAchievementService>($"Conquista '{achievementId}' já está desbloqueada no servidor.");
                    }
                    _offlineAchievements.Remove(achievementId); // Remove da lista offline após sincronização bem-sucedida
                }
                catch (Exception e)
                {
                    DebugManager.LogError<SteamAchievementService>($"Falha ao sincronizar conquista {achievementId}: {e}");
                }
            }

            SteamUserStats.StoreStats();
            SaveOfflineAchievements(); // Salva novamente para garantir que só as não sincronizadas fiquem armazenadas
            DebugManager.Log<SteamAchievementService>("Sincronização concluída.");
        }

        public void UnlockAchievement(string achievementId, bool notify = true)
        {
            if (IsAchievementUnlocked(achievementId))
            {
                DebugManager.Log<SteamAchievementService>($"Conquista '{achievementId}' já foi desbloqueada anteriormente no servidor.");
                return;
            }

            try
            {
                var achievementName = achievementId; // Nome padrão se offline

                if (ConnectedToSteam)
                {
                    var achievement = new Achievement(achievementId);
                    achievement.Trigger();
                    achievementName = achievement.Name; // Atualiza o nome se online

                    DebugManager.Log<SteamAchievementService>($"Conquista desbloqueada online: {achievementName} ({achievementId})");

                    _serverAchievements.Add(achievementId);
                    SteamUserStats.StoreStats();
                }
                else
                {
                    DebugManager.LogWarning<SteamAchievementService>($"Não está conectado ao Steam. Desbloqueando conquista '{achievementId}' offline.");
                    _offlineAchievements.Add(achievementId);
                    SaveOfflineAchievements();
                }

                if (notify && NotifyOnAchievementUnlock)
                {
                    NotifyAchievementUnlocked(achievementId, achievementName); // Notifica o usuário com o ID e Nome
                }
            }
            catch (Exception e)
            {
                DebugManager.LogError<SteamAchievementService>($"Erro ao desbloquear a conquista {achievementId} : {e}");
            }
        }

        public void ResetAllAchievements()
        {
            if (ConnectedToSteam)
            {
                SteamUserStats.ResetAll(true);
                SteamUserStats.StoreStats();
                DebugManager.Log<SteamAchievementService>("Conquistas Online Resetadas!");
            }

            PlayerPrefs.DeleteAll();
            _offlineAchievements.Clear();
            SaveOfflineAchievements();
            DebugManager.Log<SteamAchievementService>("Conquistas Offline Resetadas!");
        }

        #endregion

        #region Métodos Save Offline

        private void SaveOfflineAchievements()
        {
            DebugManager.Log<SteamAchievementService>("Salvando conquistas offline...");
            PlayerPrefs.SetString("OfflineAchievements", string.Join(";", _offlineAchievements));
            PlayerPrefs.Save();
            DebugManager.Log<SteamAchievementService>("Conquistas offline salvas com sucesso.");
        }

        private void LoadOfflineAchievements()
        {
            DebugManager.Log<SteamAchievementService>("Carregando conquistas offline de PlayerPrefs...");
            var savedAchievements = PlayerPrefs.GetString("OfflineAchievements", "");
            if (!string.IsNullOrEmpty(savedAchievements))
            {
                _offlineAchievements = savedAchievements.Split(';').ToHashSet();
                DebugManager.Log<SteamAchievementService>($"Conquistas offline carregadas: {_offlineAchievements.Count}");
            }
            else
            {
                DebugManager.Log<SteamAchievementService>("Nenhuma conquista offline encontrada para carregar.");
            }
        }

        #endregion

        #region Auxiliares

        private void NotifyAchievementUnlocked(string achievementId, string achievementName)
        {
            DebugManager.Log<SteamAchievementService>($"Notificando o desbloqueio da conquista: {achievementName} ({achievementId})");

            var notificationData = new NotificationData
            {
                message = $"Conquista Desbloqueada: {achievementName}",
                panelPrefab = NotificationManager.instance.notificationPanelPrefab,
                ConfirmAction = () => Debug.Log($"Conquista Confirmada: {achievementName}")
            };

            NotificationManager.instance.AddNotification(notificationData);
        }

        #endregion
    }
}
