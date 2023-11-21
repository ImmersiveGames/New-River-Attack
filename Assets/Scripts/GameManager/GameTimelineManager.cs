using System;
using Cinemachine;
using Utils;
using UnityEngine;
using UnityEngine.Playables;
namespace RiverAttack
{
    public class GameTimelineManager : Singleton<GameTimelineManager>
    {
        [Header("Camera Settings"), SerializeField] 
        CinemachineVirtualCamera virtualCamera;
        
        [Header("CutScenes Settings")]
        [SerializeField]
        internal PlayableDirector openCutDirector;
        [SerializeField]
        internal PlayableDirector endCutDirector;

        GameManager m_GameManager;

        void Awake()
        {
            m_GameManager = GameManager.instance;
            openCutDirector.gameObject.SetActive(true);
            endCutDirector.gameObject.SetActive(false);
        }
        protected override void OnDestroy()
        {
            //base.OnDestroy();
        }
        public void InitializePLayerInTimeline(Transform playerTransform, Animator playerAnimator)
        {
            // Atualiza a cutscene com o animator do jogador;
            Tools.ChangeBindingReference("Animation Track", playerAnimator, openCutDirector);
            Tools.ChangeBindingReference("Animation Track", playerAnimator, endCutDirector);
            // Coloca o player como Follow da camra
            Tools.SetFollowVirtualCam(virtualCamera, playerTransform);
        }

        public void EndOpenCutScene()
        {
            Debug.Log("Fim da Time Line");
            m_GameManager.ChangeState(new GameStatePlayGame());
        }
        
        public void CompletePathEndCutScene()
        {
            GameAudioManager.instance.ChangeBGM(LevelTypes.Complete, 0.1f);
            endCutDirector.gameObject.SetActive(true);
            //endCutDirector.Play();
            var panelMenuGame = m_GameManager.PanelBase<PanelMenuGame>();
            if (panelMenuGame != null)
            {
                panelMenuGame.SetMenuEndPath();
            }
            Invoke(nameof(ChangeEndGame), 3f);
        }
        
        void ChangeEndGame()
        {
            if(m_GameManager.gameModes == GameManager.GameModes.Classic)
                m_GameManager.ChangeState(new GameStateEndGame(), GameManager.GameScenes.EndGameCredits.ToString());
            if(m_GameManager.gameModes == GameManager.GameModes.Mission)
                m_GameManager.ChangeState(new GameStateHub(), GameManager.GameScenes.MissionHub.ToString());
        }
    }
}
