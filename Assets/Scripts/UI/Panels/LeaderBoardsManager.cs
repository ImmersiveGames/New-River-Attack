using System;
using System.Threading.Tasks;
using UnityEngine;

namespace RiverAttack
{
    public class LeaderBoardsManager : MonoBehaviour
    {
        [SerializeField] private int numRegister;
        [SerializeField] private GameObject prefabLine;
        [SerializeField] private Transform content;

        private void Awake()
        {
            ClearLeaderboard();
        }

        private async void OnEnable()
        {
            if (GameSteamManager.connectedToSteam == false) return;
            await CreateListLeaderboard();
        }

        private async Task CreateListLeaderboard()
        {
            var globalScores = await GameSteamManager.GetScores( numRegister );
            ClearLeaderboard();
            foreach ( var e in globalScores)
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


        private void ClearLeaderboard()
        {
            for (int i = 0; i < content.childCount; i++)
            {
                Destroy(content.GetChild(i).gameObject);
            }
        }
    }
}
