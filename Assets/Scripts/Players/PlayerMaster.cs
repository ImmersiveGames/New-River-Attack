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
        List<EnemiesResults> enemiesHitList;
        [SerializeField]
        List<EnemiesResults> collectiblesCatchList;
        int subWealth { get;
            set; }
        
        int idPlayer
        {
            get;
            set;
        }
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
        [SerializeField] PlayerSettings playerSettings;

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
        public delegate void ControllerEventHandler(Vector2 dir);
        public event ControllerEventHandler EventControllerMovement;

        public delegate void SkinChangeEventHandler(ShopProductSkin skin);
        public event SkinChangeEventHandler EventChangeSkin;
    #endregion

        public bool GetHasPlayerReady() { return hasPlayerReady; }
        public bool SetHasPlayerReady(bool set) { return hasPlayerReady = set; }

        #region UNITYMETHODS
        void Awake()
        {
            m_PlayerController = GetComponent<PlayerController>();
            enemiesHitList = new List<EnemiesResults>();
            m_StartPlayerPosition = transform.position;
            playerStatus = Status.Paused;
            m_GameManager = GameManager.instance;
        }

  #endregion

        public void Init(PlayerSettings player, int id)
        {
            /*idPlayer = id;
            playerSettings = player;
            name = playerSettings.name;
            var gameSettings = GameSettings.instance;
            gameObject.layer = gameSettings.layerPlayer;
            m_StartPlayerPosition = playerSettings.spawnPosition;
            transform.position = playerSettings.spawnPosition;
            transform.rotation = Quaternion.Euler(playerSettings.spawnRotation);
            playerSettings.lives = playerSettings.startLives;
            if (playerSettings.lives <= 0 && gameSettings.gameMode.modeId == gameSettings.GetGameModes(0).modeId)
                playerSettings.InitPlayer();
            else if (playerSettings.lives.Value <= 0 && gameSettings.gameMode.modeId != gameSettings.GetGameModes(0).modeId)
                playerSettings.ChangeLife(1);
            if (gameSettings.gameMode.modeId == gameSettings.GetGameModes(0).modeId ||
                GameManager.Instance.levelsFinish.Count <= 0)
                playerSettings.score.SetValue(0);
            GameManagerSaves.Instance.LoadPlayer(ref playerSettings);
            //playerSettings.LoadValues(); */
        }
        
        public void ChangeStatus(Status newStatus)
        {
            playerStatus = newStatus;
        }
        public PlayerSettings GetPlayersSettings()
        {
            return playerSettings;
        }
        public void AllowedMove(bool allowed = true)
        {
            hasPlayerReady = allowed;
        }
        public bool shouldPlayerBeReady
        { get
            {
                return GamePlayManager.instance.shouldBePlayingGame && hasPlayerReady == true;
            }
        }
        public void AddEnemiesHitList(EnemiesScriptable obstacle, int qnt = 1)
        {
            AddResultList(enemiesHitList, obstacle, qnt);
            AddResultList(GamePlaySettings.instance.hitEnemiesResultsList, obstacle, qnt);
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
            var itemResults = collectiblesCatchList.Find(x => x.enemy == collectible);
            if (itemResults == null)
                return true;
            return max == 0 || itemResults.quantity < max;
        }
        public void AddCollectiblesList(CollectibleScriptable collectibles, int qnt = 1)
        {
            if (collectibles.collectValuable != 0)
            {
                subWealth += collectibles.collectValuable;
            }
            AddResultList(collectiblesCatchList, collectibles, qnt);
        }
        public void AddHitList(EnemiesScriptable obstacle, int qnt = 1)
        {
            AddResultList(enemiesHitList, obstacle, qnt);
            AddResultList(GamePlaySettings.instance.hitEnemiesResultsList, obstacle, qnt);
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
        public void CallEventControllerMovement(Vector2 dir)
        {           
            EventControllerMovement?.Invoke(dir);
        }
  #endregion
        protected virtual void CallEventChangeSkin(ShopProductSkin skin)
        {
            EventChangeSkin?.Invoke(skin);
        }
    }
}
