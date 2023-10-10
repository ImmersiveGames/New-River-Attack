using System;
using UnityEngine;
using UnityEngine.EventSystems;
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
        
        [Header("Cursor")]
        [SerializeField] RectTransform cursor;
        [SerializeField] Transform[] panelHasCursor;
        
        [Header("Menu Fades")]
        [SerializeField] Animator fadeAnimator;
        [SerializeField]Transform screenWash;
        const float SCREEN_WASH_TIMER = 1f;

        [Header("Menu SFX")]
        [SerializeField] AudioClip clickSound;

        GameObject m_ActualSelect;
        bool hasCursor;
        int m_LastIndex = 0;
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
            m_LastIndex = 0;
        }

        void Start()
        {
            Invoke(nameof(DeactivateScreenWash),SCREEN_WASH_TIMER);
        }

        void Update()
        {
            var button = EventSystem.current.currentSelectedGameObject;
            if (m_ActualSelect == button)
                return;
            m_ActualSelect = button;
            SetCursor(m_ActualSelect.GetComponent<RectTransform>());
            Debug.Log($"Selecionado: {button.name} ");
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
            cursor.gameObject.SetActive(false);
            hasCursor = Array.IndexOf(panelHasCursor, menuPrincipal[indexStart]) != -1;
 
            m_ActualSelect = EventSystem.current.currentSelectedGameObject;
            SetCursor(m_ActualSelect.GetComponent<RectTransform>());
        }

        void SetSelectGameObject(GameObject goButton)
        {
            var eventSystemFirstSelect = goButton.GetComponentInChildren<EventSystemFirstSelect>();
            eventSystemFirstSelect.Init();
        }

        void SetCursor(RectTransform reference)
        {
            if (!hasCursor)
                return;
            cursor.gameObject.SetActive(true);
            var uiCursor = cursor.GetComponent<UiCursor>();
            uiCursor.SetCursor(reference, CURSOR_OFFSET_HORIZONTAL);
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
            SetInternalMenu(m_LastIndex);
        }
        public void ButtonModeMission()
        {
            PlayClickSfx();
            PerformFadeOut();
            SceneManager.LoadScene("HUB");
        }
        public void ButtonModeClassic()
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
