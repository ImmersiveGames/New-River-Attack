using System.Collections;
using System.Collections.Generic;
using ImmersiveGames.DebugManagers;
using NewRiverAttack.GameManagers;
using NewRiverAttack.HUBManagers.UI;
using NewRiverAttack.SaveManagers;
using NewRiverAttack.StateManagers;
using UnityEngine;

namespace NewRiverAttack.HUBManagers
{
    public sealed class HubGameManager : MonoBehaviour
    {
        [Header("HUB Icon Color")] public readonly Color LockedColor = Color.red;
        public readonly Color ActualColor = new Color(255, 255, 0, 255);
        public readonly Color CompleteColor = Color.green;
        public readonly Color OpenColor = Color.white;

        internal bool IsHubReady { get; set; }
        internal List<HubOrderData> LevelOrder = new List<HubOrderData>();
        private int _lastSave;
        public int actualIndex;
        
        private const float WaitBridge = 1.5f;
        private const float WaitMove = 1f;

        private UiHubPlayer _playerHub;
        public static HubGameManager Instance { get; private set; }
        

        #region Delegates & Events
        
        public delegate void HubInitializationHandler(List<HubOrderData> listHubOrderData, int startIndex);
        public event HubInitializationHandler EventInitializeHub;
        public event HubInitializationHandler EventCursorUpdateHub;

        #endregion
        

        #region Unity Methods
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DebugManager.Log<HubGameManager>("HubGameManager instanciado.");
            }
            else
            {
                Destroy(gameObject);
                DebugManager.LogWarning<HubGameManager>("Tentativa de criar uma segunda instância de SteamStatsService foi evitada.");
            }
        }
        private void Start()
        {
            IsHubReady = false;
            _lastSave = GameOptionsSave.Instance.activeIndexMissionLevel;
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
        
        private void PlayAnimationBridge()
        {
            var indexOrder = GameOptionsSave.Instance.activeIndexMissionLevel;
            var bridge = LevelOrder[indexOrder].bridge;
            bridge.ExplodeBridge();
            Invoke(nameof(UpdateComplete), WaitMove);
        }

        private async void UpdateComplete()
        {
            var lastIndex = LevelOrder.Count - 1;
            if (actualIndex < lastIndex)
            {
                _lastSave = GameOptionsSave.Instance.activeIndexMissionLevel += 1;
                OnEventCursorUpdateHub(_lastSave);
            }
            IsHubReady = true;
            if (actualIndex != LevelOrder.Count - 1) return;
            await GameManager.StateManager.ChangeStateAsync(StatesNames.GameStateEndGame.ToString()).ConfigureAwait(false);
        }

        #region Calls

        internal void OnEventInitializeHub()
        {
            actualIndex = GameManager.instance.ActiveIndex;
            if (GameManager.instance.ActiveLevel == null)
            {
                actualIndex = _lastSave;
            }
            EventInitializeHub?.Invoke(LevelOrder, actualIndex);
            //Debug.Log($"Index: {actualIndex}");
            if (actualIndex != _lastSave) return;
            if (LevelOrder[_lastSave].levelData.hudPath.levelsStates == LevelsStates.Complete)
            {
                OnEventCompleteLevel();
            }
        }

        private void OnEventCompleteLevel()
        {
            IsHubReady = false;
            LevelOrder[_lastSave].levelData.hudPath.levelsStates = LevelsStates.Open;
            Invoke(nameof(PlayAnimationBridge), WaitBridge);
            
        }
        internal void OnEventCursorUpdateHub(int startIndex)
        {
            EventCursorUpdateHub?.Invoke(LevelOrder, startIndex);
        }
        #endregion

        
    }
}