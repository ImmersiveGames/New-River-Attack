using UnityEngine;
using UnityEngine.InputSystem;

namespace RiverAttack
{
    public class PlayerController : MonoBehaviour
    {
        float m_AutoMovement;
        float m_MovementSpeed;
        float m_MultiplyVelocityUp;
        float m_MultiplyVelocityDown;

        Vector2 m_InputVector;
        PlayersInputActions m_PlayersInputActions;
        PlayerMaster m_PlayerMaster;
        GamePlayManager m_GamePlayManager;

        #region UNITYMETHODS
        void OnEnable()
        {
            SetInitialReferences();
            SetValuesFromPlayerSettings(m_PlayerMaster.getPlayerSettings);
        }
        void Start()
        {
            m_PlayersInputActions = m_PlayerMaster.playersInputActions;
            m_PlayersInputActions.Enable();
            m_PlayersInputActions.Player.Move.performed += TouchMove;
            m_PlayersInputActions.Player.Move.canceled += EndTouchMove;
        }
        void FixedUpdate()
        {
            if (!m_GamePlayManager.shouldBePlayingGame || !m_PlayerMaster.shouldPlayerBeReady) return;

            float axisAutoMovement = m_InputVector.y switch
            {
                > 0 => m_MultiplyVelocityUp,
                < 0 => m_MultiplyVelocityDown,
                _ => m_AutoMovement
            };

            m_PlayerMaster.playerMovementStatus = m_InputVector.y switch
            {
                > 0 => PlayerMaster.MovementStatus.Accelerate,
                < 0 => PlayerMaster.MovementStatus.Reduce,
                _ => PlayerMaster.MovementStatus.None
            };

            var moveDir = new Vector3(m_InputVector.x, 0, axisAutoMovement);
            if (m_GamePlayManager.getGodMode && m_GamePlayManager.godModeSpeed) moveDir *= 4;
            transform.position += moveDir * (m_MovementSpeed * Time.deltaTime);

            m_PlayerMaster.OnEventPlayerMasterControllerMovement(m_InputVector);
        }
  #endregion

        void SetInitialReferences()
        {
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_GamePlayManager = GamePlayManager.instance;
        }
        void SetValuesFromPlayerSettings(PlayerSettings settings)
        {
            //TODO: >>Provavelmente<< aqui é o melhor local para fazer alteração de valores usando o powerUp
            m_AutoMovement = settings.speedVertical;
            m_MovementSpeed = settings.mySpeedy;
            m_MultiplyVelocityUp = settings.multiplyVelocityUp;
            m_MultiplyVelocityDown = settings.multiplyVelocityDown;
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
