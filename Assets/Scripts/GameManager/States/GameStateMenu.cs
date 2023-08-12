using UnityEngine;
namespace RiverAttack
{
    public class GameStateMenu : GameState
    {
        const float TIME_TO_FADE_BGM = 0.2f;
        readonly GameManager m_GameManager;
        internal GameStateMenu()
        {
            m_GameManager = GameManager.instance;
        }

        public override void EnterState()
        {
            var gamePlayManager = GamePlayManager.instance;
            gamePlayManager.PauseGame();
            GamePlayAudio.instance.ChangeBGM(LevelTypes.Menu, TIME_TO_FADE_BGM);

            Debug.Log($"Entra no Estado: Menu");
            m_GameManager.startMenu.ResetStartMenu();
        }
        public override void UpdateState()
        {
            Debug.Log($"Rodando no Estado: Menu");
        }
        public override void ExitState()
        {
            GamePlayAudio.instance.ChangeBGM(LevelTypes.Grass, TIME_TO_FADE_BGM);
            m_GameManager.InstantiatePlayers();

            Debug.Log($"Saindo no Estado: Menu");
        }
    }
}
