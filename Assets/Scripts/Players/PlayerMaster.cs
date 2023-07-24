using System;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json.Bson;
namespace RiverAttack
{
    public class PlayerMaster : MonoBehaviour
    {
        float m_AutoMovement;
        float m_MovementSpeed;
        float m_MultiplyVelocityUp;
        float m_MultiplyVelocityDown;

        public enum MovementStatus { None, Paused, Accelerate, Reduce }
        int subWealth { get;
            set; }
        int idPlayer { get;
            set; }
        [SerializeField] float lastSavePosition = 0f;
        protected internal PlayersInputActions playersInputActions;
        GameManager m_GameManager;
        

    #region SerilizedField
        [SerializeField] public bool isPlayerDead = false;
        [SerializeField]
        List<EnemiesResults> enemiesHitList;
        [SerializeField]
        List<EnemiesResults> collectiblesCatchList;
        [SerializeField]
        protected internal MovementStatus playerMovementStatus;
        [SerializeField] PlayerSettings playerSettings;
  #endregion

        
    #region Delagetes
        
        //New Delegates
        public delegate void GeneralEventHandler();
        public event GeneralEventHandler EventPlayerMasterPlayerShoot;
        public event GeneralEventHandler EventPlayerMasterCollider;
        public event GeneralEventHandler EventPlayerMasterOnDestroy;
        public event GeneralEventHandler EventPlayerMasterReSpawn;
        
        public delegate void ControllerEventHandler(Vector2 dir);
        public event ControllerEventHandler EventPlayerMasterControllerMovement;
        
        //Old Delegates

        
        public event GeneralEventHandler EventPlayerBomb;
        public event GeneralEventHandler EventPlayerAddLive;
        
        public delegate void HealthEventHandler(int health);
        public event HealthEventHandler EventIncreaseHealth;
        public event HealthEventHandler EventDecreaseHealth;

        public delegate void SkinChangeEventHandler(ShopProductSkin skin);
        public event SkinChangeEventHandler EventChangeSkin;
    #endregion
        

        #region UNITYMETHODS
        void Awake()
        {
            playersInputActions = new PlayersInputActions();
            playersInputActions.Enable();
            enemiesHitList = new List<EnemiesResults>();
            playerMovementStatus = MovementStatus.Paused;
            m_GameManager = GameManager.instance;
            lastSavePosition = transform.position.z;
        }
        void Start()
        {
            Init();
        }
  #endregion
        void Init()
        {
            isPlayerDead = false;
            int novoLayer = Mathf.RoundToInt(Mathf.Log(GameManager.instance.layerPlayer.value, 2));
            gameObject.layer = novoLayer;
        }
        public void Init(PlayerSettings player, int id)
        {
            isPlayerDead = false;
            int novoLayer = Mathf.RoundToInt(Mathf.Log(GameManager.instance.layerPlayer.value, 2));
            gameObject.layer = novoLayer;
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
        
        public PlayerSettings GetPlayersSettings()
        {
            return playerSettings;
        }

        public bool ShouldPlayerMove() => isPlayerDead == false;
        public bool ShouldPlayerBeReady() => isPlayerDead == false && playerMovementStatus != MovementStatus.Paused;
        public MovementStatus GetActualPlayerStateMovement()
        { 
            return playerMovementStatus;
        }
        public void SetActualPlayerStateMovement(MovementStatus status)
        {
            playerMovementStatus = status;
        }

        protected internal void SetPlayerReady()
        {
            isPlayerDead = false;
        }

        protected internal float GetLastSavePosition() => lastSavePosition;
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
            //m_PlayerStartPosition = new Vector3(position.x, transform.position.y, position.z);
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
        
        //new Calls
        protected internal void CallEventPlayerShoot()
        {
            EventPlayerMasterPlayerShoot?.Invoke();
            
        }
        protected internal void CallEventPlayerMasterCollider()
        {
            EventPlayerMasterCollider?.Invoke();
        }
        protected internal void CallEventPlayerMasterOnDestroy()
        {
            playerMovementStatus = MovementStatus.Paused;
            isPlayerDead = true;
            EventPlayerMasterOnDestroy?.Invoke();
        }
        protected internal void CallEventPlayerMasterReSpawn()
        {
            EventPlayerMasterReSpawn?.Invoke();
        }
        
        public void CallEventPlayerMasterControllerMovement(Vector2 dir)
        {           
            EventPlayerMasterControllerMovement?.Invoke(dir);
        }

        //Old Calls
        
        
        
        protected internal void CallEventPlayerBomb()
        {
            EventPlayerBomb?.Invoke();
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
        
        public void CallEventChangeSkin(ShopProductSkin skin)
        {
            EventChangeSkin?.Invoke(skin);
        }
  #endregion
       
    }
}
