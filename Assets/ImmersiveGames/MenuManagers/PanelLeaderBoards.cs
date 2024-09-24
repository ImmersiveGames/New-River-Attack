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

            // Inicializa o leaderboard (executa o método async como uma tarefa)
            var initializeTask = SteamLeaderboardService.InitializeLeaderboard();

            // Espera pela conclusão da inicialização do leaderboard
            while (!initializeTask.IsCompleted)
            {
                yield return null; // Espera até o final do frame até que a tarefa seja completada
            }

            // Verifica se ocorreu algum erro durante a inicialização
            if (initializeTask.IsFaulted)
            {
                Debug.LogError("Erro ao inicializar o leaderboard.");
                yield break; // Interrompe se houver falha
            }

            // Pequeno atraso para garantir que a UI está carregada
            yield return new WaitForSeconds(0.1f);

            // Executa a tarefa de criar a lista de pontuações
            var createListTask = CreateListLeaderboard();

            // Espera pela conclusão da tarefa
            while (!createListTask.IsCompleted)
            {
                yield return null; // Espera até o final de cada frame
            }

            // Desativa o objeto de loading após a conclusão
            loadingObject.SetActive(false);

            // Verifica por exceções na criação da lista de pontuações
            if (createListTask.IsFaulted)
            {
                Debug.LogError("Erro ao carregar a leaderboard.");
            }
        }

        private async Task CreateListLeaderboard()
        {
            ClearLeaderboard();

            if (SteamConnectionManager.ConnectedToSteam)
            {
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
