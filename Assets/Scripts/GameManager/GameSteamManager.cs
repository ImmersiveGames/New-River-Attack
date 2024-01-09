using UnityEngine;
using System.Collections;
using Steamworks;
namespace RiverAttack
{
    public class GameSteamManager: MonoBehaviour
    {
        protected Callback<GameOverlayActivated_t> gameOverlayActivated;

        CGameID m_GameID;

        private void OnEnable() {
            if (!SteamManager.Initialized)
                return;
            
            m_GameID = new CGameID(SteamUtils.GetAppID());
            Debug.Log($"TESTE: {m_GameID}");
            /*if (SteamManager.Initialized) {
                gameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
            }*/
            
        }
        
        void Start()
        {
            if (!SteamManager.Initialized)
                return;
            string steamFriendsName = SteamFriends.GetPersonaName();
            Debug.Log(steamFriendsName);
        }
        static void OnGameOverlayActivated(GameOverlayActivated_t pCallback) {
            if(pCallback.m_bActive != 0) {
                Debug.Log("Steam Overlay has been activated");
                GameManager.instance.PauseGame(true);
            }
            else {
                Debug.Log("Steam Overlay has been closed");
                GameManager.instance.PauseGame(false);
            }
        }
    }
}
