using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.SteamServicesManagers.Interface;
using Steamworks;
using Steamworks.Data;
using UnityEngine;

namespace ImmersiveGames.SteamServicesManagers
{
    public class SteamLeaderboardService : MonoBehaviour, ILeaderboardService
    {
        public static SteamLeaderboardService Instance { get; private set; }
        private static Leaderboard? _leaderboard;
        private const string LeaderboardName = "River_Attack_HiScore";
        private HashSet<int> _offlineScores = new HashSet<int>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                //DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public static async Task InitializeLeaderboard()
        {
            try
            {
                DebugManager.Log<SteamLeaderboardService>("inicializando leaderboard...");
                _leaderboard = await SteamUserStats.FindLeaderboardAsync(LeaderboardName).ConfigureAwait(false);
                if (_leaderboard.HasValue)
                {
                    DebugManager.Log<SteamLeaderboardService>("Leaderboard inicializado com sucesso.");
                }
                else
                {
                    DebugManager.LogError<SteamLeaderboardService>("Falha ao inicializar o leaderboard.");
                }
            }
            catch (Exception e)
            {
                DebugManager.LogError<SteamLeaderboardService>($"Erro ao inicializar o leaderboard {e.Message}");
            }
        }

        public async Task UpdateScore(int score, bool force)
        {
            if (SteamConnectionManager.ConnectedToSteam && _leaderboard.HasValue)
            {
                try
                {
                    // Obtém a pontuação atual do usuário no leaderboard
                    var currentScoreEntry = await _leaderboard.Value.GetScoresAroundUserAsync(1, 1).ConfigureAwait(false);
                    var currentScore = currentScoreEntry?.FirstOrDefault().Score;

                    if (currentScore >= score && !force)
                    {
                        DebugManager.Log<SteamLeaderboardService>($"A nova pontuação ({score}) não é superior à pontuação atual ({currentScore.Value}).");
                        return; // Se a nova pontuação não for maior, não a envia
                    }

                    // Envia a pontuação superior ou força o envio
                    var result = force
                        ? await _leaderboard.Value.ReplaceScore(score).ConfigureAwait(false)
                        : await _leaderboard.Value.SubmitScoreAsync(score).ConfigureAwait(false);

                    if (result.HasValue)
                    {
                        DebugManager.Log<SteamLeaderboardService>($"Pontuação registrada: {result.Value.Score}");
                        await SteamLeaderboardService.Instance.GetScores(10).ConfigureAwait(true); // Atualiza a lista de scores
                    }
                }
                catch (Exception e)
                {
                    DebugManager.LogError<SteamLeaderboardService>($"Erro ao atualizar a pontuação: {e.Message}");
                }
            }
            else
            {
                _offlineScores.Add(score);
                SaveOfflineScores();
            }
        }


        public async Task<LeaderboardEntry[]> GetScores(int quantity)
        {
            if (!SteamConnectionManager.ConnectedToSteam || !_leaderboard.HasValue)
                return null;

            try
            {
                return await _leaderboard.Value.GetScoresAsync(quantity).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                DebugManager.LogError<SteamLeaderboardService>($"Erro ao buscar pontuações {e.Message}");
                return null;
            }
        }

        public Task<LeaderboardEntry[]> GetScoresFromFriends()
        {
            throw new NotImplementedException();
        }

        public Task<LeaderboardEntry[]> GetScoresAround(int start, int end)
        {
            throw new NotImplementedException();
        }

        public void SaveOfflineScores()
        {
            if (_offlineScores != null && _offlineScores.Any())
            {
                PlayerPrefs.SetString("OfflineScores", string.Join(",", _offlineScores));
                PlayerPrefs.Save();
                DebugManager.Log<SteamLeaderboardService>("Pontuações offline salvas.");
            }
            else
            {
                DebugManager.LogWarning<SteamLeaderboardService>("Nenhuma pontuação offline para salvar.");
            }
        }

        public void LoadOfflineScores()
        {
            var savedScores = PlayerPrefs.GetString("OfflineScores", "");
            if (!string.IsNullOrEmpty(savedScores))
            {
                _offlineScores = savedScores.Split(',').Select(int.Parse).ToHashSet();
                DebugManager.Log<SteamLeaderboardService>("Pontuações offline carregadas com sucesso.");
            }
            else
            {
                DebugManager.LogWarning<SteamLeaderboardService>("Nenhuma pontuação offline encontrada.");
            }
        }

        public async void SyncOfflineScores()
        {
            if (!SteamConnectionManager.ConnectedToSteam || !_leaderboard.HasValue || !_offlineScores.Any())
                return;

            DebugManager.Log<SteamLeaderboardService>("Sincronizando pontuações offline...");

            foreach (var score in _offlineScores.ToList())
            {
                await UpdateScore(score, false).ConfigureAwait(true);
            }
            _offlineScores.Clear();
            SaveOfflineScores();
        }

        private void OnApplicationQuit()
        {
            SaveOfflineScores();
        }

        private void OnEnable()
        {
            LoadOfflineScores();
            SyncOfflineScores();
        }
    }
}
