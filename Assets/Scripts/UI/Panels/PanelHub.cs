using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace RiverAttack
{
    public class PanelHub: MonoBehaviour
    {
        [SerializeField] TMP_Text missionName;
        [SerializeField] AudioEvent clickSound;
        [SerializeField] int nextIndex;

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
            nextIndex = m_GameHubManager.missions.FindIndex(x => x.levels == m_GameHubManager.gamePlayingLog.activeMission);
            missionName.text = m_GameHubManager.gamePlayingLog.activeMission.levelName;
            Debug.Log($"Name {m_GameHubManager.gamePlayingLog.activeMission.levelName}");
        }
  #endregion

        public void ButtonNextMission(int increment)
        {
            if (!m_GameHubManager.readyHub) return;
            m_GameHubManager.readyHub = false;
            GameAudioManager.instance.PlaySfx(clickSound);
            nextIndex = GetHubIndex(nextIndex, increment, m_GameHubManager.missions, m_GameHubManager.gamePlayingLog.finishLevels);
            m_GameHubManager.OnChangeMission(nextIndex);
            missionName.text = GamePlayingLog.instance.activeMission.levelName;
            Debug.Log($"Next Index: {nextIndex}");

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

            
            
            //return realIndex > finish.Count ? actual : realIndex;
            //return !finish.Contains(missions[realIndex].levels) ? actual : realIndex;
        }

        /*
        Levels m_ActualLevel;
        bool m_OneClickButton;

        
        GameHubManager m_GameHubManager;
        
        void OnEnable()
        {
            m_GameHubManager = GameHubManager.instance;
            m_InputSystem = new PlayersInputActions();
            m_InputSystem.Enable();
            //TODO: Pegar o level salvo;
            nextIndex = GamePlayingLog.instance.lastMissionIndex;
            Debug.Log($"start index: {nextIndex}");
            m_ActualLevel = m_GameHubManager.missionListLevels.Index(nextIndex);
            missionName.text = m_ActualLevel.levelName;
        }

        void OnDestroy()
        {
            Destroy(m_GameHubManager);
        }

        public void ButtonNextMission(int increment)
        {
            if (!m_GameHubManager.readyHub) return;
            if (m_OneClickButton) return;
            m_OneClickButton = true;
            PlayClickSfx();
            GetRealIndex(increment, ref nextIndex, m_GameHubManager.hubMilestones);
            //Debug.Log(m_GameHubManager.hubIndex);
            m_ActualLevel = m_GameHubManager.missionListLevels.Index(nextIndex);
            missionName.text = m_ActualLevel.levelName;
            m_GameHubManager.OnPlayerPosition(nextIndex);
            m_OneClickButton = false;
        }

        public void ButtonStartMission()
        {
            if (!m_GameHubManager.readyHub) return;
            if (m_OneClickButton) return;
            m_OneClickButton = true;
            PlayClickSfx();
            //Muito cuidado aqui pois precisa verificar
            GamePlayingLog.instance.lastMissionIndex = nextIndex;
            //
            GameManager.instance.ChangeState(new GameStateOpenCutScene(), GameManager.GameScenes.GamePlay.ToString());
        }

        static void GetRealIndex(int increment, ref int nextIndex, IReadOnlyList<float> hubMilestones)
        {
            // Checa o ultimo level finalizado.
            if (increment > 0 && nextIndex >= GamePlayingLog.instance.lastMissionFinishIndex)
                return;
            nextIndex += increment;
            if (nextIndex < 0) nextIndex = 0;
            if (nextIndex >= hubMilestones.Count) nextIndex = hubMilestones.Count - 1;
            // se o valor do milstone não for -1
            while (hubMilestones[nextIndex] < 0)
            {
                //Debug.Log($"z: {hubMilestones[nextIndex]}, {nextIndex}");
                switch (increment)
                {
                    case > 0:
                        nextIndex++;
                        break;
                    case < 0:
                        nextIndex--;
                        break;
                }
                if (nextIndex < 0) nextIndex = 0;
                if (nextIndex >= hubMilestones.Count) nextIndex = hubMilestones.Count - 1;
            }
        }
        public void PlayClickSfx()
        {
            GameAudioManager.instance.PlaySfx(clickSound);
        }
        public void ButtonReturnInitialMenu()
        {
            PlayClickSfx();
            StopAllCoroutines();
            //TODO: desligar tudo para depois mudar a scena.
            GameManager.instance.ChangeState(new GameStateMenu(), GameManager.GameScenes.MainScene.ToString());
        }
        */
    }
}
