using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
namespace RiverAttack
{ 
    [RequireComponent(typeof(AudioSource))]
    public class PanelPrincipal : MonoBehaviour
    {
        [Header("Menus")]
        [SerializeField] Transform menuInitial;
        [SerializeField] Transform[] menuPrincipal;

        [Header("Menu Fades")]
        [SerializeField] Animator fadeAnimator;
        [SerializeField]Transform screenWash;
        const float SCREEN_WASH_TIMER = 1f;

        [Header("Menu SFX")]
        [SerializeField] AudioClip clickSound;
        
        int m_LastIndex = 0;
        AudioSource m_AudioSource;
        static readonly int FadeIn = Animator.StringToHash("FadeIn");
        static readonly int FadeOut = Animator.StringToHash("FadeOut");

        void Awake()
        {
            m_AudioSource = GetComponent<AudioSource>();
            screenWash.gameObject.SetActive(true);
            menuInitial.gameObject.SetActive(true);
        }

        void OnEnable()
        {
            SetMenuPrincipal();
            m_LastIndex = 0;
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
                m_LastIndex = (menuPrincipal[i].gameObject.activeSelf) ? i : 0;
                menuPrincipal[i].gameObject.SetActive(false);
            }
            var selectPanel = menuPrincipal[indexStart].gameObject;
            selectPanel.SetActive(true);
            SetSelectGameObject(selectPanel);
        }

        void SetSelectGameObject(GameObject goButton)
        {
            var eventSystemFirstSelect = goButton.GetComponentInChildren<EventSystemFirstSelect>();
            eventSystemFirstSelect.Init();
        }
        
        
        void SetMenuPrincipal()
        {
            menuInitial.gameObject.SetActive(true);
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
            SetInternalMenu(m_LastIndex);
        }
        public void ButtonModeMission()
        {
            PlayClickSfx();
            PerformFadeOut();
            Invoke(nameof(LoadSceneHub),SCREEN_WASH_TIMER);
            
        }
        public void ButtonModeClassic()
        {
            PlayClickSfx();
            PerformFadeOut();
            Invoke(nameof(LoadSceneGamePlay),SCREEN_WASH_TIMER);
        }
        void LoadSceneHub()
        {
            SceneManager.LoadScene("HUB");
        }
        void LoadSceneGamePlay()
        {
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
