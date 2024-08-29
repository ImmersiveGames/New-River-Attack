using System.Threading.Tasks;
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
        

        #region Unity Methods
        private void Start()
        {
            ClearLeaderboard();
            SteamLeaderboardService.Instance.LoadOfflineScores();
        }

        #endregion

        public async void OnEnable()
        {
            await CreateListLeaderboard().ConfigureAwait(false);
        }

        private async Task CreateListLeaderboard()
        {
            ClearLeaderboard();
            if (SteamConnectionManager.ConnectedToSteam)
            {
                await SteamLeaderboardService.InitializeLeaderboard().ConfigureAwait(false);
                
                var scores = await SteamLeaderboardService.Instance.GetScores(numRegister).ConfigureAwait(true);
            
                foreach ( var e in scores)
                {
                    var item = Instantiate(prefabLine, content);
                    var card = item.GetComponent<ItemCardDisplayHolder>();
                    if (card != null)
                    {
                        card.Init(e.User.Name,e.Score, e.GlobalRank);
                    }
                    //Debug.Log($"{e.GlobalRank}: {e.Score} {e.User.Name}" );
                }
            }
        }
        
        private void ClearLeaderboard()
        {
            if(content.childCount <= 0) return;
            for (var i = 0; i < content.childCount; i++)
            {
                Destroy(content.GetChild(i).gameObject);
            }
        }
    }
}
