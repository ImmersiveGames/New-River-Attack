using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace RiverAttack
{
    public class PanelHub: MonoBehaviour
    {
        [SerializeField] TMP_Text missionName;
        [SerializeField] AudioEvent clickSound;
        int m_NextIndex;

        PlayersInputActions m_InputSystem;
        GameHubManager m_GameHubManager;

        #region UNITYMETHODS
        void OnEnable()
        {
            m_InputSystem = new PlayersInputActions();
            m_InputSystem.Enable();
            m_GameHubManager = GameHubManager.instance;
        }

        void Start()
        {
            m_GameHubManager.readyHub = true;
            m_NextIndex = m_GameHubManager.missions.FindIndex(x => x.levels == m_GameHubManager.gamePlayingLog.activeMission);
            missionName.text = m_GameHubManager.gamePlayingLog.activeMission.levelName;
            //Debug.Log($"Name {m_GameHubManager.gamePlayingLog.activeMission.levelName}");
        }
  #endregion

        public void ButtonNextMission(int increment)
        {
            if (!m_GameHubManager.readyHub) return;
            m_GameHubManager.readyHub = false;
            GameAudioManager.instance.PlaySfx(clickSound);
            m_NextIndex = GetHubIndex(m_NextIndex, increment, m_GameHubManager.missions, m_GameHubManager.gamePlayingLog.finishLevels);
            m_GameHubManager.OnChangeMission(m_NextIndex);
            missionName.text = GamePlayingLog.instance.activeMission.levelName;
            //Debug.Log($"Next Index: {m_NextIndex}");
        }
        public void ButtonStartMission()
        {
            if (!m_GameHubManager.readyHub) return;
            m_GameHubManager.readyHub = false;
            GameAudioManager.instance.PlaySfx(clickSound);
            GameManager.instance.ChangeState(new GameStateOpenCutScene(), GameManager.GameScenes.GamePlay.ToString());
        }
        public void ButtonReturnInitialMenu()
        {
            GameAudioManager.instance.PlaySfx(clickSound);
            StopAllCoroutines();
            GameManager.instance.ChangeState(new GameStateMenu(), GameManager.GameScenes.MainScene.ToString());
        }

        static int GetHubIndex(int actual, int increment, IReadOnlyList<HubMissions> missions, ICollection<Levels> finish)
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
