using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace RiverAttack
{
    public class PlayerController : MonoBehaviour
    {
        /*GameManager m_GameManager;
        GamePlayManager m_GamePlayManager;
        PlayerMaster m_PlayerMaster;
        PlayersInputActions m_PlayersInputActions;

        PlayerSettings m_PlayerSettings;
        
        float m_AutoMovement;
        float m_MovementSpeed;
        float m_MultiplyVelocityUp;
        float m_MultiplyVelocityDown;

        Vector2 m_InputVector;

        #region UNITYMETHODS
        void Awake()
        {
            m_AutoMovement = GameSettings.instance.autoMovement;
        }
        void OnEnable()
        {
            SetInitialReferences();
            
        }

        void Start()
        {
            m_PlayersInputActions = m_PlayerMaster.playersInputActions;
            m_PlayersInputActions.Enable();
            m_PlayersInputActions.Player.Move.performed += ctx => TouchMove(ctx);
            m_PlayersInputActions.Player.Move.canceled += ctx => EndTouchMove(ctx);          
        }

        void FixedUpdate()
        {
            if(!m_GamePlayManager.shouldBePlayingGame && !m_PlayerMaster.ShouldPlayerBeReady()) return;
            if (m_GameManager.GetActualGameState() != GameManager.States.GamePlay || m_GameManager.isGameStopped || m_GamePlayManager.isGamePlayPause)
            {
                m_PlayerMaster.SetActualPlayerStateMovement(PlayerMaster.MovementStatus.Paused);
                return;
            }
            if(m_PlayerMaster.playerMovementStatus == PlayerMaster.MovementStatus.Paused )
                m_PlayerMaster.SetActualPlayerStateMovement(PlayerMaster.MovementStatus.None);

            float axisAutoMovement = m_InputVector.y switch
            {
                > 0 => m_MultiplyVelocityUp,
                < 0 => m_MultiplyVelocityDown,
                _ => m_AutoMovement
            };

            var moveDir = new Vector3(m_InputVector.x, 0, axisAutoMovement);
            if (m_PlayerMaster.ShouldPlayerBeReady())
                transform.position += moveDir * (m_MovementSpeed * Time.deltaTime);

            m_PlayerMaster.CallEventPlayerMasterControllerMovement(m_InputVector);
        }

        #endregion

        void SetInitialReferences()
        {
            m_GameManager = GameManager.instance;
            m_GamePlayManager = GamePlayManager.instance;
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_PlayerSettings = m_PlayerMaster.GetPlayersSettings();
            SetValuesFromPlayerSettings(m_PlayerSettings);
        }

        public Vector2 GetMovementVector2Normalized()
        {
            return m_PlayersInputActions.Player.Move.ReadValue<Vector2>().normalized;
        }

        void TouchMove(InputAction.CallbackContext context) 
        {
            m_InputVector = context.ReadValue<Vector2>().normalized; 
        }

        void EndTouchMove(InputAction.CallbackContext context) 
        {
            m_InputVector = Vector2.zero;
        }

        void SetValuesFromPlayerSettings(PlayerSettings settings)
        {
            m_MovementSpeed = settings.mySpeedy;
            m_MultiplyVelocityUp = settings.multiplyVelocityUp;
            m_MultiplyVelocityDown = settings.multiplyVelocityDown;
        }*/
    }
}
