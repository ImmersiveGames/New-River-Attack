using System;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.SteamServicesManagers;
using NewRiverAttack.GameStatisticsSystem;
using Steamworks.Data;
using UnityEngine;

namespace ImmersiveGames.MenuManagers
{
    public class PanelLeaderBoards : MonoBehaviour
    {
        [SerializeField] private string leaderboardName = "River_Attack_HiScore";
        [SerializeField] private int numRegister;
        [SerializeField] private GameObject prefabLine;
        [SerializeField] private Transform content;
        [SerializeField] private GameObject loadingObject;
        private Leaderboard? _leaderboard;

        private void Awake()
        {
            SteamLeaderboardService.Init(leaderboardName);
            _leaderboard = SteamLeaderboardService.Leaderboard;
        }

        private void OnEnable()
        {
            ClearLeaderboard();
            CreateGlobalBoard(numRegister);
        }

        private void OnDisable()
        {
            SteamLeaderboardService.DestroyBoard();
        }

        private async void CreateGlobalBoard(int numTotal)
        {
            if (_leaderboard == null) return;

            loadingObject.SetActive(true);
            try
            {
                //var globalScores = await _leaderboard.Value.GetScoresAsync(numTotal).ConfigureAwait(true);
                var aroundUserAsync = await _leaderboard.Value.GetScoresAroundUserAsync(-numTotal,numTotal).ConfigureAwait(true);
                
                if (aroundUserAsync != null)
                {
                    foreach (var entry in aroundUserAsync)
                    {
                        var item = Instantiate(prefabLine, content);
                        item.SetActive(true);
                        var holder = item.GetComponent<ItemCardDisplayHolder>();
                        holder.Init(entry.User, entry.Score, entry.GlobalRank);
                        //Debug.Log($"Usuário: {entry.User}, Pontuação: {entry.Score}, Posição: {entry.GlobalRank}");
                    }
                }
                else
                {
                    DebugManager.LogError<PanelLeaderBoards>("Leaderboard não foi inicializado ou não há dados disponíveis.");
                }
            }
            catch (Exception ex)
            {
                DebugManager.LogError<PanelLeaderBoards>($"Erro ao carregar o leaderboard: {ex.Message}");
            }
            finally
            {
                loadingObject.SetActive(false);
            }
        }
        private void ClearLeaderboard()
        {
            if (content.childCount <= 0) return;
            for (var i = 0; i < content.childCount; i++)
            {
                Destroy(content.GetChild(i).gameObject);
            }
        }
    }
}
