using System.Collections.Generic;
using ImmersiveGames;
using ImmersiveGames.InputManager;
using NewRiverAttack.AudioManagers;
using NewRiverAttack.GameManagers;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.HUBManagers.UI;
using NewRiverAttack.SaveManagers;
using NewRiverAttack.StateManagers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
namespace NewRiverAttack.HUBManagers
{
    public class HubPanelMaster : MonoBehaviour
    {
        public Button forwardButton;
        public Button backwardButton;
        public Button startButton;
        public Button backButton;

        public UiHubPlayer cursor;

        private int _indexMenu;

        private bool _inInvoke;
        
        private HubGameManager _hubGameManager;
        private GameManager _gameManager;
        private void OnEnable()
        {
            SetInitialReferences();
            _inInvoke = false;
            InputGameManager.ActionManager.ActivateActionMap(ActionManager.GameActionMaps.UiControls);
            InputGameManager.RegisterAction("StartButton", ButtonStartMission );
            InputGameManager.RegisterAction("BackButton", BackToMenu );
            InputGameManager.RegisterAction("RightSelection", GoRight );
            InputGameManager.RegisterAction("LeftSelection", GoLeft );
            _hubGameManager.EventInitializeHub += SetIndexMenu;
            _hubGameManager.EventCursorUpdateHub += SetIndexMenu;
        }
        
        private void GoLeft(InputAction.CallbackContext obj)
        {
            if(_inInvoke) return;
            /*if (EventSystem.current != null)
            {
                // Seleciona o botão especificado
                EventSystem.current.SetSelectedGameObject(backwardButton.gameObject);
            }*/
            backwardButton.onClick.Invoke();
        }
        private void GoRight(InputAction.CallbackContext obj)
        {
            if(_inInvoke) return;
            /*if (EventSystem.current != null)
            {
                // Seleciona o botão especificado
                EventSystem.current.SetSelectedGameObject(forwardButton.gameObject);
            }*/
            forwardButton.onClick.Invoke();
        }

        private void BackToMenu(InputAction.CallbackContext obj)
        {
            if(_inInvoke) return;
            if (EventSystem.current != null)
            {
                // Seleciona o botão especificado
                EventSystem.current.SetSelectedGameObject(backButton.gameObject);
            }
            backButton.onClick.Invoke();
        }

        private void ButtonStartMission(InputAction.CallbackContext obj)
        {
            if(_inInvoke) return;
            if (EventSystem.current != null)
            {
                // Seleciona o botão especificado
                EventSystem.current.SetSelectedGameObject(startButton.gameObject);
            }
            startButton.onClick.Invoke();
        }

        private void OnDisable()
        {
            _hubGameManager.EventInitializeHub -= SetIndexMenu;
            _hubGameManager.EventCursorUpdateHub -= SetIndexMenu;
            InputGameManager.ActionManager.RestoreActionMap();
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
            _inInvoke = true;
            _indexMenu += increment;
            if (_indexMenu < 0) _indexMenu = 0;
            _hubGameManager.OnEventCursorUpdateHub(_indexMenu);
            _inInvoke = false;
        }

        public async void ButtonStartMission()
        {
            _inInvoke = true;
            var audioGameOver = AudioManager.instance.GetAudioSfxEvent(EnumSfxSound.SfxMouseClick);
            audioGameOver.PlayOnShot(GetComponent<AudioSource>());
            _gameManager.gamePlayMode = GamePlayModes.MissionMode;
            _gameManager.ActiveIndex = _indexMenu;
            _gameManager.ActiveLevel = _hubGameManager.LevelOrder[_indexMenu].levelData;
            await GameManager.StateManager.ChangeStateAsync(StatesNames.GameStatePlay.ToString()).ConfigureAwait(false);
        }
        
        public async void ButtonMenuInicial()
        {
            _inInvoke = true;
            var audioGameOver = AudioManager.instance.GetAudioSfxEvent(EnumSfxSound.SfxMouseClick);
            audioGameOver.PlayOnShot(GetComponent<AudioSource>());
            _gameManager.gamePlayMode = GamePlayModes.MissionMode;
            await GameManager.StateManager.ChangeStateAsync(StatesNames.GameStateMenuInitial.ToString()).ConfigureAwait(false);
        }
    }
}