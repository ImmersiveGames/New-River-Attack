using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
namespace RiverAttack
{
    [RequireComponent(typeof(AudioSource))]
    public class PanelMenuGame : PanelBase
    {
        [Header("HUD")]
        [SerializeField] Transform hud;
        
        [Header("Buttons")]
        [SerializeField] internal GameObject pauseButton;
        [SerializeField] internal GameObject continueButton;

        GameState m_CurrentGameState;
        PlayersInputActions m_InputSystem;
        

        #region UNITYMETHODS
        protected override void Awake()
        {
            m_InputSystem = new PlayersInputActions();
            m_InputSystem.Enable();
            m_CurrentGameState = GameManager.instance.currentGameState;
            base.Awake();
            menuInitial.gameObject.SetActive(false);
            hud.gameObject.SetActive(true);
        }
        void OnEnable()
        {
            lastIndex = 0;
            // As ações quando chmar o menu, mas ele não precisa se auto configurar a unica coisa que precisa aqui é ativar a hud
        }

        void Start()
        {
            m_InputSystem.Player.Pause.performed += ExecutePauseGame;
        }
        #endregion
        
        
        #region  Actions Application
        protected override void OnApplicationFocus(bool hasFocus)
        {
            base.OnApplicationFocus(hasFocus);
            if (!hasFocus && GameManager.instance.currentGameState is GameStatePlayGame)
                    pauseButton.GetComponent<Button>().onClick.Invoke();
        }

        protected override void OnApplicationPause(bool pauseStatus)
        {
            base.OnApplicationPause(pauseStatus);
            if (pauseStatus && GameManager.instance.currentGameState is GameStatePlayGame)
                    pauseButton.GetComponent<Button>().onClick.Invoke();

        }
        #endregion
        public void SetMenuHudControl(bool active)
        {
            /*menuHud.gameObject.SetActive(active);
            menuControl.gameObject.SetActive(active);
            menuGameOver.gameObject.SetActive(false);
            
            //menuFade.gameObject.SetActive(false);*/
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
        
        void ExecutePauseGame(InputAction.CallbackContext callbackContext)
        {
            switch (m_CurrentGameState)
            {
                case GameStatePlayGame:
                    pauseButton.GetComponent<Button>().onClick.Invoke();
                    break;
                case GameStatePause:
                    continueButton.GetComponent<Button>().onClick.Invoke();
                    break;
            }
        }
    }
}
