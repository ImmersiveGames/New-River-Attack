using ImmersiveGames;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RiverAttack
{
    public class PlayerController : MonoBehaviour
    {
        private float m_AutoMovement;
        private float m_MovementSpeed;
        private float m_MultiplyVelocityUp;
        private float m_MultiplyVelocityDown;

        private Vector2 m_InputVector;
        private PlayersInputActions m_PlayersInputActions;
        private PlayerMasterOld _mPlayerMasterOld;
        private GamePlayManager m_GamePlayManager;

        #region UNITYMETHODS

        private void OnEnable()
        {
            SetInitialReferences();
            SetValuesFromPlayerSettings(_mPlayerMasterOld.getPlayerSettings);
        }

        private void Start()
        {
            m_PlayersInputActions = _mPlayerMasterOld.playersInputActions;
            m_PlayersInputActions.Enable();
            m_PlayersInputActions.Player.Axis_Move.performed += TouchMove;
            m_PlayersInputActions.Player.Axis_Move.canceled += EndTouchMove;
        }

        private void FixedUpdate()
        {
            if (!m_GamePlayManager.shouldBePlayingGame || !_mPlayerMasterOld.ShouldPlayerBeReady) return;

            float axisAutoMovement = m_InputVector.y switch
            {
                > 0 => m_MultiplyVelocityUp,
                < 0 => m_MultiplyVelocityDown,
                _ => m_AutoMovement
            };

            _mPlayerMasterOld.playerMovementStatus = m_InputVector.y switch
            {
                > 0 => PlayerMasterOld.MovementStatus.Accelerate,
                < 0 => PlayerMasterOld.MovementStatus.Reduce,
                _ => PlayerMasterOld.MovementStatus.None
            };
            
            var moveDir = new Vector3(m_InputVector.x, 0, axisAutoMovement);
            if (m_GamePlayManager.getGodMode && m_GamePlayManager.godModeSpeed) moveDir *= 4;
            if (m_GamePlayManager.bossFight)
            {
                float axisX = m_InputVector.x;
                float axisZ = axisAutoMovement;
                var nextPosition = transform.position + moveDir * (m_MovementSpeed * Time.deltaTime);
                if (m_InputVector.x != 0)
                {
                    if (nextPosition.x is >= GamePlayManager.LIMIT_X or <= GamePlayManager.LIMIT_X * -1)
                    {
                        axisX = 0;
                    }
                }
                if (m_InputVector.y != 0)
                {
                    if (nextPosition.z is >= GamePlayManager.LIMIT_Z_TOP or <= GamePlayManager.LIMIT_Z_BOTTOM)
                    {
                        axisZ = 0;
                    }
                }
                moveDir = new Vector3(axisX, 0, axisZ);
            }
            transform.position += moveDir * (m_MovementSpeed * Time.deltaTime);

            _mPlayerMasterOld.OnEventPlayerMasterControllerMovement(m_InputVector);
        }
  #endregion

  private void SetInitialReferences()
        {
            _mPlayerMasterOld = GetComponent<PlayerMasterOld>();
            m_GamePlayManager = GamePlayManager.instance;
        }

        private void SetValuesFromPlayerSettings(PlayerSettings settings)
        {
            m_AutoMovement = (!m_GamePlayManager.bossFight) ? settings.speedVertical : 0f;
            m_MovementSpeed = settings.mySpeedy;
            m_MultiplyVelocityUp = (!m_GamePlayManager.bossFight)? settings.multiplyVelocityUp : 1f;
            m_MultiplyVelocityDown = (!m_GamePlayManager.bossFight)? settings.multiplyVelocityDown : -1f;
        }

        private void TouchMove(InputAction.CallbackContext context)
        {
            m_InputVector = context.ReadValue<Vector2>().normalized;
        }

        private void EndTouchMove(InputAction.CallbackContext context)
        {
            m_InputVector = Vector2.zero;
        }
    }
}
