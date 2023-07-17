using System;
using UnityEngine;
using System.Collections.Generic;
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
    #region Delagetes
        public delegate void GeneralEventHandler();
        public event GeneralEventHandler EventPlayerShoot;
        public event GeneralEventHandler EventPlayerDestroy;
        public event GeneralEventHandler EventPlayerReload;
        public event GeneralEventHandler EventPlayerBomb;
        public event GeneralEventHandler EventPlayerAddLive;
        public event GeneralEventHandler EventPlayerHit;

        public delegate void HealthEventHandler(int health);
        public event HealthEventHandler EventIncresceHealth;
        public event HealthEventHandler EventDecresceHealth;

        public delegate void ControllerEventHandler(Vector3 dir);
        public event ControllerEventHandler EventControllerMovement;

        //public delegate void SkinChangeEventHandler(ShopProductSkin skin);
        //public event SkinChangeEventHandler EventChangeSkin;

    #endregion
        
        [SerializeField]
        private List<global::EnemiesResults> listEnemiesHit;

    #region UNITYMETHODS
        void Awake()
        {
            m_AutoMovement = GameSettings.instance.autoMovement;
            m_MovementSpeed = playerStats.mySpeedy;
            m_MultiplyVelocityUp = playerStats.multiplyVelocityUp;
            m_MultiplyVelocityDown = playerStats.multiplyVelocityDown;
            m_PlayerController = GetComponent<PlayerController>();
            listEnemiesHit = new List<global::EnemiesResults>();
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
        public void AddEnemiesHitList(EnemiesScriptable obstacle, int qnt = 1)
        {
            AddResultList(listEnemiesHit, obstacle, qnt);
            AddResultList(GamePlaySettings.instance.HitEnemys, obstacle, qnt);
        }

        static void AddResultList(List<global::EnemiesResults> list, EnemiesScriptable enemy, int qnt = 1)
        {
            var itemResults = list.Find(x => x.enemy == enemy);
            if (itemResults != null)
            {
                if (enemy.GetType() == typeof(CollectibleScriptable))
                {
                    var collectibles = (CollectibleScriptable)enemy;
                    if (itemResults.quantity + qnt < collectibles.maxCollectible)
                        itemResults.quantity += qnt;
                    else
                        itemResults.quantity = collectibles.maxCollectible;
                }
                else
                    itemResults.quantity += qnt;
            }
            else
            {
                itemResults = new global::EnemiesResults(enemy, qnt);
                list.Add(itemResults);
            }
        }

        #region Calls
        public void CallEventPlayerShoot()
        {
            EventPlayerShoot?.Invoke();
        }
        public void CallEventPlayerHit()
        {
            EventPlayerHit?.Invoke();
        }
  #endregion
    }
}