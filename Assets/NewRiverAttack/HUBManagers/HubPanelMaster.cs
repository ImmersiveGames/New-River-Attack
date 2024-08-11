using System.Collections.Generic;
using ImmersiveGames;
using NewRiverAttack.AudioManagers;
using NewRiverAttack.GameManagers;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.HUBManagers.UI;
using NewRiverAttack.SaveManagers;
using NewRiverAttack.StateManagers;
using UnityEngine;
using UnityEngine.UI;
namespace NewRiverAttack.HUBManagers
{
    public class HubPanelMaster : MonoBehaviour
    {
        public Button forwardButton;
        public Button backwardButton;
        public Button startButton;

        public UiHubPlayer cursor;

        private int _indexMenu;
        
        private HubGameManager _hubGameManager;
        private GameManager _gameManager;
        private void OnEnable()
        {
            SetInitialReferences();
            _hubGameManager.EventInitializeHub += SetIndexMenu;
            _hubGameManager.EventCursorUpdateHub += SetIndexMenu;
        }

        private void OnDisable()
        {
            _hubGameManager.EventInitializeHub -= SetIndexMenu;
            _hubGameManager.EventCursorUpdateHub -= SetIndexMenu;
        }

        private void SetIndexMenu(List<HubOrderData> listHubOrderData, int startIndex)
        {
            _indexMenu = startIndex;
        }


        private void SetInitialReferences()
        {
            _hubGameManager = HubGameManager.instance;
            _gameManager = GameManager.instance;

        }
        public void ButtonNavigation(bool back)
        {
            if(!_hubGameManager.IsHubReady) return;
            var increment = (back) ? -1 : 1;
            if(_indexMenu + increment > GameOptionsSave.instance.activeIndexMissionLevel) return;
            _indexMenu += increment;
            if (_indexMenu < 0) _indexMenu = 0;
            _hubGameManager.OnEventCursorUpdateHub(_indexMenu);
            
        }

        public async void ButtonStartMission()
        {
            var audioGameOver = AudioManager.instance.GetAudioSfxEvent(EnumSfxSound.SfxMouseClick);
            audioGameOver.PlayOnShot(GetComponent<AudioSource>());
            _gameManager.gamePlayMode = GamePlayModes.MissionMode;
            _gameManager.ActiveIndex = _indexMenu;
            _gameManager.ActiveLevel = _hubGameManager.LevelOrder[_indexMenu].levelData;
            await GameManager.StateManager.ChangeStateAsync(StatesNames.GameStatePlay.ToString()).ConfigureAwait(false);
        }
        
        public async void ButtonMenuInicial()
        {
            var audioGameOver = AudioManager.instance.GetAudioSfxEvent(EnumSfxSound.SfxMouseClick);
            audioGameOver.PlayOnShot(GetComponent<AudioSource>());
            _gameManager.gamePlayMode = GamePlayModes.MissionMode;
            await GameManager.StateManager.ChangeStateAsync(StatesNames.GameStateMenuInitial.ToString()).ConfigureAwait(false);
        }
    }
}