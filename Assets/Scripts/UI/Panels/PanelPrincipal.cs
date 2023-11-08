using UnityEngine;
using UnityEngine.Serialization;
namespace RiverAttack
{
    [RequireComponent(typeof(AudioSource))]
    public class PanelPrincipal : PanelBase
    {
        [Header("Animation")]
        [SerializeField] protected TimeLineManager timelineManager;
        [Header("Menu Fades")]
        [SerializeField] Transform screenWash;
        const float SCREEN_WASH_TIMER = 1f;
        GameManager m_GameManager;
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
        void OnEnable()
        {

            SetMenuPrincipal();
            lastIndex = 0;
        }
        void Start()
        {
            Invoke(nameof(DeactivateScreenWash), SCREEN_WASH_TIMER);
        }
  #endregion

        protected override void SetInternalMenu(int indexStart = 0)
        {
            base.SetInternalMenu(indexStart);
            SwitchCamera(indexStart);
        }
        void DeactivateScreenWash()
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

 
  #endregion
    }
}
