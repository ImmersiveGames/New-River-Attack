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
        [SerializeField] public bool hasPlayerReady = false;
        float m_MultiplyVelocityUp;
        float m_MultiplyVelocityDown;
        [SerializeField]
        private List<EnemiesResults> ListEnemysHit;
        [SerializeField]
        private List<EnemiesResults> ListCollectiblesCatch;
        public int subWealth { get; private set; }
        public enum Status
        {
            None,
            Paused,
            Play,
            Accelerate,
            Reduce,
            Dead,
            Invincible
        }
        [SerializeField]
        protected internal Status playerStatus;
  #endregion

        PlayerController m_PlayerController;
        [SerializeField] PlayerStats playerStats;

        GameManager m_GameManager;
        Vector3 m_StartPlayerPosition;
    #region Delagetes
        public delegate void GeneralEventHandler();
        public event GeneralEventHandler EventPlayerShoot;
        public event GeneralEventHandler EventPlayerDestroy;
        public event GeneralEventHandler EventPlayerReload;
        public event GeneralEventHandler EventPlayerBomb;
        public event GeneralEventHandler EventPlayerAddLive;
        public event GeneralEventHandler EventPlayerHit;

        public delegate void HealthEventHandler(int health);
        public event HealthEventHandler EventIncreaseHealth;
        public event HealthEventHandler EventDecreaseHealth;

        public delegate void ControllerEventHandler(Vector3 dir);
        public event ControllerEventHandler EventControllerMovement;

        public delegate void SkinChangeEventHandler(ShopProductSkin skin);
        public event SkinChangeEventHandler EventChangeSkin;
    #endregion

        [SerializeField]
        private List<EnemiesResults> listEnemiesHit;

    #region UNITYMETHODS
        void Awake()
        {
            m_AutoMovement = GameSettings.instance.autoMovement;
            m_MovementSpeed = playerStats.mySpeedy;
            m_MultiplyVelocityUp = playerStats.multiplyVelocityUp;
            m_MultiplyVelocityDown = playerStats.multiplyVelocityDown;
            m_PlayerController = GetComponent<PlayerController>();
            listEnemiesHit = new List<EnemiesResults>();
            m_StartPlayerPosition = transform.position;
            playerStatus = Status.Paused;
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

        public void ChangeStatus(Status newStatus)
        {
            playerStatus = newStatus;
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

        static void AddResultList(List<EnemiesResults> list, EnemiesScriptable enemy, int qnt = 1)
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
                itemResults = new EnemiesResults(enemy, qnt);
                list.Add(itemResults);
            }
        }
        public void UpdateSavePoint(Vector3 position)
        {
            m_StartPlayerPosition = new Vector3(position.x, transform.position.y, position.z);
            //SaveWealth();
            /*if (GameManager.Instance.firebase.MyFirebaseApp != null && GameManager.Instance.firebase.dependencyStatus == Firebase.DependencyStatus.Available)
            {
                Firebase.Analytics.Parameter[] FinishPath =
                {
                    new Firebase.Analytics.Parameter(Firebase.Analytics.FirebaseAnalytics.ParameterLevelName, gamePlay.GetActualLevel().GetName), new Firebase.Analytics.Parameter("Milstone", gamePlay.GetActualPath())
                };
                Firebase.Analytics.FirebaseAnalytics.LogEvent("FinishPath", FinishPath);
            }*/
            //GameManagerSaves.Instance.SavePlayer(playerSettings);
            //playerSettings.SaveValues();
        }
        public bool CouldCollectItem(int max, CollectibleScriptable collectible)
        {
            EnemiesResults itemResults = ListCollectiblesCatch.Find(x => x.enemy == collectible);
            if (itemResults != null)
            {
                if (max != 0 && itemResults.quantity >= max)
                    return false;
            }
            return true;
        }
        public void AddCollectiblesList(CollectibleScriptable collectibles, int qnt = 1)
        {
            if (collectibles.collectValuable != 0)
            {
                subWealth += collectibles.collectValuable;
            }
            AddResultList(ListCollectiblesCatch, collectibles, qnt);
        }
        public void AddHitList(EnemiesScriptable obstacle, int qnt = 1)
        {
            AddResultList(ListEnemysHit, obstacle, qnt);
            AddResultList(GamePlaySettings.instance.HitEnemys, obstacle, qnt);
        }

        #region Calls
        protected internal void CallEventPlayerShoot()
        {
            EventPlayerShoot?.Invoke();
        }
        protected internal void CallEventPlayerHit()
        {
            EventPlayerHit?.Invoke();
        }
        protected internal void CallEventPlayerDestroy()
        {
            EventPlayerDestroy?.Invoke();
        }
        protected internal void CallEventPlayerBomb()
        {
            EventPlayerBomb?.Invoke();
        }
        protected internal void CallEventPlayerReload()
        {
            EventPlayerReload?.Invoke();
        }
        protected internal void CallEventIncreaseHealth(int health)
        {
            EventIncreaseHealth?.Invoke(health);
        }
        protected internal void CallEventDecreaseHealth(int health)
        {
            EventDecreaseHealth?.Invoke(health);
        }
        protected internal void CallEventPlayerAddLive()
        {
            EventPlayerAddLive?.Invoke();
        }
  #endregion

    }
}
