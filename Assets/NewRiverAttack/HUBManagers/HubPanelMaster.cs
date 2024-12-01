using ImmersiveGames;
using ImmersiveGames.InputManager;
using NewRiverAttack.AudioManagers;
using NewRiverAttack.GameManagers;
using NewRiverAttack.StateManagers;
using UnityEngine;
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

        private HubGameManager _hubGameManager;
        private HubNavigationManager _navigationManager;

        private void Awake()
        {
            _hubGameManager = HubGameManager.Instance;
            _navigationManager = GetComponent<HubNavigationManager>();
            EnableButtons(false);
            // Adiciona os eventos para os botões
            forwardButton.onClick.AddListener(() => _navigationManager.NavigateForward());
            backwardButton.onClick.AddListener(() => _navigationManager.NavigateBackward());
            startButton.onClick.AddListener( ButtonStartMission);
            backButton.onClick.AddListener(ButtonMenuInitial);
        }

        private void OnEnable()
        {
            _hubGameManager.EventBuildHub += InitializePanel;
            InputGameManager.RegisterAction("StartButton", ButtonStartMission);
            InputGameManager.RegisterAction("BackButton", ButtonMenuInitial);
        }

        private void OnDisable()
        {
            _hubGameManager.EventBuildHub -= InitializePanel;
            InputGameManager.UnregisterAction("StartButton", ButtonStartMission);
            InputGameManager.UnregisterAction("BackButton", ButtonMenuInitial);
        }

        private void InitializePanel()
        {
            EnableButtons(true);
        }

        private void EnableButtons(bool enable)
        {
            startButton.interactable = enable;
            backButton.interactable = enable;
            forwardButton.interactable = enable;
            backwardButton.interactable = enable;
        }
        private void ButtonStartMission(InputAction.CallbackContext obj)
        {
            ButtonStartMission();
        }
        private async void ButtonStartMission()
        {
            if(!_hubGameManager.ActiveHub) return;
            EnableButtons(false);
            // Som de clique
            var audioMouseClick = AudioManager.instance.GetAudioSfxEvent(EnumSfxSound.SfxMouseClick);
            audioMouseClick.PlayOnShot(GetComponent<AudioSource>());
            _navigationManager.StartMission();
            // Troca de estado de forma assíncrona
            await GameManager.StateManager.ChangeStateAsync(StatesNames.GameStatePlay.ToString()).ConfigureAwait(false);
        }
        private void ButtonMenuInitial(InputAction.CallbackContext obj)
        {
            ButtonMenuInitial();
        }
        private async void ButtonMenuInitial()
        {
            if(!_hubGameManager.ActiveHub) return;
            EnableButtons(false);
            // Som de clique
            var audioMouseClick = AudioManager.instance.GetAudioSfxEvent(EnumSfxSound.SfxMouseClick);
            audioMouseClick.PlayOnShot(GetComponent<AudioSource>());
            _navigationManager.MenuInicial();
            // Troca de estado de forma assíncrona
            await GameManager.StateManager.ChangeStateAsync(StatesNames.GameStateMenuInitial.ToString()).ConfigureAwait(false);
        }
        
    }
}
