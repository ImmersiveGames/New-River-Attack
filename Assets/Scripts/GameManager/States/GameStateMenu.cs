using UnityEngine;
namespace RiverAttack
{
    public class GameStateMenu : GameState
    {
        const float TIME_TO_FADE_BGM = 0.01f;

        public override void EnterState()
        {
            //Debug.Log($"Entra no Estado: Menu");
            GameAudioManager.instance.ChangeBGM(LevelTypes.Menu, TIME_TO_FADE_BGM);
            //m_GameManager.startMenu.SetMenuPrincipal();
            //m_GameManager.startMenu.SetMenuHudControl(false);
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
