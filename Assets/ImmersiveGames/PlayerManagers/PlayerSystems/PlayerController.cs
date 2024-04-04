using ImmersiveGames.DebugManagers;
using ImmersiveGames.GamePlayManagers;
using ImmersiveGames.InputManager;
using ImmersiveGames.PlayerManagers.ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ImmersiveGames.PlayerManagers.PlayerSystems
{
    [RequireComponent(typeof(PlayerMaster))]
    public class PlayerController : MonoBehaviour
    {
        public bool autoMove;
        [Range(1.1f, 4f)]
        public float multiplyAccelerate = 2f;
        [Range(0.1f, 0.99f)]
        public float multiplyDecelerate = .5f;

        private float _speedVertical;
        private float _speedHorizontal;
        
        private const float SpeedAutoVertical = 1f;

        private Vector2 _inputVector = Vector2.zero;
        
        private PlayerMaster _playerMaster;
        private GamePlayManager _gamePlayManager;
        #region Unity Methods

        private void Awake()
        {
            SetInitialReferences();
            _playerMaster.EventPlayerMasterInitialize += InitializePlayerController;
        }

        private void Start()
        {
            InputGameManager.ActionManager.ActivateActionMap(ActionManager.GameActionMaps.Player);
            InputGameManager.RegisterAxisAction("Axis_Move", InputAxisPerformed, InputAxisCanceled);
        }

        private void FixedUpdate()
        {
            if (!_gamePlayManager.ShouldBePlayingGame || !_playerMaster.PlayerIsReady) return;

            transform.position += MovePlayer();

        }

        private void OnDisable()
        {
            _playerMaster.EventPlayerMasterInitialize -= InitializePlayerController;
        }

        #endregion
        
        #region Initializations
        private void SetInitialReferences()
        {
            _gamePlayManager = GamePlayManager.instance;
            autoMove = !_gamePlayManager.IsBossFight;
            _playerMaster = GetComponent<PlayerMaster>();
        }

        private void InitializePlayerController(int indexPlayer, PlayersDefaultSettings playersDefaultSettings)
        {
            _speedVertical = playersDefaultSettings.playerSpeed;
            _speedHorizontal = playersDefaultSettings.playerAgility;
        }
        #endregion
        
        private Vector3 MovePlayer()
        {
            var verticalVector = autoMove ? SpeedAutoVertical : _inputVector.y;
            var actualSpeed = _playerMaster.godMode ? _speedVertical * 4 : _speedVertical;
            if (autoMove)
            {
                actualSpeed *= _inputVector.y switch
                {
                    > 0 => multiplyAccelerate,
                    < 0 => multiplyDecelerate,
                    _ => SpeedAutoVertical
                }; 
            }

            var verticalSpeed = verticalVector * actualSpeed * Time.deltaTime;
            var horizontalSpeed = _inputVector.x * _speedHorizontal * Time.deltaTime;
            var moveDir = new Vector3(horizontalSpeed, 0, verticalSpeed);
            return moveDir;
        }

        #region InputSystem

        private void InputAxisPerformed(InputAction.CallbackContext context)
        {
            _inputVector = context.ReadValue<Vector2>().normalized;
            DebugManager.Log<PlayerController>($"Eixo performed: {_inputVector}");
        }
        private void InputAxisCanceled(InputAction.CallbackContext context)
        {
            _inputVector = Vector2.zero;
            DebugManager.Log<PlayerController>($"Eixo Canceled: {_inputVector}");
        }

        #endregion
    }
}