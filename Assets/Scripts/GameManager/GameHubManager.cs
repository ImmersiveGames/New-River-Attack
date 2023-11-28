using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;
namespace RiverAttack
{
    public sealed class GameHubManager : Singleton<GameHubManager>
    {
        [SerializeField] internal List<HubMissions> missions;
        [SerializeField] public GamePlayingLog gamePlayingLog;

        internal bool readyHub;
        
        #region Delagetes
        public delegate void HubEventHandler(int index);
        internal event HubEventHandler ChangeMission;
        #endregion
        void Start()
        {
            if (gamePlayingLog.activeMission != null)
                return;
            gamePlayingLog.activeMission = missions[0].levels;
            gamePlayingLog.finishLevels = new List<Levels>();
        }

        void SetActualLevel(int index)
        {
            gamePlayingLog.activeMission.levelsStates = gamePlayingLog.finishLevels.Contains(gamePlayingLog.activeMission) ? LevelsStates.Open : LevelsStates.Locked;
            missions[index].levels.levelsStates = LevelsStates.Actual;
            gamePlayingLog.activeMission = missions[index].levels;
        }

        internal void OnChangeMission(int index)
        {
            SetActualLevel(index);
            ChangeMission?.Invoke(index);
        }
        
        
        /*
         [SerializeField] internal ListLevels missionListLevels;
        [SerializeField] internal List<float> hubMilestones;
        

        [SerializeField] PanelHub panelHub;
        [SerializeField] GamePlayingLog gamePlayingLog;
        public bool readyHub;
        
        
    #region UNITYMETHODS
        void Start()
        {
            GameManager.instance.gameModes = GameManager.GameModes.Mission;
            if(!missionListLevels)
                missionListLevels = GameManager.instance.missionLevels;
            if (GameManager.instance.currentGameState is not GameStateHub)
            {
                GameManager.instance.ChangeState(new GameStateHub());
            }
        }

        void OnDisable()
        {
            readyHub = false;
        }
        protected override void OnDestroy(){ }
        
  #endregion

        #region Delagetes
        
        public delegate void HubEventNormalHandler();
        internal event HubEventNormalHandler CompleteLevel;
        public delegate void HubEventHandler(int index);
        internal event HubEventHandler MissionIndex;
  #endregion
        
        

        internal void MissionNextLevel()
        {
            if (gamePlayingLog.lastMissionIndex + 1 == hubMilestones.Count)
            {
                Debug.Log($"Não tem proxima fase");
                return;
            }
            gamePlayingLog.lastMissionIndex++;
            gamePlayingLog.lastMissionFinishIndex++;
            panelHub.ButtonNextMission(+1);
        }
        
        internal void OnCheckCompleteLevel()
        {
            //Debug.Log($"Checar se Algum Nivel foi completo");
            CompleteLevel?.Invoke();
        }
        
        internal void OnPlayerPosition(int index)
        {
            MissionIndex?.Invoke(index);
        }
        */
    }
    [System.Serializable]
    class HubMissions
    {
        public float position;
        public Levels levels;
    }
}

