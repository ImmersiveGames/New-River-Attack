﻿using UnityEngine;
namespace RiverAttack
{ 
    [RequireComponent(typeof(AudioSource))]
    public class PanelPrincipal : PanelBase
    {
        const string NAME_SCENE_GAMEPLAY = "GamePlay";
        const string NAME_SCENE_HUD = "HUD";
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
            m_GameManager.gameModes = GameManager.GameModes.Mission;
            m_GameManager.ChangeState(new GameStateHub(), NAME_SCENE_HUD);
        }
        public void ButtonModeClassic()
        {
            PlayClickSfx();
            m_GameManager.gameModes = GameManager.GameModes.Classic;
            m_GameManager.ChangeState(new GameStateOpenCutScene(), NAME_SCENE_GAMEPLAY);
        }
        
        public void ButtonIndexChange(int indexMenu)
        {
            PlayClickSfx();
            SetInternalMenu(indexMenu);
        }
  #endregion
    }
}
