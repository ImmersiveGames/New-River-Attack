using UnityEngine;
using Utils;
namespace RiverAttack
{
    [RequireComponent(typeof(EnemiesMaster),typeof(Renderer))]
    public class EnemiesShoot : ObstacleDetectApproach, IHasPool
    {
        [SerializeField] GameObject bullet;
        [SerializeField] int startPool;
        [SerializeField] float shootCadence;
        [Header("Bullet Settings")]
        [SerializeField]
        float bulletSpeed;
        [SerializeField] float bulletLifeTime;
        Transform m_Target;

        #region IShoots
        readonly StateShoot m_StateShoot = new StateShoot();
        readonly StateShootHold m_StateShootHold = new StateShootHold();
        readonly StateShootPatrol m_StateShootPatrol = new StateShootPatrol();
  #endregion
        IShoot m_ActualState;
        
        GamePlayManager m_GamePlayManager;
        EnemiesMaster m_EnemiesMaster;
        EnemiesSetDifficulty m_EnemyDifficult;
        MeshRenderer m_Renderer;
        Transform m_SpawnPoint;

        #region UNITYMETHODS
        void OnEnable()
        {
            SetInitialReferences();
            m_EnemiesMaster.EventDestroyEnemy += StopFire;
            m_GamePlayManager.EventEnemyDestroyPlayer += StopFire;
            //m_GamePlayManager.EventResetEnemies += StartFire;
        }
        void Start()
        {
            // setup inicial do status
            StartMyPool();
            ChangeState(m_StateShootHold);
        }

        void Update()
        {
            if (!m_GamePlayManager.shouldBePlayingGame || !m_EnemiesMaster.shouldEnemyBeReady || m_EnemiesMaster.isDestroyed || !m_Renderer.isVisible)
                return;
            switch (shouldBeFire)
            {
                case true when shouldBeFireByApproach && !m_Target:
                    m_Target = null;
                    ChangeState(m_StateShootPatrol);
                    m_Target = m_StateShootPatrol.target;
                    break;
                case true:
                    ChangeState(m_StateShoot);
                    break;
                case false:
                    m_Target = null;
                    ChangeState(m_StateShootHold);
                    break;
            }
            m_ActualState.UpdateState();
        }
        void OnDisable()
        {
            m_EnemiesMaster.EventDestroyEnemy -= StopFire;
            m_GamePlayManager.EventEnemyDestroyPlayer -= StopFire;
            //m_GamePlayManager.EventResetEnemies -= StartFire;
        }
  #endregion
        void SetInitialReferences()
        {
            m_Renderer = GetComponent<MeshRenderer>();
            if (m_Renderer == null)
                m_Renderer = gameObject.AddComponent<MeshRenderer>();
            m_GamePlayManager = GamePlayManager.instance;
            m_EnemiesMaster = GetComponent<EnemiesMaster>();
            m_SpawnPoint = GetComponentInChildren<EnemiesShootSpawn>().transform ? GetComponentInChildren<EnemiesShootSpawn>().transform : transform;
            // Set States
            playerApproachRadius = SetPlayerApproachRadius();
            m_StateShootPatrol.SetPatrol(playerApproachRadius,timeToCheck);
            m_StateShoot.SetBullet(shootCadence, bulletSpeed,bulletLifeTime, this);
            m_StateShoot.SetSpawnPoint(m_SpawnPoint);
        }
        void ChangeState(IShoot newState)
        {
            if (m_ActualState == newState) return;
            m_ActualState?.ExitState();

            m_ActualState = newState;
            m_ActualState?.EnterState(m_EnemiesMaster);
        }
        
        void StopFire()
        {
            m_Target = null;
            ChangeState(m_StateShootHold);
        }
        bool shouldBeFire
        {
            get
            {
                return shootCadence > 0 && bulletSpeed > 0;
            }
        }
        bool shouldBeFireByApproach
        {
            get
            {
                return playerApproachRadius != 0 || playerApproachRadiusRandom != Vector2.zero;
            }
        }
        public void StartMyPool(bool isPersistent = false)
        {
            PoolObjectManager.CreatePool(this, bullet, startPool, transform, isPersistent);
        }
    }
}
