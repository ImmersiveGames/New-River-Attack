using UnityEngine;
namespace RiverAttack
{ 
    [RequireComponent(typeof(AudioSource))]
    public class PanelPrincipal : PanelBase
    {
        [Header("Menu Fades")]
        [SerializeField]Transform screenWash;
        const float SCREEN_WASH_TIMER = 1f;
        GameManager m_GameManager;
#region UNITYMETHODS
        protected override void Awake()
        {
            base.Awake();
            menuInitial.gameObject.SetActive(true);
            screenWash.gameObject.SetActive(true);
            panelFade.gameObject.SetActive(true);
        }
        void OnEnable()
        {
            m_GameManager = GameManager.instance;
            SetMenuPrincipal();
            lastIndex = 0;
        }
        void Start()
        {
            Invoke(nameof(DeactivateScreenWash),SCREEN_WASH_TIMER);
        }
  #endregion
        void DeactivateScreenWash()
        {
            screenWash.gameObject.SetActive(false);
        }

        #region Buttons
        public void ButtonExit()
        {
            PlayClickSfx();
            Application.Quit();
        }

        public void ButtonBack()
        {
            PlayClickSfx();
            SetInternalMenu(lastIndex);
        }
        public void ButtonModeMission()
        {
            PlayClickSfx();
            PerformFadeOut();
            Invoke(nameof(LoadSceneHub),fadeOutTime);
        }
        public void ButtonModeClassic()
        {
            PlayClickSfx();
            PerformFadeOut();
            m_GameManager.ChangeState(new GameStateOpenCutScene());
            //Invoke(nameof(LoadSceneGamePlay),fadeOutTime);
        }
        
        public void ButtonIndexChange(int indexMenu)
        {
            PlayClickSfx();
            SetInternalMenu(indexMenu);
        }
  #endregion
    }
}
