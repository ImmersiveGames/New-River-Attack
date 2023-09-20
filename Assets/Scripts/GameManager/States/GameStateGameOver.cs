using UnityEngine;
namespace RiverAttack
{
    public class GameStateGameOver : GameState
    {
        const float TIME_TO_FADE_BGM = 0.1f;
        readonly GameManager m_GameManager;
        readonly GamePlayAudio m_GamePlayAudio;

        public GameStateGameOver()
        {
            m_GameManager = GameManager.instance;
            m_GamePlayAudio = GamePlayAudio.instance;
        }
        public override void EnterState()
        {
            Debug.Log($"Entra no Estado: GameOver");
            m_GamePlayAudio.ChangeBGM(LevelTypes.GameOver, TIME_TO_FADE_BGM);
            
        }

        public void GameOverState()
        {
            m_GameManager.startMenu.SetMenuGameOver();
            m_GamePlayAudio.PlayVoice(m_GamePlayAudio.missionFailSound);
        }
        
        public override void UpdateState()
        {
            Debug.Log($"GameOver");
        }
        public override void ExitState()
        {
            Debug.Log($"Saida no Estado: GameOver");
            m_GameManager.RemoveAllPlayers();
            
        }
    }
}
