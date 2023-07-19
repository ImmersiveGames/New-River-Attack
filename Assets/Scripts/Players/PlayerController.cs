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
        
        [SerializeField] float autoMovement;
        [SerializeField] float movementSpeed;
        [SerializeField] float multiplyVelocityUp;
        [SerializeField] float multiplyVelocityDown;

        Vector2 m_InputVector;

        #region UNITYMETHODS
        void Awake()
        {
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_PlayerSettings = m_PlayerMaster.GetPlayersSettings();
            m_PlayersInputActions = new PlayersInputActions();

            autoMovement = GameSettings.instance.autoMovement;
            movementSpeed = m_PlayerSettings.mySpeedy;
            multiplyVelocityUp = m_PlayerSettings.multiplyVelocityUp;
            multiplyVelocityDown = m_PlayerSettings.multiplyVelocityDown;            

            m_PlayersInputActions.Enable();
        }

        void Start()
        {
            m_PlayersInputActions.Player.Move.performed += ctx => TouchMove(ctx);
            m_PlayersInputActions.Player.Move.canceled += ctx => EndTouchMove(ctx);
        }

        void FixedUpdate()
        {
            if (GameManager.instance.GetStates() != GameManager.States.GamePlay || GameManager.instance.GetPaused())
            {
                m_PlayerMaster.SetHasPlayerReady(false);
                return;
            }

            m_PlayerMaster.SetHasPlayerReady(true);

            //var inputVector = GetMovementVector2Normalized();

            float axisAutoMovement = m_InputVector.y switch
            {
                > 0 => multiplyVelocityUp,
                < 0 => multiplyVelocityDown,
                _ => autoMovement
            };

            var moveDir = new Vector3(m_InputVector.x, 0, axisAutoMovement);
            if (m_PlayerMaster.GetHasPlayerReady())
                transform.position += moveDir * (movementSpeed * Time.deltaTime);

            m_PlayerMaster.CallEventControllerMovement(m_InputVector);
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
