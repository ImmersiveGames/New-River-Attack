using ImmersiveGames;
using ImmersiveGames.InputManager;
using NewRiverAttack.AudioManagers;
using UnityEngine;
using NewRiverAttack.GameManagers;
using NewRiverAttack.GamePlayManagers;
using UnityEngine.InputSystem;

namespace NewRiverAttack.HUBManagers
{
    public class HubNavigationManager : MonoBehaviour
    {
        private int _tempIndex; // Índice temporário usado para navegação
        private HubGameManager _hubGameManager;
        private GameManager _gameManager;

        private void Awake()
        {
            _gameManager = GameManager.instance;
            _hubGameManager = HubGameManager.Instance;
            _tempIndex = _gameManager.ActiveIndex >= 0 ? _gameManager.ActiveIndex :_hubGameManager.SaveIndex; // Começa no índice salvo
        }

        private void OnEnable()
        {
            _hubGameManager.EventUpdateIndex += UpdateIndex;
            // Ativa o Action Map para navegação no Hub
            InputGameManager.ActionManager.ActivateActionMap(ActionManager.GameActionMaps.HubControl);

            // Registra ações de entrada
            InputGameManager.RegisterAction("RightSelection", NavigateForward);
            InputGameManager.RegisterAction("LeftSelection", NavigateBackward);
            
        }

        private void OnDisable()
        {
            _hubGameManager.EventUpdateIndex += UpdateIndex;
            // Des-registra ações
            InputGameManager.UnregisterAction("RightSelection", NavigateForward);
            InputGameManager.UnregisterAction("LeftSelection", NavigateBackward);

            // Restaura o Action Map anterior
            InputGameManager.ActionManager.RestoreActionMap();
        }

        private void UpdateIndex(int obj)
        {
            _tempIndex = obj;
        }
        private void NavigateForward(InputAction.CallbackContext obj)
        {
            NavigateForward();
        }

        /// <summary>
        /// Avança o índice temporário dentro dos limites permitidos e atualiza o cursor.
        /// </summary>
        public void NavigateForward()
        {
            if(!_hubGameManager.ActiveHub) return;
            var maxIndex = Mathf.Min(_hubGameManager.CachedHubOrderData.Count - 1, _hubGameManager.SaveIndex);

            if (_tempIndex >= maxIndex) return;
            // Som de clique
            var audioMouseClick = AudioManager.instance.GetAudioSfxEvent(EnumSfxSound.SfxEngineAccelerate);
            audioMouseClick.PlayOnShot(GetComponent<AudioSource>());
            _tempIndex++;
            _hubGameManager.OnEventCursorMove(_tempIndex);
        }

        /// <summary>
        /// Retrocede o índice temporário até o limite inicial (0) e atualiza o cursor.
        /// </summary>
        /// 
        private void NavigateBackward(InputAction.CallbackContext obj)
        {
            NavigateBackward();
        }
        public void NavigateBackward()
        {
            if (_tempIndex <= 0 || !_hubGameManager.ActiveHub) return;
            var audioMouseClick = AudioManager.instance.GetAudioSfxEvent(EnumSfxSound.SfxEngineAccelerate);
            audioMouseClick.PlayOnShot(GetComponent<AudioSource>());
            _tempIndex--;
            _hubGameManager.OnEventCursorMove(_tempIndex);
        }
        
        public void StartMission()
        {
            // Inicia a missão
            _gameManager.gamePlayMode = GamePlayModes.MissionMode;
            _gameManager.ActiveIndex = _tempIndex;
            _gameManager.ActiveLevel = _hubGameManager.GetLevelDataByIndex(_tempIndex);
        }
        public void MenuInicial()
        {
            // Volta ao menu inicial
            _gameManager.gamePlayMode = GamePlayModes.MissionMode;
            _gameManager.ActiveIndex = -1;
            _gameManager.CompleteIndex = -1;
            _gameManager.ActiveLevel = null;
        }
    }
}