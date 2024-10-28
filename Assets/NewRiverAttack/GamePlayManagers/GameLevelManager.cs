using ImmersiveGames;
using ImmersiveGames.DebugManagers;
using NewRiverAttack.GameManagers;
using NewRiverAttack.LevelBuilder;
using UnityEngine;

namespace NewRiverAttack.GamePlayManagers
{
    public class GameLevelManager : MonoBehaviour
    {
        internal bool IsBossFight;
        private LevelBuilderManager _levelBuilderManager;
        private LevelData _actualLevel;
        
        private GameManager _gameManager;
        private GamePlayManager _gamePlayManager;
        public static GameLevelManager Instance { get; private set; }

        #region Unity Methods

        private void Awake()
        {
            SetInitialReferences();
            if (Instance == null)
            {
                Instance = this;

                DebugManager.Log<GameLevelManager>("GameLevelManager instanciado.");
            }
            else
            {
                Destroy(gameObject);
                DebugManager.LogWarning<GameLevelManager>(
                    "Tentativa de criar uma segunda instância de GameLevelManager foi evitada.");
            }
            
        }
        private void OnEnable()
        {
            BuildLevel();
            _gamePlayManager.EventGameReset += LevelReset;
        }

        private void OnDisable()
        {
            _gamePlayManager.EventGameReset -= LevelReset;
            CleanUpGame();
        }

        #endregion
        
        private void SetInitialReferences()
        {
            _levelBuilderManager = LevelBuilderManager.Instance;
            _gameManager = GameManager.instance;
            _gamePlayManager = GamePlayManager.Instance;
        }
        private void BuildLevel()
        {
            _actualLevel = GetLevel(_gameManager.gamePlayMode);
            IsBossFight = _actualLevel.levelType == LevelTypes.Boss;
            AudioManager.instance.PlayBGM(_actualLevel.setLevelList[0].levelType.ToString());
            _levelBuilderManager.StartToBuild(_actualLevel);
        }
        private LevelData GetLevel(GamePlayModes modes)
        {
            switch (modes)
            {
                case GamePlayModes.ClassicMode:
                    return _gameManager.classicModeLevels;
                case GamePlayModes.MissionMode:
                    return _gameManager.ActiveLevel;
                default:
                    DebugManager.LogError<GamePlayManager>("Não existe um arquivo de data para construir uma cena");
                    return null;
            }
        }

        private void LevelReset()
        {
            _levelBuilderManager.CleanUpLevel();
            BuildLevel();
        }
        private void CleanUpGame()
        {
            _levelBuilderManager.DestroyLevel();
            _levelBuilderManager = null;
        }
    }
}