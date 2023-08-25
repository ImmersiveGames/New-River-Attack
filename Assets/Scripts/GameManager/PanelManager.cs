using System.Collections.Generic;
using UnityEngine;

namespace RiverAttack
{
    public class PanelManager : MonoBehaviour
    {
        [SerializeField]
        Transform menuParent, menuFade;
        [SerializeField]
        List<Transform> menuPrincipal = new List<Transform>();
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
            if (active)
                menuPrincipal[indexStart].gameObject.SetActive(true);
            //menuFade.gameObject.SetActive(false);
        }

        public void PerformFadeOut()
        {
            m_Animator.SetTrigger(FadeOut);
        }
        public void PerformFadeIn()
        {
            m_Animator.SetTrigger(FadeIn);
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
        
    }
}
