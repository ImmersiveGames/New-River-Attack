using System;
using System.Threading.Tasks;
using UnityEngine;

namespace RiverAttack
{
    public class LeaderBoardsManager : MonoBehaviour
    {
        [SerializeField] int numRegister;
        [SerializeField] GameObject prefabLine;
        [SerializeField] Transform content;
        void Awake()
        {
            ClearLeaderboard();
        }
        async void OnEnable()
        {
            if (GameSteamManager.connectedToSteam == false) return;
            await CreateListLeaderboard();
        }

        async Task CreateListLeaderboard()
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


        void ClearLeaderboard()
        {
            for (int i = 0; i < content.childCount; i++)
            {
                Destroy(content.GetChild(i).gameObject);
            }
        }
    }
}
