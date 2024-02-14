using UnityEngine;
using Cinemachine;
namespace RiverAttack
{
    [RequireComponent(typeof(AudioSource))]
    public class PanelPrincipal : PanelBase
    {
        [SerializeField] private CinemachineVirtualCameraBase[] menuCamera;
        [Header("Animation")]
        [SerializeField] protected TimeLineManager timelineManager;
        [Header("Menu Fades")]
        [SerializeField]
        private Transform screenWash;

        private const float SCREEN_WASH_TIMER = 1f;
        private GameManager m_GameManager;
#region UNITYMETHODS
        protected override void Awake()
        {
            base.Awake();
            m_GameManager = GameManager.instance;
            if (!m_GameManager.panelBaseGame)
            {
                m_GameManager.panelBaseGame = this;
            }
            menuInitial.gameObject.SetActive(true);
            screenWash.gameObject.SetActive(true);
            m_GameManager.panelFade.gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            SetMenuPrincipal();
            lastIndex = 0;
        }

        private void Start()
        {
            Invoke(nameof(DeactivateScreenWash), SCREEN_WASH_TIMER);
        }
#endregion

private void SwitchCamera(int cameraIndex) 
        {
            foreach (var virtualCam in menuCamera) 
            {
                virtualCam.Priority = 0;
                virtualCam.gameObject.SetActive(false);
            }

            menuCamera[cameraIndex].Priority = 10;            
            menuCamera[cameraIndex].gameObject.SetActive(true);            
        }
        protected override void SetInternalMenu(int indexStart = 0)
        {
            base.SetInternalMenu(indexStart);
            SwitchCamera(indexStart);
        }

        private void DeactivateScreenWash()
        {
            screenWash.gameObject.SetActive(false);
            PlayAnimation(0f);
        }
        
        
        #region Buttons
        public void PlayAnimation(float animStartTime)
        {
            timelineManager.PlayAnimation(animStartTime);
        }
        public void ButtonExit()
        {
            PlayClickSfx();
            Application.Quit();
        }

        public void ButtonModeMission()
        {
            PlayClickSfx();
            m_GameManager.gameModes = GameManager.GameModes.Mission;
            m_GameManager.ChangeState(new GameStateHub(), GameManager.GameScenes.MissionHub.ToString());
        }
        public void ButtonModeClassic()
        {
            PlayClickSfx();
            m_GameManager.gameModes = GameManager.GameModes.Classic;
            m_GameManager.ChangeState(new GameStateOpenCutScene(), GameManager.GameScenes.GamePlay.ToString());
        }
        public void ButtonBriefingRoom()
        {
            PlayClickSfx();
            m_GameManager.ChangeState(new GameStateTutorial(), GameManager.GameScenes.BriefingRoom.ToString());
        }
        #endregion
    }
}
