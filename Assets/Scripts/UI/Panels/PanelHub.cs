using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace RiverAttack
{
    public class PanelHub: MonoBehaviour
    {
        [SerializeField] private TMP_Text missionName;
        [SerializeField] private AudioEvent clickSound;
        private int m_NextIndex;

        private PlayersInputActions m_InputSystem;
        private GameHubManager m_GameHubManager;

        private bool m_PushButtonStart;

        #region UNITYMETHODS

        private void Awake()
        {
            m_InputSystem = new PlayersInputActions();
            //m_InputSystem.UI_Controlls.Disable();
            //m_InputSystem.Player.Enable();
        }

        private void OnEnable()
        {
            SetControllersInput();
            m_GameHubManager = GameHubManager.instance;
        }

        private void Start()
        {
            m_GameHubManager.readyHub = true;
            m_NextIndex = m_GameHubManager.missions.FindIndex(x => x.levels == m_GameHubManager.gamePlayingLog.activeMission);
            missionName.text = m_GameHubManager.gamePlayingLog.activeMission.levelName;
            //Debug.Log($"Name {m_GameHubManager.gamePlayingLog.activeMission.levelName}");
        }

        private void OnDisable()
        {
            //m_InputSystem.UI_Controlls.Disable();
        }
        #endregion

        private void SetControllersInput()
        {
            //m_InputSystem.UI_Controlls.Enable();
            m_InputSystem.UI_Controlls.StartButton.performed += _ => ButtonStartMission();
            m_InputSystem.UI_Controlls.BackButton.performed += _ => ButtonReturnInitialMenu();
            m_InputSystem.UI_Controlls.LeftSelection.performed += _ => ButtonNextMission(-1);
            m_InputSystem.UI_Controlls.RightSelection.performed += _ => ButtonNextMission(1);
        }

        public void ButtonNextMission(int increment)
        {
            if (!m_GameHubManager.readyHub || m_PushButtonStart) return;
            m_PushButtonStart = true;
            //m_GameHubManager.readyHub = false;
            GameAudioManager.instance.PlaySfx(clickSound);
            m_NextIndex = GetHubIndex(m_NextIndex, increment, m_GameHubManager.missions, m_GameHubManager.gamePlayingLog.finishLevels);
            m_GameHubManager.OnChangeMission(m_NextIndex);
            missionName.text = GamePlayingLog.instance.activeMission.levelName;
            m_PushButtonStart = false;
            //Debug.Log($"Next Index: {m_NextIndex}");
        }
        public void ButtonStartMission()
        {
            if (!m_GameHubManager.readyHub || m_PushButtonStart) return;
            m_PushButtonStart = true;
            GameAudioManager.instance.PlaySfx(clickSound);
            if(m_GameHubManager.gamePlayingLog.activeMission.bossFight){}
            GameManager.instance.ChangeState(new GameStateOpenCutScene(), m_GameHubManager.gamePlayingLog.activeMission.bossFight ? 
                GameManager.GameScenes.GamePlayBoss.ToString() : GameManager.GameScenes.GamePlay.ToString());
            m_PushButtonStart = false;
        }
        public void ButtonReturnInitialMenu()
        {
            if (!m_GameHubManager.readyHub || m_PushButtonStart) return;
            m_PushButtonStart = true;
            GameAudioManager.instance.PlaySfx(clickSound);
            StopAllCoroutines();
            GameManager.instance.ChangeState(new GameStateMenu(), GameManager.GameScenes.MainScene.ToString());
            m_PushButtonStart = false;
        }

        private static int GetHubIndex(int actual, int increment, IReadOnlyList<HubMissions> missions, ICollection<Levels> finish)
        {
            int realIndex = actual + increment;
            int max = missions.Count;
            if (realIndex >= max) return max - 1;
            if (realIndex <= 0) return 0;
            if(finish.Contains(missions[realIndex].levels)) return realIndex;
            return !finish.Contains(missions[realIndex].levels) && finish.Contains(missions[actual].levels) ? realIndex :
            actual;
        }
        
    }
}
