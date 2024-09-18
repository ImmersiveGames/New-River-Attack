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

            // Ativa o Action Map para navegação no Hub
            InputGameManager.ActionManager.ActivateActionMap(ActionManager.GameActionMaps.HubControl);

            // Registra ações de entrada
            InputGameManager.RegisterAction("StartButton", HandleMissionStart);
            InputGameManager.RegisterAction("BackButton", HandleBackToMenu);
            InputGameManager.RegisterAction("RightSelection", GoRight);
            InputGameManager.RegisterAction("LeftSelection", GoLeft);

            // Adiciona o listener ao botão Start para executar a mesma lógica
            startButton.onClick.AddListener(StartMission);

            _hubGameManager.EventInitializeHub += SetIndexMenu;
            _hubGameManager.EventCursorUpdateHub += SetIndexMenu;
        }

        private void GoLeft(InputAction.CallbackContext obj)
        {
            if (_inInvoke) return;

            // Limpa a seleção anterior e atualiza o botão selecionado
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(backwardButton.gameObject);
            backwardButton.onClick.Invoke();
        }

        private void GoRight(InputAction.CallbackContext obj)
        {
            if (_inInvoke) return;

            // Limpa a seleção anterior e atualiza o botão selecionado
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(forwardButton.gameObject);
            forwardButton.onClick.Invoke();
        }

        private void HandleBackToMenu(InputAction.CallbackContext obj)
        {
            if (_inInvoke) return;

            // Limpa a seleção anterior e atualiza o botão selecionado
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(backButton.gameObject);
            backButton.onClick.Invoke();
        }

        private void HandleMissionStart(InputAction.CallbackContext obj)
        {
            if (_inInvoke) return;

            // Limpa a seleção anterior e atualiza o botão selecionado
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(startButton.gameObject);

            // Unifica a execução da missão chamando a função centralizada
            StartMission();
        }

        private void OnDisable()
        {
            _hubGameManager.EventInitializeHub -= SetIndexMenu;
            _hubGameManager.EventCursorUpdateHub -= SetIndexMenu;

            // Desregistra ações
            InputGameManager.UnregisterAction("StartButton", HandleMissionStart);
            InputGameManager.UnregisterAction("BackButton", HandleBackToMenu);
            InputGameManager.UnregisterAction("RightSelection", GoRight);
            InputGameManager.UnregisterAction("LeftSelection", GoLeft);

            // Remove listener do botão Start
            startButton.onClick.RemoveListener(StartMission);

            // Restaura o Action Map anterior
            InputGameManager.ActionManager.RestoreActionMap();
        }

        private void SetIndexMenu(List<HubOrderData> listHubOrderData, int startIndex)
        {
            _indexMenu = startIndex;
        }

        private void SetInitialReferences()
        {
            _hubGameManager = HubGameManager.Instance;
            _gameManager = GameManager.instance;
        }

        public void ButtonNavigation(bool back)
        {
            if (!_hubGameManager.IsHubReady) return;

            var increment = back ? -1 : 1;

            if (_indexMenu + increment > GameOptionsSave.Instance.activeIndexMissionLevel) return;

            _inInvoke = true;
            _indexMenu += increment;
            if (_indexMenu < 0) _indexMenu = 0;

            _hubGameManager.OnEventCursorUpdateHub(_indexMenu);
            _inInvoke = false;
        }

        // Método unificado para iniciar a missão, chamado independentemente do input
        private async void StartMission()
        {
            _inInvoke = true;

            // Som de clique
            var audioGameOver = AudioManager.instance.GetAudioSfxEvent(EnumSfxSound.SfxMouseClick);
            audioGameOver.PlayOnShot(GetComponent<AudioSource>());

            // Inicia a missão
            _gameManager.gamePlayMode = GamePlayModes.MissionMode;
            _gameManager.ActiveIndex = _indexMenu;
            _gameManager.ActiveLevel = _hubGameManager.LevelOrder[_indexMenu].levelData;

            // Troca de estado de forma assíncrona
            await GameManager.StateManager.ChangeStateAsync(StatesNames.GameStatePlay.ToString()).ConfigureAwait(false);

            _inInvoke = false;
        }

        public async void ButtonMenuInicial()
        {
            _inInvoke = true;

            // Som de clique
            var audioGameOver = AudioManager.instance.GetAudioSfxEvent(EnumSfxSound.SfxMouseClick);
            audioGameOver.PlayOnShot(GetComponent<AudioSource>());

            // Volta ao menu inicial
            _gameManager.gamePlayMode = GamePlayModes.MissionMode;

            // Troca de estado para o menu inicial
            await GameManager.StateManager.ChangeStateAsync(StatesNames.GameStateMenuInitial.ToString()).ConfigureAwait(false);

            _inInvoke = false;
        }
    }
}
