using System.Transactions;
using UnityEngine;
namespace RiverAttack
{
    public class GameStateMenu : GameState
    {
        readonly PanelManager m_PanelManager;
        const float TIME_TO_FADE_BGM = 0.2f;
        internal GameStateMenu(PanelManager panelManager)
        {
            m_PanelManager = panelManager;
        }

        public override void EnterState()
        {
            //Aplicar Fade
            //Muda Para Musica De Menu
            GamePlayAudio.instance.ChangeBGM(LevelTypes.Menu, TIME_TO_FADE_BGM);
            //Pausa o Jogo.
            //Abre A UI
            Debug.Log($"Entra no Estado: Menu");
            m_PanelManager.ResetStartMenu();
        }
        public override void UpdateState()
        {
            Debug.Log($"Rodando no Estado: Menu");
        }
        public override void ExitState()
        {
            //Despausa o Jogo
            //Instancia os Players
            var gameManager = GameManager.instance;
            gameManager.InstantiatePlayers();
            var gamePlayManager = GamePlayManager.instance;
            gamePlayManager.PauseGame();
            //Aplicar Fade
            Debug.Log($"Saindo no Estado: Menu");
        }
    }
}
