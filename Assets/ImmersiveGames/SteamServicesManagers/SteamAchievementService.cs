using System;
using System.Collections.Generic;
using System.Linq;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.MenuManagers.NotificationManager;
using ImmersiveGames.SteamServicesManagers.Interface;
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
        private bool ConnectedToSteam => SteamConnectionManager.ConnectedToSteam; // Verifica conexão com Steam
        private bool NotifyOnAchievementUnlock { get; set; } = false; // Definir se deve notificar conquistas

        #region Unity Methods

        private void Awake()
        {
            if (Instance == null)
            {
                // Cria o objeto na raiz da cena se ele não for o objeto raiz
                if (transform.parent != null)
                {
                    GameObject rootObject = new GameObject("SteamAchievementService");
                    Instance = rootObject.AddComponent<SteamAchievementService>();
                    DontDestroyOnLoad(rootObject);  // Marca o objeto como persistente
                }
                else
                {
                    Instance = this;
                    DontDestroyOnLoad(gameObject);  // Marca o objeto como persistente
                }
                DebugManager.Log<SteamAchievementService>("Instância criada e marcada para não destruir ao carregar uma nova cena.");
            }
            else
            {
                Destroy(gameObject);  // Garante que não haverá múltiplas instâncias
                DebugManager.LogWarning<SteamAchievementService>("Tentativa de criar outra instância evitada e o novo objeto foi destruído.");
            }
        }

        private void Start()
        {
            DebugManager.Log<SteamAchievementService>("Iniciando carregamento e sincronização de conquistas...");

            if (ConnectedToSteam)
            {
                DebugManager.Log<SteamAchievementService>("Conectado ao Steam. Carregando conquistas do servidor.");
                LoadAchievements();
                SyncAchievements(); // Sincroniza conquistas ao iniciar
            }
            else
            {
                DebugManager.LogWarning<SteamAchievementService>("Não está conectado ao Steam. Operando no modo offline.");
                LoadOfflineAchievements();
            }
        }

        private void OnApplicationQuit()
        {
            DebugManager.Log<SteamAchievementService>("Aplicativo está fechando. Sincronizando conquistas...");
            SyncAchievements(); // Sincroniza conquistas ao sair
        }

        #endregion

        #region IAchievementService Methods

        // Carrega conquistas do servidor Steam
        public void LoadAchievements()
        {
            DebugManager.Log<SteamAchievementService>("Carregando conquistas offline...");

            LoadOfflineAchievements(); // Carrega conquistas offline primeiro

            if (ConnectedToSteam)
            {
                DebugManager.Log<SteamAchievementService>("Carregando conquistas do servidor Steam...");

                _serverAchievements = SteamUserStats.Achievements
                    .Where(a => a.State)
                    .Select(a => a.Identifier)
                    .ToHashSet();

                DebugManager.Log<SteamAchievementService>($"Conquistas carregadas do servidor: {_serverAchievements.Count}");
            }
            else
            {
                DebugManager.LogWarning<SteamAchievementService>("Não foi possível carregar conquistas do servidor, sem conexão com o Steam.");
            }
        }

        // Verifica se uma conquista foi desbloqueada
        public bool IsAchievementUnlocked(string achievementId)
        {
            bool unlocked = _serverAchievements.Contains(achievementId);
            DebugManager.Log<SteamAchievementService>($"Conquista '{achievementId}' - Desbloqueada no servidor: {unlocked}");
            return unlocked;
        }

        // Sincroniza conquistas offline com o servidor Steam
        public void SyncAchievements()
        {
            if (!ConnectedToSteam)
            {
                DebugManager.LogWarning<SteamAchievementService>("Sem conexão ao Steam. Salvando conquistas offline...");
                SaveOfflineAchievements();
                return;
            }

            DebugManager.Log<SteamAchievementService>("Sincronizando conquistas offline com o servidor...");

            foreach (var achievementId in _offlineAchievements.ToList())
            {
                try
                {
                    if (!IsAchievementUnlocked(achievementId))
                    {
                        var achievement = new Achievement(achievementId);
                        achievement.Trigger(); // Desbloqueia a conquista no Steam
                        DebugManager.Log<SteamAchievementService>($"Conquista '{achievementId}' sincronizada com sucesso.");
                        _serverAchievements.Add(achievementId);
                    }
                    _offlineAchievements.Remove(achievementId); // Remove da lista de conquistas offline
                }
                catch (Exception e)
                {
                    DebugManager.LogError<SteamAchievementService>($"Erro ao sincronizar conquista {achievementId}: {e}");
                }
            }

            SteamUserStats.StoreStats(); // Salva no Steam
            SaveOfflineAchievements(); // Salva localmente
        }

        // Desbloqueia uma conquista
        public void UnlockAchievement(string achievementId, bool notify = true)
        {
            if (IsAchievementUnlocked(achievementId))
            {
                DebugManager.Log<SteamAchievementService>($"Conquista '{achievementId}' já foi desbloqueada.");
                return;
            }

            try
            {
                var achievementName = achievementId; // Nome padrão para modo offline

                if (ConnectedToSteam)
                {
                    var achievement = new Achievement(achievementId);
                    achievement.Trigger(); // Desbloqueia no Steam
                    achievementName = achievement.Name; // Nome atualizado do Steam
                    _serverAchievements.Add(achievementId);
                    SteamUserStats.StoreStats();
                    DebugManager.Log<SteamAchievementService>($"Conquista '{achievementName}' ({achievementId}) desbloqueada.");
                }
                else
                {
                    DebugManager.LogWarning<SteamAchievementService>($"Desbloqueando conquista '{achievementId}' offline.");
                    _offlineAchievements.Add(achievementId);
                    SaveOfflineAchievements();
                }

                if (notify && NotifyOnAchievementUnlock)
                {
                    NotifyAchievementUnlocked(achievementId, achievementName); // Notifica o jogador
                }
            }
            catch (Exception e)
            {
                DebugManager.LogError<SteamAchievementService>($"Erro ao desbloquear a conquista {achievementId}: {e}");
            }
        }

        public void ResetAllAchievements()
        {
            if (ConnectedToSteam)
            {
                SteamUserStats.ResetAll(true); // Reseta conquistas no Steam
                SteamUserStats.StoreStats();
                DebugManager.Log<SteamAchievementService>("Conquistas online resetadas.");
            }

            PlayerPrefs.DeleteAll();
            _offlineAchievements.Clear();
            SaveOfflineAchievements();
            DebugManager.Log<SteamAchievementService>("Conquistas offline resetadas.");
        }

        #endregion

        #region Offline Achievement Methods

        // Salva conquistas offline no PlayerPrefs
        private void SaveOfflineAchievements()
        {
            DebugManager.Log<SteamAchievementService>("Salvando conquistas offline...");
            PlayerPrefs.SetString("OfflineAchievements", string.Join(";", _offlineAchievements));
            PlayerPrefs.Save();
            DebugManager.Log<SteamAchievementService>("Conquistas offline salvas com sucesso.");
        }

        // Carrega conquistas offline do PlayerPrefs
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
                DebugManager.Log<SteamAchievementService>("Nenhuma conquista offline encontrada.");
            }
        }

        #endregion

        #region Notification Methods

        // Notifica que uma conquista foi desbloqueada
        private void NotifyAchievementUnlocked(string achievementId, string achievementName)
        {
            DebugManager.Log<SteamAchievementService>($"Notificando desbloqueio da conquista: {achievementName} ({achievementId})");

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
