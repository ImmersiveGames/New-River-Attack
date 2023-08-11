using UnityEngine;
namespace RiverAttack
{
    public class GameStateOpenCutScene: GameState
    {
        const float TIME_TO_FADE_BGM = 0.2f;
        readonly PanelManager m_PanelManager;
        internal GameStateOpenCutScene(PanelManager panelManager)
        {
            m_PanelManager = panelManager;
        }
        public override void EnterState()
        {
            // Muda Para Musica De CutScvene
            GamePlayAudio.instance.ChangeBGM(LevelTypes.Grass, TIME_TO_FADE_BGM);
            m_PanelManager.StartGameHUD();
            var gameManager = GameManager.instance;
            gameManager.InstantiatePlayers();

            //Fazer o Setup inicial do Player e zera-lo para um novo jogo.
            Debug.Log($"Entra no Estado: CutScene");
        }
        public override void UpdateState()
        {
            Debug.Log($"Rodando no Estado: CutScene");
        }
        public override void ExitState()
        {
            Debug.Log($"Saindo no Estado: CutScene");
        }
    }
}
