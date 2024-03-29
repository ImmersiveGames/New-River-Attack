﻿using UnityEngine;
using UnityEngine.UI;
namespace RiverAttack
{
    public class UiHubIcons: MonoBehaviour
    {
        [SerializeField] public Levels level;
        Image m_MissionIcon;
        
        [Header("HUB Icon Color")]
        public Color lockedColor = new Color(164,31,31,255);
        public Color actualColor = new Color(255,255,0,255);
        public Color completeColor= new Color(12,149,0,255);
        public Color openColor= new Color(255,255,255,255);

        #region UNITYMETHODS
        void OnEnable()
        {
            m_MissionIcon = GetComponentInChildren<Image>();
            GameHubManager.instance.ChangeMission += SetUpMission;
        }
        
        void Start()
        {
            m_MissionIcon.color = SetColorStates(level.levelsStates);
        }
  #endregion

        internal void Initialization(Levels levels)
        {
            level = levels;
            if (levels.levelsStates == LevelsStates.Complete && GameHubManager.instance.gamePlayingLog.finishLevels.Contains(levels)) return;
            level.levelsStates = LevelsStates.Locked;
            if (GameHubManager.instance.gamePlayingLog.finishLevels.Contains(levels))
            {
                level.levelsStates = LevelsStates.Open;
            }
            if (GameHubManager.instance.gamePlayingLog.activeMission == levels)
            {
                level.levelsStates = LevelsStates.Actual;
            }
            SetUpMission(0);
        }
        void SetUpMission(int index)
        {
            m_MissionIcon.color = SetColorStates(level.levelsStates);
        }
        Color SetColorStates(LevelsStates levelsStates)
        {
            return levelsStates switch
            {
                LevelsStates.Locked => lockedColor,
                LevelsStates.Actual => actualColor,
                LevelsStates.Complete => completeColor,
                LevelsStates.Open => openColor,
                _ => lockedColor
            };
        }
    }
}
