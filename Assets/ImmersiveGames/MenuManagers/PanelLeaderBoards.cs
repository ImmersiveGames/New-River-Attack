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
            // Carrega as pontuações offline ao iniciar
            SteamLeaderboardService.Instance.LoadOfflineScores();
        }

        private void OnEnable()
        {
            // Sempre que o painel for habilitado, atualiza a leaderboard
            StartCoroutine(UpdateLeaderboard());
        }

        #endregion

        private IEnumerator UpdateLeaderboard()
        {
            // Ativa o objeto de loading
            loadingObject.SetActive(true);

            // Garante que o leaderboard será sempre inicializado
            var initializeTask = SteamLeaderboardService.InitializeLeaderboard();

            // Espera pela inicialização
            while (!initializeTask.IsCompleted)
            {
                yield return null;
            }

            // Verifica se a inicialização falhou
            if (initializeTask.IsFaulted)
            {
                Debug.LogError("Erro ao inicializar o leaderboard.");
                yield break;
            }

            // Pequeno atraso para garantir que a UI está pronta
            yield return new WaitForSeconds(0.1f);

            // Carrega a lista de pontuações
            var createListTask = CreateListLeaderboard();
            while (!createListTask.IsCompleted)
            {
                yield return null;
            }

            // Desativa o loading
            loadingObject.SetActive(false);
        }

        private async Task CreateListLeaderboard()
        {
            ClearLeaderboard();

            if (SteamConnectionManager.ConnectedToSteam)
            {
                var scores = await SteamLeaderboardService.Instance.GetScores(numRegister).ConfigureAwait(true);

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
