using UnityEngine;
namespace RiverAttack
{
    public class GameStateOpenCutScene : GameState
    {
        const float TIME_TO_FADE_BGM = 0.1f;
        public override void OnLoadState()
        {
            var startLevel = GameManager.instance.GetLevel(0);
            var startBgm = startLevel.bgmStartLevel;
            GameAudioManager.instance.ChangeBGM(startBgm, TIME_TO_FADE_BGM);
        }
        public override void EnterState()
        {
            GameMissionBuilder.instance.StartBuildMission(GamePlayManager.instance.actualLevels);
            PlayerManager.instance.InstantiatePlayers();
            Debug.Log($"Entra no Estado: CutScene");
            GameTimelineManager.instance.openCutDirector.Play();
        }

        public override void UpdateState()
        {
            if (GameManager.instance.loadSceneFinish) return;
            Debug.Log($"CutScene!");
        }
        public override void ExitState()
        {
            Debug.Log($"Sai do Estado: CutScene");
        }

        


        /*const float TIME_TO_FADE_BGM = 0.1f;
        const float TOLERANCE = 1f;
        readonly PlayableDirector m_PlayableDirector;
        readonly GameManager m_GameManager;
        readonly GamePlayManager m_GamePlayManager;

        internal GameStateOpenCutScene(PlayableDirector playableDirector)
        {
            m_PlayableDirector = playableDirector;
            m_GameManager = GameManager.instance;
            m_GamePlayManager = GamePlayManager.instance;
        }
        public override void EnterState()
        {
            //Debug.Log($"Entra no Estado: CutScene");
            m_GameManager.openCutDirector.gameObject.SetActive(true);
            if (!m_GamePlayManager.haveAnyPlayerInitialized)
                m_GamePlayManager.InstantiatePlayers();
            //m_GameManager.PlayOpenCutScene();
            //Iniciar a BGM
            GameAudioManager.instance.ChangeBGM(GamePlayManager.instance.actualLevels.bgmStartLevel, TIME_TO_FADE_BGM);
            //m_GameManager.startMenu.SetMenuPrincipal();
            //m_GameManager.startMenu.SetMenuHudControl(false);
        }

        public override void UpdateState()
        {
           // Debug.Log($"Rodando no Estado: CutScene");
            if (m_PlayableDirector == null) return;

            // Verificar se a animação já terminou
            if (!(m_PlayableDirector.time >= m_PlayableDirector.duration - TOLERANCE))
                return;
            m_GameManager.ChangeState(new GameStatePlayGame());
        }
        public override void ExitState()
        {
            //Debug.Log($"Saindo no Estado: CutScene");
        }*/

    }
}
