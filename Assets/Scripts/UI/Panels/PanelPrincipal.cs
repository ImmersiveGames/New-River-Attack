﻿using UnityEngine;
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
            m_GameManager = GameManager.instance;
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
            m_GameManager.PerformFadeOut();
        }
        public void ButtonModeClassic()
        {
            PlayClickSfx();
            m_GameManager.PerformFadeOut();
            m_GameManager.ChangeState(new GameStateOpenCutScene());
        }
        
        public void ButtonIndexChange(int indexMenu)
        {
            PlayClickSfx();
            SetInternalMenu(indexMenu);
        }
  #endregion
    }
}