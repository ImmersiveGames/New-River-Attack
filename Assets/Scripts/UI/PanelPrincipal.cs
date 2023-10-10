using UnityEngine;
using UnityEngine.SceneManagement;
namespace RiverAttack
{ 
    [RequireComponent(typeof(AudioSource))]
    public class PanelPrincipal : MonoBehaviour
    {

        const int CURSOR_OFFSET_HORIZONTAL = 20;
        [Header("Menus")]
        [SerializeField] Transform menuInicial;
        [SerializeField] Transform[] menuPrincipal;
        [SerializeField] RectTransform cursor;
        
        [Header("Menu Fades")]
        [SerializeField] Animator fadeAnimator;
        [SerializeField]Transform screenWash;
        const float SCREEN_WASH_TIMER = 1f;

        [Header("Menu SFX")]
        [SerializeField] AudioClip clickSound;


        int lastIndex = 0;
        AudioSource m_AudioSource;
        static readonly int FadeIn = Animator.StringToHash("FadeIn");
        static readonly int FadeOut = Animator.StringToHash("FadeOut");

        void Awake()
        {
            m_AudioSource = GetComponent<AudioSource>();
            screenWash.gameObject.SetActive(true);
            menuInicial.gameObject.SetActive(true);
        }

        void OnEnable()
        {
            SetMenuPrincipal();
            lastIndex = 0;
        }

        void Start()
        {
            Invoke(nameof(DeactivateScreenWash),SCREEN_WASH_TIMER);
        }

        void SetInternalMenu(int indexStart = 0)
        {
            if(menuPrincipal.Length < 1) return;
            for (int i = 0; i < menuPrincipal.Length; i++)
            {
                lastIndex = (menuPrincipal[i].gameObject.activeSelf) ? i : 0;
                menuPrincipal[i].gameObject.SetActive(false);
            }
            menuPrincipal[indexStart].gameObject.SetActive(true);
            
            SetSelectGameObject(menuPrincipal[indexStart].gameObject);
        }

        void SetSelectGameObject(GameObject goButton)
        {
            var eventSystemFirstSelect = goButton.GetComponentInChildren<EventSystemFirstSelect>();
            eventSystemFirstSelect.Init();
            //eventSystemFirstSelect.SetCursor(ref cursor, CURSOR_OFFSET_HORIZONTAL);
        }
        
        
        void SetMenuPrincipal()
        {
            menuInicial.gameObject.SetActive(true);
            SetInternalMenu();
        }

        void DeactivateScreenWash()
        {
            screenWash.gameObject.SetActive(false);
        }
        public void PerformFadeOut()
        {
            fadeAnimator.SetTrigger(FadeOut);
        }
        public void PerformFadeIn()
        {
            fadeAnimator.SetTrigger(FadeIn);
        }
        public void PlayClickSfx()
        {
            m_AudioSource.PlayOneShot(clickSound);
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
        public void ButtonModoMission()
        {
            PlayClickSfx();
            PerformFadeOut();
            SceneManager.LoadScene("HUB");
        }
        public void ButtonModoClassic()
        {
            PlayClickSfx();
            PerformFadeOut();
            SceneManager.LoadScene("GamePlay");
        }

        public void ButtonIndexChange(int indexMenu)
        {
            PlayClickSfx();
            SetInternalMenu(indexMenu);
        }
  #endregion
    }
}
