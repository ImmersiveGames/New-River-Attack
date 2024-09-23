using System.Collections;
using System.Threading.Tasks;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.SteamServicesManagers;
using NewRiverAttack.GameStatisticsSystem;
using UnityEngine;

namespace ImmersiveGames.MenuManagers
{
    public class PanelLeaderBoards : MonoBehaviour
    {
        [SerializeField] private int numRegister;
        [SerializeField] private GameObject prefabLine;
        [SerializeField] private Transform content;
        [SerializeField] private GameObject loadingObject; // Objeto de Loading

        #region Unity Methods
        private void Start()
        {
            ClearLeaderboard();
            SteamLeaderboardService.Instance.LoadOfflineScores();
            StartCoroutine(UpdateLeaderboard());
        }
        #endregion

        private IEnumerator UpdateLeaderboard()
        {
            // Ativa o objeto de loading
            loadingObject.SetActive(true);

            // Pequeno atraso para garantir que a UI está carregada
            yield return new WaitForSeconds(0.1f);

            // Executa a tarefa assíncrona em paralelo
            var task = CreateListLeaderboard();

            // Espera pela conclusão da tarefa
            while (!task.IsCompleted)
            {
                yield return null; // Retorna ao final de cada frame até a tarefa completar
            }

            // Desativa o objeto de loading após o carregamento terminar
            loadingObject.SetActive(false);

            // Verifica por exceções
            if (task.IsFaulted)
            {
                Debug.LogError("Erro ao carregar a leaderboard.");
            }
        }

        private async Task CreateListLeaderboard()
        {
            ClearLeaderboard();

            if (SteamConnectionManager.ConnectedToSteam)
            {
                await SteamLeaderboardService.InitializeLeaderboard();
                var scores = await SteamLeaderboardService.Instance.GetScores(numRegister);

                if (scores == null)
                {
                    DebugManager.LogError<PanelLeaderBoards>("Erro ao obter pontuações.");
                    return;
                }

                foreach (var e in scores)
                {
                    var item = Instantiate(prefabLine, content); // Certifique-se de estar na thread principal
                    var card = item.GetComponent<ItemCardDisplayHolder>();
                    if (card != null)
                    {
                        card.Init(e.User.Name, e.Score, e.GlobalRank);
                    }
                }
            }
            else
            {
                DebugManager.LogError<PanelLeaderBoards>("Não conectado à Steam. Exibindo pontuações offline.");
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
