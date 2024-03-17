using System.Collections.Generic;
using ImmersiveGames;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RiverAttack
{
    public class PanelHub: MonoBehaviour
    {
        [SerializeField] private TMP_Text missionName;
        [SerializeField] private AudioEvent clickSound;
        private int _nextIndex;

        private PlayersInputActions _inputActions;
        private GameHubManager _gameHubManager;

        private bool _pushButtonStart;

        #region UNITYMETHODS
        private void OnEnable()
        {
            SetControllersInput();
            _gameHubManager = GameHubManager.instance;
        }

        private void Start()
        {
            _gameHubManager.readyHub = true;
            _nextIndex = _gameHubManager.missions.FindIndex(x => x.levels == _gameHubManager.gamePlayingLog.activeMission);
            missionName.text = _gameHubManager.gamePlayingLog.activeMission.levelName;
            //Debug.Log($"Name {_gameHubManager.gamePlayingLog.activeMission.levelName}");
        }

        private void OnDisable()
        {
            _inputActions.UiControls.StartButton.performed -= ButtonStartMission;
            _inputActions.UiControls.BackButton.performed -= ButtonReturnInitialMenu;
            _inputActions.UiControls.LeftSelection.performed -= LastMission;
            _inputActions.UiControls.RightSelection.performed -= NextMission;
        }
        #endregion

        private void SetControllersInput()
        {
            _inputActions = GameManager.instance.inputSystem;
            _inputActions.UiControls.StartButton.performed += ButtonStartMission;
            _inputActions.UiControls.BackButton.performed += ButtonReturnInitialMenu;
            _inputActions.UiControls.LeftSelection.performed += LastMission;
            _inputActions.UiControls.RightSelection.performed += NextMission;
        }

        public void ButtonNextMission(int increment)
        {
            if (!_gameHubManager.readyHub || _pushButtonStart) return;
            _pushButtonStart = true;
            //_gameHubManager.readyHub = false;
            GameAudioManager.instance.PlaySfx(clickSound);
            _nextIndex = GetHubIndex(_nextIndex, increment, _gameHubManager.missions, _gameHubManager.gamePlayingLog.GetLevelsResult());
            _gameHubManager.OnChangeMission(_nextIndex);
            missionName.text = GamePlayingLog.instance.activeMission.levelName;
            _pushButtonStart = false;
            //Debug.Log($"Next Index: {_nextIndex}");
        }

        private void NextMission(InputAction.CallbackContext context)
        {
            ButtonNextMission(1);
        }
        private void LastMission(InputAction.CallbackContext context)
        {
            ButtonNextMission(-1);
        }

        private void ButtonStartMission(InputAction.CallbackContext context)
        {
            ButtonStartMission();
        }
        public void ButtonStartMission()
        {
            if (!_gameHubManager.readyHub || _pushButtonStart) return;
            _pushButtonStart = true;
            GameAudioManager.instance.PlaySfx(clickSound);
            if(_gameHubManager.gamePlayingLog.activeMission.bossFight){}
            GameManager.instance.ChangeState(new GameStateOpenCutScene(), _gameHubManager.gamePlayingLog.activeMission.bossFight ? 
                GameManager.GameScenes.GamePlayBoss.ToString() : GameManager.GameScenes.GamePlay.ToString());
            _pushButtonStart = false;
        }

        private void ButtonReturnInitialMenu(InputAction.CallbackContext context)
        {
            ButtonReturnInitialMenu();
        }
        public void ButtonReturnInitialMenu()
        {
            if (!_gameHubManager.readyHub || _pushButtonStart) return;
            _pushButtonStart = true;
            GameAudioManager.instance.PlaySfx(clickSound);
            StopAllCoroutines();
            GameManager.instance.ChangeState(new GameStateMenu(), GameManager.GameScenes.MainScene.ToString());
            _pushButtonStart = false;
        }

        private static int GetHubIndex(int actual, int increment, IReadOnlyList<HubMissions> missions, ICollection<Levels> finish)
        {
            var realIndex = actual + increment;
            var max = missions.Count;
            if (realIndex >= max) return max - 1;
            if (realIndex <= 0) return 0;
            if(finish.Contains(missions[realIndex].levels)) return realIndex;
            return !finish.Contains(missions[realIndex].levels) && finish.Contains(missions[actual].levels) ? realIndex :
            actual;
        }
        
    }
}
