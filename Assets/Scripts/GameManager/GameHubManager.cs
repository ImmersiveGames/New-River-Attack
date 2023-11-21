using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;
namespace RiverAttack
{
    public sealed class GameHubManager : Singleton<GameHubManager>
    {
        [SerializeField] internal ListLevels missionListLevels;
        [SerializeField] internal List<float> hubMilestones;
        [SerializeField] PanelHub panelHub;
        [SerializeField] GamePlayingLog gamePlayingLog;
        public bool readyHub;
        [Header("HUB Icon Color")]
        public Color lockedColor;
        public Color actualColor;
        public Color completeColor;
        public Color openColor;
        
    #region UNITYMETHODS
        void Awake()
        {
            GameManager.instance.gameModes = GameManager.GameModes.Mission;
            
            if(!missionListLevels)
                missionListLevels = GameManager.instance.missionLevels;
        }
        
        void Start()
        {
            if (GameManager.instance.currentGameState is not GameStateHub)
            {
                GameManager.instance.ChangeState(new GameStateHub());
            }
        }
        protected override void OnDestroy()
        {
            //base.OnDestroy();
        }
        
  #endregion

        #region Delagetes
        
        public delegate void HubEventNormalHandler();
        internal event HubEventNormalHandler CompleteLevel;
        public delegate void HubEventHandler(int index);
        internal event HubEventHandler MissionIndex;
        /*public delegate void IconEventHandler(LevelsStates states);
        internal event IconEventHandler ChangeState;*/
  #endregion
        
        public Color SetColorStates(LevelsStates levelsStates)
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

        internal void MissionNextLevel()
        {
            gamePlayingLog.lastMissionIndex++;
            gamePlayingLog.lastMissionFinishIndex++;
            panelHub.ButtonNextMission(+1);
        }
        
        internal void OnCheckCompleteLevel()
        {
            Debug.Log($"Checar se Algum Nivel foi completo");
            CompleteLevel?.Invoke();
        }
        
        internal void OnPlayerPosition(int index)
        {
            MissionIndex?.Invoke(index);
        }

        /*internal void OnChangeState(LevelsStates states)
        {
            ChangeState?.Invoke(states);
        }*/
    }
}
