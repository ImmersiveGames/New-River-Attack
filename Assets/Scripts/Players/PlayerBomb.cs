using ImmersiveGames;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;
namespace RiverAttack
{
    public class PlayerBomb : MonoBehaviour, ICommand
    {
        [Header("Bomb Settings")]
        [SerializeField]
        private Vector3 bombOffset;
        [SerializeField] private GameObject prefabBomb;

        private PlayerMasterOld _playerMasterOld;
        private GamePlayManager _gamePlayManager;
        private PlayerSettings _playerSettings;
        private PlayersInputActions _playersInputActions;
        private GamePlayingLog _gamePlayingLog;

        #region UNITYMETHODS

        private void OnEnable()
        {
            SetInitialReferences();
        }

        private void Start()
        {
            _playersInputActions = GameManager.instance.inputSystem;
            _playerSettings.bombs = GameSettings.instance.startBombs;
            _playersInputActions.Player.Bomb.performed += Execute;
        }

        private void OnDisable()
        {
            _playersInputActions.Player.Bomb.performed -= Execute;
        }

        #endregion

        private void SetInitialReferences()
        {
            _gamePlayManager = GamePlayManager.instance;
            _playerMasterOld = GetComponent<PlayerMasterOld>();
            _playerSettings = _playerMasterOld.getPlayerSettings;
            _gamePlayingLog = _gamePlayManager.gamePlayingLog;
        }
        
        public void Execute(InputAction.CallbackContext callbackContext)
        {
           //Debug.Log($"Collect{_playerSettings.bombs}");
           if (_playerSettings.bombs <= 0 || !_gamePlayManager.shouldBePlayingGame || !_playerMasterOld.ShouldPlayerBeReady)
               return;
           _gamePlayManager.OnEventPlayerPushButtonBomb();
            _playerSettings.bombs -= 1;
            var bomb = Instantiate(prefabBomb);
            bomb.transform.localPosition = transform.localPosition + bombOffset;
            bomb.GetComponent<Bullets>().ownerShoot = _playerMasterOld;
            bomb.SetActive(true);
            _gamePlayManager.OnEventUpdateBombs(_playerSettings.bombs);
            LogGamePlay(1);
        }
        public void UnExecute(InputAction.CallbackContext callbackContext)
        {
            //throw new NotImplementedException();
        }

        private void LogGamePlay(int bomb)
        {
            _gamePlayingLog.bombSpent += bomb;
        }

        /*

        public void AddBomb(int ammoAmount)
        {
            bombQuantity += ammoAmount;
            _playerMaster.CallEventPlayerBomb();
        }
        */
    }
}
