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
                DebugManager.Log<SteamLeaderboardService>("inicializando..");
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
                DebugManager.LogError<SteamLeaderboardService>($"Erro ao inicializar o leaderboard {e}");
            }
        }

        public async Task UpdateScore(int score, bool force)
        {
            if (SteamConnectionManager.ConnectedToSteam && _leaderboard.HasValue)
            {
                try
                {
                    var result = force
                        ? await _leaderboard.Value.ReplaceScore(score).ConfigureAwait(false)
                        : await _leaderboard.Value.SubmitScoreAsync(score).ConfigureAwait(false);

                    if (result.HasValue)
                    {
                        DebugManager.Log<SteamLeaderboardService>($"Score registrado: {result.Value.Score}");
                    }
                }
                catch (Exception e)
                {
                    DebugManager.LogError<SteamLeaderboardService>($"Erro ao atualizar a pontuação {e}");
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
                DebugManager.LogError<SteamLeaderboardService>($"Erro ao buscar pontuações {e}");
                return null;
            }
        }

        public async Task<LeaderboardEntry[]> GetScoresFromFriends()
        {
            if (!SteamConnectionManager.ConnectedToSteam || !_leaderboard.HasValue)
                return null;

            try
            {
                return await _leaderboard.Value.GetScoresFromFriendsAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                DebugManager.LogError<SteamLeaderboardService>($"Erro ao buscar pontuações de amigos {e}");
                return null;
            }
        }

        public async Task<LeaderboardEntry[]> GetScoresAround(int start, int end)
        {
            if (!SteamConnectionManager.ConnectedToSteam || !_leaderboard.HasValue)
                return null;

            try
            {
                return await _leaderboard.Value.GetScoresAroundUserAsync(start, end).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                DebugManager.LogError<SteamLeaderboardService>($"Erro ao buscar pontuações ao redor do usuário {e}");
                return null;
            }
        }

        public void SaveOfflineScores()
        {
            PlayerPrefs.SetString("OfflineScores", string.Join(",", _offlineScores));
            PlayerPrefs.Save();
        }

        public async void SyncOfflineScores()
        {
            if (!SteamConnectionManager.ConnectedToSteam || !_leaderboard.HasValue || !_offlineScores.Any())
                return;

            foreach (var score in _offlineScores.ToList())
            {
                await UpdateScore(score, false).ConfigureAwait(false);
            }
            _offlineScores.Clear();
            SaveOfflineScores();
        }

        public void LoadOfflineScores()
        {
            var savedScores = PlayerPrefs.GetString("OfflineScores", "");
            if (!string.IsNullOrEmpty(savedScores))
            {
                _offlineScores = savedScores.Split(',').Select(int.Parse).ToHashSet();
            }
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
