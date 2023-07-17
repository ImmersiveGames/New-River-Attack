using System;
using UnityEngine;
using UnityEngine.Serialization;
namespace RiverAttack
{
    public class PlayerMaster : MonoBehaviour
    {
    #region SerilizedField
        float m_AutoMovement;
        float m_MovementSpeed;
        [SerializeField] bool hasPlayerReady = false;
        float m_MultiplyVelocityUp;
        float m_MultiplyVelocityDown;
  #endregion

        PlayerController m_PlayerController;
        [SerializeField] PlayerStats playerStats;

        GameManager m_GameManager;
   

    #region UNITYMETHODS
        void Awake()
        {
            m_AutoMovement = GameSettings.instance.autoMovement;
            m_MovementSpeed = playerStats.mySpeedy;
            m_MultiplyVelocityUp = playerStats.multiplyVelocityUp;
            m_MultiplyVelocityDown = playerStats.multiplyVelocityDown;
            m_PlayerController = GetComponent<PlayerController>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (GameManager.instance.GetStates() != GameManager.States.GamePlay || GameManager.instance.GetPaused())
            {
                hasPlayerReady = false;
                return;
            }
            hasPlayerReady = true;
            var inputVector = m_PlayerController.GetMovementVector2Normalized();
            float axisAutoMovement = inputVector.y switch
            {
                > 0 => m_MultiplyVelocityUp,
                < 0 => m_MultiplyVelocityDown,
                _ => m_AutoMovement
            };

            var moveDir = new Vector3(inputVector.x, 0, axisAutoMovement);
            if (hasPlayerReady)
                transform.position += moveDir * (m_MovementSpeed * Time.deltaTime);
        }
  #endregion

        public void Init(PlayerStats player, int id)
        {
            
        }

        public PlayerStats PlayersSettings()
        {
            return playerStats;
    }
        public void AllowedMove(bool allowed = true)
        {
            hasPlayerReady = allowed;
        }
        public bool shouldPlayerBeReady
        {
            get
            {
                return GamePlayManager.instance.shouldBePlayingGame && hasPlayerReady == true;
            }
        }
    }
}