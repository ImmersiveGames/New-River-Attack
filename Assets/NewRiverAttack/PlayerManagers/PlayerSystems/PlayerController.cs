using ImmersiveGames.DebugManagers;
using ImmersiveGames.InputManager;
using ImmersiveGames.ShopManagers.ShopProducts;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.PlayerManagers.ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NewRiverAttack.PlayerManagers.PlayerSystems
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
            _playerMaster.EventPlayerMasterChangeSkin += UpdateControllerValues;
            _gamePlayManager.EventGameFinisher += SetAutoPilot;
        }

        private void Start()
        {
            InputGameManager.ActionManager.ActivateActionMap(ActionManager.GameActionMaps.Player);
            InputGameManager.RegisterAxisAction("Axis_Move", InputAxisPerformed, InputAxisCanceled);
        }

        private void FixedUpdate()
        {
            if (!_gamePlayManager.ShouldBePlayingGame || !_playerMaster.ObjectIsReady) return;

            transform.position += MovePlayer();

        }

        private void OnDisable()
        {
            _playerMaster.EventPlayerMasterInitialize -= InitializePlayerController;
            _playerMaster.EventPlayerMasterChangeSkin -= UpdateControllerValues;
            _gamePlayManager.EventGameFinisher -= SetAutoPilot;
        }

        #endregion
        
        #region Initializations
        private void SetInitialReferences()
        {
            _gamePlayManager = GamePlayManager.instance;
            //todo: Revisitar aqui para o boss fight
            autoMove = !_gamePlayManager.IsBossFight;
            _playerMaster = GetComponent<PlayerMaster>();
        }

        private void InitializePlayerController(int indexPlayer, PlayersDefaultSettings playersDefaultSettings)
        {
            var playerMaster = _gamePlayManager.GetPlayerMaster(indexPlayer);
            var skin = playerMaster.ActualSkin;
            if (skin == null)
            {
                skin = playersDefaultSettings.skinDefault;
            }
            _speedVertical = skin.playerSpeed;
            _speedHorizontal = skin.playerAgility;
        }
        
        private void UpdateControllerValues(ShopProductSkin shopProductSkin)
        {
            _speedVertical = shopProductSkin.playerSpeed;
            _speedHorizontal = shopProductSkin.playerAgility;
        }
        #endregion
        
        private Vector3 MovePlayer()
        {
            if (!_playerMaster.ObjectIsReady) return Vector3.zero;
            var verticalVector = autoMove ? SpeedAutoVertical : _inputVector.y;
            if (_playerMaster.inFinishPath)
            {
                verticalVector = 1;
                _inputVector = Vector2.up;
            }
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
            var upSpeed = (_playerMaster.inFinishPath) ? verticalVector/3 * actualSpeed * Time.deltaTime:0;
            var horizontalSpeed = _inputVector.x * _speedHorizontal * Time.deltaTime;
            var moveDir = new Vector3(horizontalSpeed, upSpeed, verticalSpeed);
            return moveDir;
        }
        private void SetAutoPilot()
        {
            _playerMaster.inFinishPath = true;
        }

        #region InputSystem

        private void InputAxisPerformed(InputAction.CallbackContext context)
        {
            _inputVector = context.ReadValue<Vector2>().normalized;
            _playerMaster.OnEventPlayerMasterAxisMovement(_inputVector);
            
            DebugManager.Log<PlayerController>($"Eixo performed: {_inputVector}");
        }
        private void InputAxisCanceled(InputAction.CallbackContext context)
        {
            _inputVector = context.ReadValue<Vector2>().normalized;
            _playerMaster.OnEventPlayerMasterAxisMovement(_inputVector);
            DebugManager.Log<PlayerController>($"Eixo Canceled: {_inputVector}");
        }

        #endregion
    }
}