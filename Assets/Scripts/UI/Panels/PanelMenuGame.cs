using UnityEngine;
namespace RiverAttack
{
    [RequireComponent(typeof(AudioSource))]
    public class PanelMenuGame : PanelBase
    {
        [Header("Background")]
        [SerializeField]
        private Transform background;
        [Header("HUD")]
        [SerializeField]
        private Transform hud;
        private GameState _currentGameState;
        private GameManager _gameManager;
        private PlayersInputActions _inputSystem;

        #region UNITYMETHODS
        protected override void Awake()
        {
            _gameManager = GameManager.instance;
            //Garantir que este objeto seja o menu principal quando voltar para a tela de inicio
            if (!_gameManager.panelBaseGame)
            {
                _gameManager.panelBaseGame = this;
            }
            base.Awake();
        }

        private void OnEnable()
        {
            lastIndex = 0;
            // As ações quando chmar o menu, mas ele não precisa se auto configurar a unica coisa que precisa aqui é ativar a hud
        }

        private void Start()
        {
            StartMenuOnOpenScene();
        }
        #endregion

        private void StartMenuOnOpenScene()
        {
            _currentGameState = _gameManager.currentGameState;
            SetInternalMenu();
            menuInitial.gameObject.SetActive(false);
            hud.gameObject.SetActive(false);
            background.gameObject.SetActive(false);
        }
        public void StartMenuGame()
        {
            _currentGameState = GameManager.instance.currentGameState;
            menuInitial.gameObject.SetActive(false);
            background.gameObject.SetActive(false);
            hud.gameObject.SetActive(true);
        }

        public void PauseMenu(bool active)
        {
            _currentGameState = GameManager.instance.currentGameState;
            menuInitial.gameObject.SetActive(active);
            background.gameObject.SetActive(active);
            hud.gameObject.SetActive(!active);
        }

        public void SetMenuEndPath()
        {
            _currentGameState = GameManager.instance.currentGameState;
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
        }

        public void ButtonUnPause()
        {
            GamePlayManager.UnPauseGamePlay();
        }
        
        public void ButtonExitToHub()
        {
            ClearSceneForButtons();
            _gameManager.ChangeState(new GameStateHub(), GameManager.GameScenes.MissionHub.ToString());
        }
        public void ButtonReturnInitialMenu()
        {
            ClearSceneForButtons();
            _gameManager.ChangeState(new GameStateMenu(), GameManager.GameScenes.MainScene.ToString());
        }

        private void ClearSceneForButtons()
        {
            Time.timeScale = 1;
            PlayClickSfx();
            StopAllCoroutines();
            PlayerManager.instance.DestroyPlayers();
            GameMissionBuilder.instance.ResetBuildMission();
        }
    }
}
