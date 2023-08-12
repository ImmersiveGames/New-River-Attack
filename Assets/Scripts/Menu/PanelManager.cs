using System;
using System.Collections.Generic;
using UnityEngine;

namespace RiverAttack
{
    public class PanelManager : MonoBehaviour
    {
        [SerializeField]
        Transform menuParent;
        [SerializeField]
        List<Transform> menuPrincipal = new List<Transform>();
        [SerializeField]
        int startIndex;
        [SerializeField]
        Transform menuHud, menuControl;
        GameManager m_GameManager;
        GamePlayAudio m_GamePlayAudio;
        AudioSource m_AudioSource;

        #region UNITYMETHODS
        void Awake()
        {
            m_AudioSource = GetComponent<AudioSource>();
        }
        void OnEnable()
        {
            m_GameManager = GameManager.instance;
            m_GamePlayAudio = GamePlayAudio.instance;
            ResetStartMenu();
            
        }
        #endregion

        public void ResetStartMenu()
        {
            menuParent.gameObject.SetActive(true);
            menuHud.gameObject.SetActive(false);
            menuControl.gameObject.SetActive(false);
            foreach (var t in menuPrincipal)
            {
                t.gameObject.SetActive(false);
            }
            menuPrincipal[startIndex].gameObject.SetActive(true);
        }

        public void StartGameHUD()
        {
            foreach (var t in menuPrincipal)
            {
                t.gameObject.SetActive(false);
            }
            menuParent.gameObject.SetActive(false);
            menuHud.gameObject.SetActive(true);
            menuControl.gameObject.SetActive(true);
        }

        public void PauseGameMenu(int menuPauseIndex)
        {
            GamePlayManager.instance.PauseGame();
            menuParent.gameObject.SetActive(true);
            menuHud.gameObject.SetActive(false);
            menuControl.gameObject.SetActive(false);
            foreach (var t in menuPrincipal)
            {
                t.gameObject.SetActive(false);
            }
            menuPrincipal[menuPauseIndex].gameObject.SetActive(true);
        }
        
        public void UnPauseGameMenu()
        {
            GamePlayManager.instance.UnPauseGame();
            StartGameHUD();
        }
        
        #region Buttons Actions
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
