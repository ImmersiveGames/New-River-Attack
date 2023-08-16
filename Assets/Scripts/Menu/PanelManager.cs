using System;
using System.Collections.Generic;
using UnityEngine;

namespace RiverAttack
{
    public class PanelManager : MonoBehaviour
    {
        [SerializeField]
        Transform menuParent,menuFade;
        [SerializeField]
        List<Transform> menuPrincipal = new List<Transform>();
        [SerializeField]
        int startIndex;
        [SerializeField]
        Transform menuHud, menuControl;
        GameManager m_GameManager;
        GamePlayAudio m_GamePlayAudio;
        AudioSource m_AudioSource;
        Animator m_Animator;
        static readonly int FadeIn = Animator.StringToHash("FadeOut");
        static readonly int FadeOut = Animator.StringToHash("FadeIn");

        #region UNITYMETHODS
        void Awake()
        {
            m_AudioSource = GetComponent<AudioSource>();
            m_Animator = GetComponent<Animator>();
        }
        void OnEnable()
        {
            m_GameManager = GameManager.instance;
            m_GamePlayAudio = GamePlayAudio.instance;
        }
        #endregion

        public void SetMenuHudControl(bool active)
        {
            menuHud.gameObject.SetActive(active);
            menuControl.gameObject.SetActive(active);
            //menuFade.gameObject.SetActive(false);
        }

        public void SetMenuPrincipal(int indexStart, bool active)
        {
            menuParent.gameObject.SetActive(active);
            foreach (var t in menuPrincipal)
            {
                t.gameObject.SetActive(false);
            }
            if(active)
                menuPrincipal[indexStart].gameObject.SetActive(true);
            //menuFade.gameObject.SetActive(false);
        }

        public void PerformFadeOut()
        {
            m_Animator.SetTrigger( FadeOut);
        }
        public void PerformFadeIn()
        {
            m_Animator.SetTrigger( FadeIn);
        }

        #region Buttons Actions

        public void ButtonGamePause()
        {
            m_GameManager.PauseGame();
        }
        public void ButtonGameUnPause()
        {
            m_GameManager.UnPauseGame();
        }
        public void ButtonQuitApplication()
        {
            Application.Quit();
        }
        public void PlayClickSfx()
        {
            m_GamePlayAudio.PlayClickSfx(m_AudioSource);
        }
        #endregion
        
        /*GameSettings m_GameSettings;
        GameManager m_GameManager;
        [SerializeField] List<Transform> panelsMenus;
        [SerializeField] bool firstMenuStartEnable = true;
        [SerializeField] PanelBackButton panelBackButton;
        [SerializeField] List<int> navegationMenu;
        [SerializeField] int menuIndexActive;

        void Awake()
        {
            ClearMenu();
            if (!firstMenuStartEnable) return;
            panelsMenus[0].gameObject.SetActive(true);
            navegationMenu.Add(0);
        }
        void Start()
        {
            m_GameSettings = GameSettings.instance;
            m_GameManager = GameManager.instance;
        }

        void ClearMenu()
        {
            foreach (var child in panelsMenus)
            {
                child.gameObject.SetActive(false);
            }
        }

        void ActiveMenuIndex(int index)
        {
            if (index >= 0 && index < transform.childCount)
            {
                transform.GetChild(index).gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Invalid child object index!");
            }
        }

        #region ButtonActions
        
        public void ButtonStartGame()
        {
            ClearMenu();
            m_GameManager.ChangeStatesGamePlay(GameManager.States.WaitGamePlay);
        }
  #endregion*/
    }
}
