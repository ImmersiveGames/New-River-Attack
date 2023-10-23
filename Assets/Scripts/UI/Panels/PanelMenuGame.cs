using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.UI;
namespace RiverAttack
{
    [RequireComponent(typeof(AudioSource))]
    public class PanelMenuGame : PanelBase
    {
        [Header("Background")]
        [SerializeField] Transform background;
        [Header("HUD")]
        [SerializeField] Transform hud;

        /*[Header("Buttons")]
        [SerializeField] internal GameObject pauseButton;
        [SerializeField] internal GameObject continueButton;*/

        GameState m_CurrentGameState;
        GameManager m_GameManager;
        PlayersInputActions m_InputSystem;


        #region UNITYMETHODS
        protected override void Awake()
        {
            m_GameManager = GameManager.instance;
            if (!m_GameManager.panelBaseGame)
            {
                m_GameManager.panelBaseGame = this;
            }
            base.Awake();
            StartMenuOnOpenScene();
        }
        void OnEnable()
        {
            m_InputSystem = new PlayersInputActions();
            m_InputSystem.Enable();
            lastIndex = 0;
            // As ações quando chmar o menu, mas ele não precisa se auto configurar a unica coisa que precisa aqui é ativar a hud
        }

        void Start()
        {
            m_InputSystem.Player.Pause.performed += ExecutePauseGame;
        }
        #endregion


        #region Actions Application
        protected override void OnApplicationFocus(bool hasFocus)
        {
            base.OnApplicationFocus(hasFocus);
            if (!hasFocus && GameManager.instance.currentGameState is GameStatePlayGame)
                ButtonGamePause();
        }

        protected override void OnApplicationPause(bool pauseStatus)
        {
            base.OnApplicationPause(pauseStatus);
            if (pauseStatus && GameManager.instance.currentGameState is GameStatePlayGame)
                ButtonGamePause();
        }
        #endregion

        void StartMenuOnOpenScene()
        {
            m_CurrentGameState = GameManager.instance.currentGameState;
            SetInternalMenu();
            menuInitial.gameObject.SetActive(false);
            hud.gameObject.SetActive(false);
            background.gameObject.SetActive(false);
        }
        public void StartMenuGame()
        {
            m_CurrentGameState = GameManager.instance.currentGameState;
            menuInitial.gameObject.SetActive(false);
            background.gameObject.SetActive(false);
            hud.gameObject.SetActive(true);
        }

        public void PauseMenu(bool active)
        {
            m_CurrentGameState = GameManager.instance.currentGameState;
            menuInitial.gameObject.SetActive(active);
            background.gameObject.SetActive(active);
            hud.gameObject.SetActive(!active);
        }
        public void SetMenuGameOver()
        {
            /*menuControl.gameObject.SetActive(false);
            menuHud.gameObject.SetActive(false);
            menuParent.gameObject.SetActive(true);
            backgroundImage.enabled = false;
            logoImage.enabled = false;
            foreach (var t in menuPrincipal)
            {
                t.gameObject.SetActive(false);
            }
            menuGameOver.gameObject.SetActive(true);*/
        }
        void ButtonGamePause()
        {
            m_GameManager.ChangeState(new GameStatePause());
        }
        public void ButtonGameUnPause()
        {
            m_GameManager.ChangeState(new GameStatePlayGame());
        }

        void ExecutePauseGame(InputAction.CallbackContext callbackContext)
        {
            m_CurrentGameState = GameManager.instance.currentGameState;
            Debug.Log($"Pause: {m_CurrentGameState}");
            switch (m_CurrentGameState)
            {
                case GameStatePlayGame:
                    ButtonGamePause();
                    break;
                case GameStatePause:
                    ButtonGameUnPause();
                    break;
            }
        }

        public void ButtonReturnInitialMenu()
        {
            PlayClickSfx();
            StopAllCoroutines();
            PlayerManager.instance.DestroyPlayers();
            GameMissionBuilder.instance.ResetBuildMission();
            m_InputSystem.Disable();
            //TODO: desligar tudo para depois mudar a scena.
            m_GameManager.ChangeState(new GameStateMenu(), GameManager.GameScenes.MainScene.ToString());
        }
    }
}
