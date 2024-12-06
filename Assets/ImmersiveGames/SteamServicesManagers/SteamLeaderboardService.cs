using System;
using ImmersiveGames.DebugManagers;
using NewRiverAttack.GamePlayManagers.GamePlayLogs;
using Steamworks;
using Steamworks.Data;
using UnityEngine;

namespace ImmersiveGames.SteamServicesManagers
{
    public class SteamLeaderboardService : MonoBehaviour
    {
        [SerializeField] private string leaderboardName = "River_Attack_HiScore";
        private Leaderboard? _leaderboard;
        public Leaderboard? Leaderboard => _leaderboard;
        public static SteamLeaderboardService Instance { get; private set; }
        private GemeStatisticsDataLog _gemeStatistics;

        private void Awake()
        {
            if (Instance == null)
            {
                // Cria o objeto na raiz da cena se ele não for o objeto raiz
                if (transform.parent != null)
                {
                    GameObject rootObject = new GameObject("SteamLeaderboardService");
                    Instance = rootObject.AddComponent<SteamLeaderboardService>();
                    DontDestroyOnLoad(rootObject);  // Marca o objeto como persistente
                }
                else
                {
                    Instance = this;
                    _gemeStatistics = GemeStatisticsDataLog.Instance;
                    Init(leaderboardName);
                    DontDestroyOnLoad(gameObject);  // Marca o objeto como persistente
                }
                DebugManager.Log<SteamLeaderboardService>("Instância criada e marcada para não destruir ao carregar uma nova cena.");
            }
            else
            {
                Destroy(gameObject);  // Garante que não haverá múltiplas instâncias
                DebugManager.LogWarning<SteamLeaderboardService>("Tentativa de criar outra instância evitada e o novo objeto foi destruído.");
            }
        }

        private void Start()
        {
            if (_gemeStatistics.playersMaxScore > 0)
            {
                UpdateScore(_gemeStatistics.playersMaxScore);
            }
        }
        private void OnDisable()
        {
            _leaderboard = null;
        }

        private async void Init(string boardName)
        {
            if (!SteamConnectionManager.ConnectedToSteam) return;
            _leaderboard = await SteamUserStats.FindLeaderboardAsync(boardName).ConfigureAwait(false);
            if (_leaderboard.HasValue)
            {
                // Leaderboard encontrado, use leaderboard.Value para acessar
                DebugManager.Log<SteamLeaderboardService>("Leaderboard encontrado: " + _leaderboard.Value.Name);
            }
            else
            {
                // Leaderboard não encontrado ou houve erro
                DebugManager.LogError<SteamLeaderboardService>("O leaderboard não foi encontrado.");
            }
        }

        public async void UpdateScore(int score)
        {
            if (!SteamConnectionManager.ConnectedToSteam) return;
            if (_leaderboard == null) return;
            try
            {
                var result = await _leaderboard.Value.SubmitScoreAsync(score).ConfigureAwait(true);
                if (result.HasValue)
                {
                    DebugManager.Log<SteamLeaderboardService>($"Placar atualizado: {score}");
                }
            }
            catch (Exception ex)
            {
                DebugManager.LogError<SteamLeaderboardService>($"Erro ao Atualizar o placar: {ex.Message}");
            }
        }
    }
}