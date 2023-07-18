using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace RiverAttack
{
    public class PlayerController : MonoBehaviour
    {
        PlayerMaster m_PlayerMaster;        
        PlayersInputActions m_PlayersInputActions;

        PlayerStats playerStats;
        
        [SerializeField] float m_AutoMovement;
        [SerializeField] float m_MovementSpeed;
        [SerializeField] float m_MultiplyVelocityUp;
        [SerializeField] float m_MultiplyVelocityDown;

        Vector2 inputVector;

        #region UNITYMETHODS
        void Awake()
        {
            m_PlayerMaster = GetComponent<PlayerMaster>();
            playerStats = m_PlayerMaster.GetPlayersSettings();
            m_PlayersInputActions = new PlayersInputActions();

            m_AutoMovement = GameSettings.instance.autoMovement;
            m_MovementSpeed = playerStats.mySpeedy;
            m_MultiplyVelocityUp = playerStats.multiplyVelocityUp;
            m_MultiplyVelocityDown = playerStats.multiplyVelocityDown;            

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

            float axisAutoMovement = inputVector.y switch
            {
                > 0 => m_MultiplyVelocityUp,
                < 0 => m_MultiplyVelocityDown,
                _ => m_AutoMovement
            };

            var moveDir = new Vector3(inputVector.x, 0, axisAutoMovement);
            if (m_PlayerMaster.GetHasPlayerReady())
                transform.position += moveDir * (m_MovementSpeed * Time.deltaTime);

            m_PlayerMaster.CallEventControllerMovement(inputVector);
        }

        #endregion
        /*
         * Pega o valor do input, normaliza e retorna um vetor para ser usado com o transform do objeto
         */
        public Vector2 GetMovementVector2Normalized()
        {
            var inputVector = m_PlayersInputActions.Player.Move.ReadValue<Vector2>();
            inputVector = inputVector.normalized;

            return inputVector;
        }

        private void TouchMove(InputAction.CallbackContext context) 
        {
            inputVector = context.ReadValue<Vector2>().normalized;            
        }

        private void EndTouchMove(InputAction.CallbackContext context) 
        {
            inputVector = Vector2.zero;            
        }
    }
}
