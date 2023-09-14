using UnityEngine;
namespace RiverAttack
{
    public class GameStateGameOver : GameState
    {
        const float TIME_TO_FADE_BGM = 0.1f;
        readonly GameManager m_GameManager;
        
        public GameStateGameOver()
        {
            m_GameManager = GameManager.instance;
        }
        public override void EnterState()
        {
            Debug.Log($"Entra no Estado: GameOver");
            GamePlayAudio.instance.ChangeBGM(LevelTypes.GameOver, TIME_TO_FADE_BGM);
            m_GameManager.startMenu.SetMenuGameOver();
        }
        public override void UpdateState()
        {
            Debug.Log($"GameOver");
        }
        public override void ExitState()
        {
            Debug.Log($"Saida no Estado: GameOver");
        }
    }
}
