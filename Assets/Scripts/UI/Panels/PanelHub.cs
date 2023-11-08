using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace RiverAttack
{
    public class PanelHub: MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI missionName;
        [SerializeField] AudioEvent clickSound;
        [SerializeField] int nextIndex;
        Levels m_ActualLevel;
        bool m_OneClickButton;

        GameHubManager m_GameHubManager;
        void Awake()
        {
            m_GameHubManager = GameHubManager.instance;
        }
        void OnEnable()
        {
            //TODO: Pegar o level salvo;
            nextIndex = GamePlayingLog.instance.lastMissionIndex;
            Debug.Log($"start index: {nextIndex}");
            m_ActualLevel = m_GameHubManager.missionListLevels.Index(nextIndex);
            m_ActualLevel.levelsStates = LevelsStates.Actual;
            missionName.text = m_ActualLevel.levelName;
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
    }
}
