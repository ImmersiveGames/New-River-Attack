using UnityEngine;
using UnityEngine.Playables;
namespace RiverAttack
{
    public class GameStateOpenCutScene: GameState
    {
        const float TIME_TO_FADE_BGM = 0.2f;
        const float TOLERANCE = 0.08f;
        readonly PlayableDirector m_PlayableDirector;
        readonly GameManager m_GameManager;
   
        internal GameStateOpenCutScene(PlayableDirector playableDirector)
        {
            m_PlayableDirector = playableDirector;
            m_GameManager = GameManager.instance;
        }
        public override void EnterState()
        {
            m_GameManager.PlayOpenCutScene();
            m_GameManager.startMenu.SetMenuPrincipal(1,false);
            m_GameManager.startMenu.SetMenuHudControl(false);
            
            Debug.Log($"Entra no Estado: CutScene");
        }
        public override void UpdateState()
        {
            if (m_PlayableDirector == null)
            {
                Debug.LogWarning("O PlayableDirector não foi atribuído.");
                return;
            }
            // Verificar se a animação já terminou
            if (!(m_PlayableDirector.time >= m_PlayableDirector.duration - TOLERANCE))
                return;
            m_GameManager.ChangeState(new GameStatePlayGame());
            //Debug.Log($"Rodando no Estado: CutScene {m_PlayableDirector.time} e {m_PlayableDirector.duration}");
        }
        public override void ExitState()
        {
            Debug.Log($"Saindo no Estado: CutScene");
        }
    }
}
