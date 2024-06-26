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
        [Range(1.1f, 4f)] public float multiplyAccelerate = 2f;
        [Range(0.1f, 0.99f)] public float multiplyDecelerate = .5f;

        private const float AutoPilotSpeed = 1.5f;
        private const int GodMultiSpeedy = 5;

        private float _speedVertical;
        private float _speedHorizontal;

        private const float SpeedAutoVertical = 1f;
        private Vector2 _inputVector = Vector2.zero;

        private PlayerMaster _playerMaster;
        private GamePlayManager _gamePlayManager;

        private void Awake()
        {
            SetInitialReferences();
            SubscribeToEvents();
        }

        private void Start()
        {
            InitializeInput();
        }

        private void FixedUpdate()
        {
            if (!_gamePlayManager.ShouldBePlayingGame || !_playerMaster.ObjectIsReady) return;
            if (_playerMaster.AutoPilot)
            {
                transform.position += MovePlayerAutoPilot();
                return;
            }
            if (_gamePlayManager.IsBossFight && _playerMaster.BossController)
            {
                transform.position += MovePlayerBoss();
                return;
            }
            transform.position += MovePlayer(_playerMaster.godMode);
        }

        private void OnDisable()
        {
            UnsubscribeFromEvents();
        }

        private void SetInitialReferences()
        {
            _gamePlayManager = GamePlayManager.instance;
            _playerMaster = GetComponent<PlayerMaster>();
        }

        private void SubscribeToEvents()
        {
            _playerMaster.EventPlayerMasterInitialize += InitializePlayerController;
            _playerMaster.EventPlayerMasterChangeSkin += UpdateControllerValues;
            _gamePlayManager.EventGameFinisher += SetAutoPilot;
        }

        private void UnsubscribeFromEvents()
        {
            _playerMaster.EventPlayerMasterInitialize -= InitializePlayerController;
            _playerMaster.EventPlayerMasterChangeSkin -= UpdateControllerValues;
            _gamePlayManager.EventGameFinisher -= SetAutoPilot;
        }

        private void InitializeInput()
        {
            InputGameManager.ActionManager.ActivateActionMap(ActionManager.GameActionMaps.Player);
            InputGameManager.RegisterAxisAction("Axis_Move", InputAxisPerformed, InputAxisCanceled);
        }

        private void InitializePlayerController(int indexPlayer, PlayersDefaultSettings playersDefaultSettings)
        {
            var playerMaster = _gamePlayManager.GetPlayerMaster(indexPlayer);
            var skin = playerMaster.ActualSkin ?? playersDefaultSettings.skinDefault;
            _speedVertical = skin.playerSpeed;
            _speedHorizontal = skin.playerAgility;
        }

        private void UpdateControllerValues(ShopProductSkin shopProductSkin)
        {
            _speedVertical = shopProductSkin.playerSpeed;
            _speedHorizontal = shopProductSkin.playerAgility;
        }
        private Vector3 MovePlayerBoss()
        {
            var actualSpeed = _speedVertical;
            switch (_inputVector.y)
            {
                case > 0:
                    actualSpeed *= multiplyAccelerate;
                    break;
                case < 0:
                    actualSpeed *= multiplyDecelerate;
                    break;
            }
            
            var verticalSpeed = _inputVector.y * actualSpeed * Time.deltaTime;
            var horizontalSpeed = _inputVector.x * _speedHorizontal * Time.deltaTime;
            
            var newPosition = transform.position + new Vector3(horizontalSpeed, 0, verticalSpeed);
            var bossAreaX = GamePlayBossManager.instance.bossAreaX;
            var bossAreaZ = GamePlayBossManager.instance.bossAreaZ;
            // Aplicar os limites para a posição da nave
            newPosition.x = Mathf.Clamp(newPosition.x, bossAreaX.x, bossAreaX.y);
            newPosition.z = Mathf.Clamp(newPosition.z, bossAreaZ.x, bossAreaZ.y);

            return newPosition - transform.position;
        }

        private Vector3 MovePlayerAutoPilot()
        {
            var actualSpeed = _speedVertical * AutoPilotSpeed;
            var verticalSpeed = SpeedAutoVertical * actualSpeed * Time.deltaTime;
            return new Vector3(0, 0, verticalSpeed);
        }

        private Vector3 MovePlayer(bool multi)
        {
            var actualSpeed = multi ? _speedVertical * GodMultiSpeedy : _speedVertical;
            switch (_inputVector.y)
            {
                case > 0:
                    actualSpeed *= multiplyAccelerate;
                    break;
                case < 0:
                    actualSpeed *= multiplyDecelerate;
                    break;
            }

            var verticalSpeed = SpeedAutoVertical * actualSpeed * Time.deltaTime;
            var horizontalSpeed = _inputVector.x * _speedHorizontal * Time.deltaTime;

            return new Vector3(horizontalSpeed, 0, verticalSpeed);
        }

        private void SetAutoPilot()
        {
            _playerMaster.AutoPilot = true;
        }

        private void InputAxisPerformed(InputAction.CallbackContext context)
        {
            _inputVector = context.ReadValue<Vector2>().normalized;
            _playerMaster.OnEventPlayerMasterAxisMovement(_inputVector);
            DebugManager.Log<PlayerController>($"Eixo performed: {_inputVector}");
        }

        private void InputAxisCanceled(InputAction.CallbackContext context)
        {
            _inputVector = Vector2.zero;
            _playerMaster.OnEventPlayerMasterAxisMovement(_inputVector);
            DebugManager.Log<PlayerController>($"Eixo Canceled: {_inputVector}");
        }
    }
}