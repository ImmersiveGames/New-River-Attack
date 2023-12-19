using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;
namespace RiverAttack
{
    public sealed class GameHubManager : Singleton<GameHubManager>
    {
        [SerializeField] internal List<HubMissions> missions;
        public GamePlayingLog gamePlayingLog;
        [SerializeField] PanelHub panelHub;

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
        protected override void OnDestroy()
        {
            //base.OnDestroy();
        }

        void SetActualLevel(int index)
        {
            gamePlayingLog.activeMission.levelsStates = gamePlayingLog.finishLevels.Contains(gamePlayingLog.activeMission) ? LevelsStates.Open : LevelsStates.Locked;
            missions[index].levels.levelsStates = LevelsStates.Actual;
            gamePlayingLog.activeMission = missions[index].levels;
        }
        internal void CheckNextLevel()
        {
            //Debug.Log($"Verifica se existe uma proxima missão ou termina o jogo");
            if (gamePlayingLog.activeMission == missions[^1].levels)
            {
                // Fim de jogo - chamar o fim do jogo
                return;
            }
            panelHub.ButtonNextMission(+1);
        }

        internal void OnChangeMission(int index)
        {
            SetActualLevel(index);
            ChangeMission?.Invoke(index);
        }
    }
    [Serializable]
    class HubMissions
    {
        public float position;
        public Levels levels;
    }
}

