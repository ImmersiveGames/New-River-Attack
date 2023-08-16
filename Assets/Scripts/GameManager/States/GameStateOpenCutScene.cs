using UnityEngine;
using UnityEngine.Playables;
namespace RiverAttack
{
    public class GameStateOpenCutScene: GameState
    {
        const float TOLERANCE = 1f;
        readonly PlayableDirector m_PlayableDirector;
        readonly GameManager m_GameManager;

        internal GameStateOpenCutScene(PlayableDirector playableDirector)
        {
            m_PlayableDirector = playableDirector;
            m_GameManager = GameManager.instance;
        }
        public override void EnterState()
        {
            Debug.Log($"Entra no Estado: CutScene");
            
            m_GameManager.InstantiatePlayers();
            m_GameManager.PlayOpenCutScene();
            m_GameManager.startMenu.SetMenuPrincipal(1,false);
            m_GameManager.startMenu.SetMenuHudControl(false);
        }
        public override void UpdateState()
        {
            Debug.Log($"Rodando no Estado: CutScene");
            if (m_PlayableDirector == null) return;
            
            // Verificar se a animação já terminou
            if (!(m_PlayableDirector.time >= m_PlayableDirector.duration - TOLERANCE))
                return;
            m_GameManager.ChangeState(new GameStatePlayGame());
        }
        public override void ExitState()
        {
            Debug.Log($"Saindo no Estado: CutScene");
        }
    }
}
