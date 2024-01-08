using UnityEngine;
using UnityEngine.InputSystem;
namespace RiverAttack
{
    [RequireComponent(typeof(AudioSource))]
    public class PanelMenuGame : PanelBase
    {
        [Header("Background")]
        [SerializeField] Transform background;
        [Header("HUD")]
        [SerializeField] Transform hud;

        GameState m_CurrentGameState;
        GameManager m_GameManager;
        PlayersInputActions m_InputSystem;

        #region UNITYMETHODS
        protected override void Awake()
        {
            m_GameManager = GameManager.instance;
            //Garantir que este objeto seja o menu principal quando voltar para a tela de inicio
            if (!m_GameManager.panelBaseGame)
            {
                m_GameManager.panelBaseGame = this;
            }
            base.Awake();
        }
        void OnEnable()
        {
            m_InputSystem = GamePlayManager.instance.inputSystem;
            m_InputSystem.UI_Controlls.Enable();
            m_InputSystem.Player.Disable();
            lastIndex = 0;
            // As ações quando chmar o menu, mas ele não precisa se auto configurar a unica coisa que precisa aqui é ativar a hud
        }

        void Start()
        {
            m_InputSystem.Player.Pause.performed += ExecutePauseGame;
            StartMenuOnOpenScene();
        }
        void OnDisable()
        {
            m_InputSystem.UI_Controlls.Disable();
            m_InputSystem.Player.Enable();
        }
        #endregion
        
        void StartMenuOnOpenScene()
        {
            m_CurrentGameState = m_GameManager.currentGameState;
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

        public void SetMenuEndPath()
        {
            m_CurrentGameState = GameManager.instance.currentGameState;
            menuInitial.gameObject.SetActive(false);
            background.gameObject.SetActive(false);
            hud.gameObject.SetActive(false);
        }
        public void SetMenuGameOver()
        {
            hud.gameObject.SetActive(false);
            background.gameObject.SetActive(false);
            menuInitial.gameObject.SetActive(true);
            SetInternalMenu(menuPrincipal.Length - 1);
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
            if(m_GameManager.lastGameState is GameStatePlayGame)
                m_GameManager.ChangeState(new GameStatePlayGame());
            if (GameManager.instance.currentGameState is GameStatePlayGameBoss)
            {
                GameStatePlayGameBoss.PauseState(false);
            }
        }

        void ExecutePauseGame(InputAction.CallbackContext callbackContext)
        {
            m_CurrentGameState = GameManager.instance.currentGameState;
            //Debug.Log($"Pause: {m_CurrentGameState}");
            switch (m_CurrentGameState)
            {
                case GameStatePlayGame:
                    ButtonGamePause();
                    break;
                case GameStatePlayGameBoss:
                    GameStatePlayGameBoss.PauseState(!GamePlayManager.instance.bossFightPause);
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
