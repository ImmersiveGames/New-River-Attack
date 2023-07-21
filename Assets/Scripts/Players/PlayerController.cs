using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace RiverAttack
{
    public class PlayerController : MonoBehaviour
    {
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
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_PlayerSettings = m_PlayerMaster.GetPlayersSettings();
            m_PlayersInputActions = new PlayersInputActions();

            m_AutoMovement = GameSettings.instance.autoMovement;
            m_MovementSpeed = m_PlayerSettings.mySpeedy;
            m_MultiplyVelocityUp = m_PlayerSettings.multiplyVelocityUp;
            m_MultiplyVelocityDown = m_PlayerSettings.multiplyVelocityDown;            

            m_PlayersInputActions.Enable();
        }

        void Start()
        {
            m_PlayersInputActions.Player.Move.performed += ctx => TouchMove(ctx);
            m_PlayersInputActions.Player.Move.canceled += ctx => EndTouchMove(ctx);          
        }

        void FixedUpdate()
        {
            if (GameManager.instance.GetActualGameState() != GameManager.States.GamePlay || GameManager.instance.HasGameStopped())
            {
                m_PlayerMaster.SetActualPlayerStateMovement(PlayerMaster.MovementStatus.Paused);
                return;
            }

            //m_PlayerMaster.SetActualPlayerStateMovement(PlayerMaster.MovementStatus.None);

            float axisAutoMovement = m_InputVector.y switch
            {
                > 0 => m_MultiplyVelocityUp,
                < 0 => m_MultiplyVelocityDown,
                _ => m_AutoMovement
            };

            var moveDir = new Vector3(m_InputVector.x, 0, axisAutoMovement);
            if (m_PlayerMaster.ShouldPlayerMove())
                transform.position += moveDir * (m_MovementSpeed * Time.deltaTime);

            m_PlayerMaster.CallEventPlayerMasterControllerMovement(m_InputVector);
        }

        #endregion

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
    }
}
