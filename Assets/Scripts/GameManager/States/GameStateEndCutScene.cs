using UnityEngine;
using UnityEngine.Playables;
namespace RiverAttack
{
    public class GameStateEndCutScene : GameState
    {
        /*const float TIME_TO_FADE_BGM = 0.1f;
        const float TOLERANCE = 1f;*/
        readonly PlayableDirector m_PlayableDirector;
        readonly GameManager m_GameManager;
        public override void OnLoadState()
        {
            //throw new System.NotImplementedException();
        }
        public override void EnterState()
        {
            Debug.Log($"Enter State: CutScene END");
            PlayerManager.instance.DestroyPlayers();
            GameMissionBuilder.instance.ResetBuildMission();
            GameTimelineManager.instance.endCutDirector.gameObject.SetActive(false);
            
           /*var playerMaster = m_GameManager.initializedPlayerMasters[0];
            playerMaster.gameObject.SetActive(true);
            var playerAnimator = (playerMaster.gameObject).GetComponent<Animator>();
            m_GameManager.endCutDirector.gameObject.SetActive(true);
            m_GameManager.startMenu.SetMenuPrincipal(1, false);
            m_GameManager.startMenu.SetMenuHudControl(false);
            Tools.ChangeBindingReference("Animation Track", playerAnimator, m_GameManager.endCutDirector);
            // Coloca o player como Follow da camra
            //Tools.SetFollowVirtualCam(m_GameManager.virtualCamera, playerMaster.transform);
            //Iniciar a BGM
            GamePlayAudio.instance.ChangeBGM(LevelTypes.Complete, TIME_TO_FADE_BGM);*/
        }

        public override void UpdateState()
        {
            Debug.Log($"Run State: CutScene END");
            /*if (m_PlayableDirector == null) return;

            // Verificar se a animação já terminou
            if (!(m_PlayableDirector.time >= m_PlayableDirector.duration - TOLERANCE))
                return;
            m_GameManager.ChangeState(new GameStatePlayGame());*/
        }
        public override void ExitState()
        {
            //Debug.Log($"Saindo no Estado: CutScene END");
        }
    }
}
