using System.Collections.Generic;
using ImmersiveGames;
using NewRiverAttack.AudioManagers;
using NewRiverAttack.GameManagers;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.HUBManagers.UI;
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

        }
        public void ButtonNavigation(bool back)
        {
            if(!_hubGameManager.IsHubReady) return;
            var increment = (back) ? -1 : 1;
            if(_indexMenu + increment > _hubGameManager.IndexMax) return;
            _indexMenu += increment;
            if (_indexMenu < 0) _indexMenu = 0;
            _hubGameManager.OnEventCursorUpdateHub(_indexMenu);
            
        }

        public async void ButtonStartMission()
        {
            var audioGameOver = AudioManager.GetAudioSfxEvent(EnumSfxSound.SfxMouseClick);
            audioGameOver.PlayOnShot(GetComponent<AudioSource>());
            GameManager.instance.gamePlayMode = GamePlayModes.MissionMode;
            GameManager.instance.ActiveLevel = _hubGameManager.LevelOrder[_indexMenu].levelData;
            await GameManager.StateManager.ChangeStateAsync(StatesNames.GameStatePlay.ToString()).ConfigureAwait(false);
        }
        
        public async void ButtonMenuInicial()
        {
            var audioGameOver = AudioManager.GetAudioSfxEvent(EnumSfxSound.SfxMouseClick);
            audioGameOver.PlayOnShot(GetComponent<AudioSource>());
            GameManager.instance.gamePlayMode = GamePlayModes.MissionMode;
            await GameManager.StateManager.ChangeStateAsync(StatesNames.GameStateMenuInitial.ToString()).ConfigureAwait(false);
        }
    }
}