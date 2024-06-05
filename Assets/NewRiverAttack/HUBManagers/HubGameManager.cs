using System.Collections;
using System.Collections.Generic;
using ImmersiveGames.Utils;
using NewRiverAttack.GameManagers;
using NewRiverAttack.HUBManagers.UI;
using NewRiverAttack.SaveManagers;
using UnityEngine;

namespace NewRiverAttack.HUBManagers
{
    public sealed class HubGameManager : Singleton<HubGameManager>
    {
        [Header("HUB Icon Color")] public readonly Color LockedColor = Color.red;
        public readonly Color ActualColor = new Color(255, 255, 0, 255);
        public readonly Color CompleteColor = Color.green;
        public readonly Color OpenColor = Color.white;

        internal bool IsHubReady { get; set; }
        internal List<HubOrderData> LevelOrder = new List<HubOrderData>();
        public int IndexMax { get; private set; }
        private const float WaitBridge = 1.5f;
        private const float WaitMove = 1f;

        private UiHubPlayer _playerHub;

        #region Delegates & Events
        
        public delegate void HubInitializationHandler(List<HubOrderData> listHubOrderData, int startIndex);
        public event HubInitializationHandler EventInitializeHub;
        public event HubInitializationHandler EventCursorUpdateHub;

        #endregion
        

        #region Unity Methods

        private void Start()
        {
            IsHubReady = false;
            StartCoroutine(WaitForInitialization());
        }

        private void OnDisable()
        {
            LevelOrder = new List<HubOrderData>();
        }

        #endregion

        private IEnumerator WaitForInitialization()
        {
            while (!GameManager.StateManager.GetCurrentState().StateFinalization)
            {
                yield return null;
            }
            IsHubReady = true;
        }
        

        #region Calls

        internal void OnEventInitializeHub()
        {
            IndexMax = GameOptionsSave.instance.activeIndexMissionLevel;
            EventInitializeHub?.Invoke(LevelOrder, IndexMax);
            if (LevelOrder[IndexMax].levelData.hudPath.levelsStates == LevelsStates.Complete)
            {
                OnEventCompleteLevel();
            }
        }

        private void OnEventCompleteLevel()
        {
            IsHubReady = false;
            LevelOrder[IndexMax].levelData.hudPath.levelsStates = LevelsStates.Open;
            Invoke(nameof(PlayAnimationBridge), WaitBridge);
            
        }

        private void PlayAnimationBridge()
        {
            var indexOrder = GameOptionsSave.instance.activeIndexMissionLevel;
            var bridge = LevelOrder[indexOrder].bridge;
            bridge.ExplodeBridge();
            Invoke(nameof(UpdateComplete), WaitMove);
        }

        private void UpdateComplete()
        {
            IndexMax = GameOptionsSave.instance.activeIndexMissionLevel += 1;
            OnEventCursorUpdateHub(IndexMax);
            IsHubReady = true;
        }

        #endregion

        internal void OnEventCursorUpdateHub(int startIndex)
        {
            EventCursorUpdateHub?.Invoke(LevelOrder, startIndex);
        }
    }
}