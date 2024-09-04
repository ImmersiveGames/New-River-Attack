using ImmersiveGames.DebugManagers;
using ImmersiveGames.InputManager;
using NewRiverAttack.AudioManagers;
using NewRiverAttack.GameManagers;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.StateManagers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace ImmersiveGames.MenuManagers
{
    /*public class MenuGamePauseManager: MonoBehaviour
    {
        [SerializeField] private Transform splashScreen;
        [SerializeField] private Transform menuPause;
        [SerializeField] private Transform menuHud;
        [SerializeField] private Transform splashComplete;

        private GamePlayManager _gamePlayManager;
        //private GameStartMenu _gameStartMenu;

        #region Unity Methods

        private void OnEnable()
        {
            SetInitialReferences();
            splashScreen.gameObject.SetActive(false);
            menuPause.gameObject.SetActive(false);
            menuHud.gameObject.SetActive(false);
            splashComplete.gameObject.SetActive(false);

            _gamePlayManager.EventGameReady += SetHudMenu;
            _gamePlayManager.EventGameOver += SetGameOver;
            _gamePlayManager.EventGameFinisher += SetEndPath;
            
        }

        private void OnDisable()
        {
            _gamePlayManager.EventGameReady -= SetHudMenu;
            _gamePlayManager.EventGameOver -= SetGameOver;
            _gamePlayManager.EventGameFinisher -= SetEndPath;
        }

        #endregion
        
        private void SetInitialReferences()
        {
            _gamePlayManager = GamePlayManager.Instance;
            //_gameStartMenu = GetComponentInChildren<GameStartMenu>(true);
        }

        private void SetPauseMenu(bool onPause)
        {
            DebugManager.Log<MenuGamePauseManager>($"Ativou Menu Pause");
            splashScreen.gameObject.SetActive(!onPause);
            menuPause.gameObject.SetActive(!onPause);
            menuHud.gameObject.SetActive(onPause);
            splashComplete.gameObject.SetActive(false);
            _gameStartMenu.ActivateMenu(0);
        }
        private void SetHudMenu()
        {
            DebugManager.Log<MenuGamePauseManager>($"Ativou Menu HUD");
            InputGameManager.ActionManager.ActivateActionMap(ActionManager.GameActionMaps.Player);
            InputGameManager.RegisterAction("Pause", StartPauseMenu );
            SetupInitial();
        }
        private void StartPauseMenu(InputAction.CallbackContext obj)
        {
            if(!_gamePlayManager.ShouldBePlayingGame) return;
            DebugManager.Log<MenuGamePauseManager>($"Chamou a Pausa");
            SetPauseMenu(_gamePlayManager.IsPause);
            if (!_gamePlayManager.IsPause)
            {
                _gamePlayManager.OnEventGamePause();
            }
            else
            {
                _gamePlayManager.OnEventGameUnPause();
            }
        }

        private void SetGameOver()
        {
            splashScreen.gameObject.SetActive(false);
            menuPause.gameObject.SetActive(true);
            _gameStartMenu.ActivateMenu(1);
            menuHud.gameObject.SetActive(true);
            splashComplete.gameObject.SetActive(false);
            var audioGameOver = AudioManager.instance.GetAudioSfxEvent(EnumSfxSound.SfxGameOver);
            audioGameOver.PlayOnShot(GetComponent<AudioSource>());
        }

        private void SetEndPath()
        {
            splashScreen.gameObject.SetActive(false);
            menuPause.gameObject.SetActive(false); 
            menuHud.gameObject.SetActive(true);
            splashComplete.gameObject.SetActive(true);
            var audioGameOver = AudioManager.instance.GetAudioSfxEvent(EnumSfxSound.SfxComplete);
            audioGameOver.PlayOnShot(GetComponent<AudioSource>());
        }

        private void SetupInitial()
        {
            splashScreen.gameObject.SetActive(false);
            menuPause.gameObject.SetActive(false);
            menuHud.gameObject.SetActive(true);
            splashComplete.gameObject.SetActive(false);
        }
        
        
        #region Button Menu

        public void ButtonUnPause()
        {
            SetPauseMenu(true);
            _gamePlayManager.OnEventGameUnPause();
        }

        public async void ButtonExitGame()
        {
            _gamePlayManager.OnEventGameUnPause();
            await GameManager.StateManager.ChangeStateAsync(StatesNames.GameStateMenuInitial.ToString()).ConfigureAwait(false);
        }
        /*public async void ButtonRestart()
        {
            SetupInitial();
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
            await GameManager.StateManager.ChangeStateAsync(StatesNames.GameStatePlay.ToString()).ConfigureAwait(false);
        }#1#

        #endregion
    }*/
}