
using System.Collections;
using UnityEngine;
namespace RiverAttack
{
    public class GameStateGameOver : GameState
    {
        const float TIME_TO_FADE_BGM = 0.1f;
        readonly GameAudioManager m_GameAudioManager;
        readonly GamePlayManager m_GamePlayManager;
        readonly PlayerManager m_PlayerManager;

        public GameStateGameOver()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_GameAudioManager = GameAudioManager.instance;
            m_PlayerManager = PlayerManager.instance;
            
        }
        public override IEnumerator OnLoadState()
        {
            //throw new System.NotImplementedException();
            m_GameAudioManager.ChangeBGM(LevelTypes.GameOver, TIME_TO_FADE_BGM);
            m_GameAudioManager.PlayVoice(m_GameAudioManager.missionFailSound);
            yield return null;
        }
        public override void EnterState()
        {
            Debug.Log($"Entra no Estado: GameOver");
            m_GamePlayManager.GameOverMenu();
        }
        public override void UpdateState()
        {
            Debug.Log($"GameOver");
        }
        public override void ExitState()
        {
            //Debug.Log($"Saida no Estado: GameOver");
            m_PlayerManager.RemoveAllPlayers();
            m_GamePlayManager.OnEventEnemiesMasterForceRespawn();
        }
    }
}
