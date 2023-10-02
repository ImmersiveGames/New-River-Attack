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
            m_GameManager.startMenu.menuPreFade.gameObject.SetActive(false);
        }

        public override void EnterState()
        {
            //Debug.Log($"Entra no Estado: Menu");
            GamePlayAudio.instance.ChangeBGM(LevelTypes.Menu, TIME_TO_FADE_BGM);
            m_GameManager.startMenu.SetMenuPrincipal(0, true);
            m_GameManager.startMenu.SetMenuHudControl(false);
        }
        public override void UpdateState()
        {
           // Debug.Log($"Rodando no Estado: Menu");
        }
        public override void ExitState()
        {
            //Debug.Log($"Saindo no Estado: Menu");
        }
    }
}
