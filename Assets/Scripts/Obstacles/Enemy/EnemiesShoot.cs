using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class EnemiesShoot : ObstacleDetectApproach, IHasPool
    {
        [SerializeField] GameObject bullet;
        [SerializeField] int startPool;
        [SerializeField] internal float shootCadence;
        [Header("Bullet Settings")]
        [SerializeField]
        internal float bulletSpeed;
        [SerializeField] internal float bulletLifeTime;

        #region IShoots
        readonly StateShoot m_StateShoot;
        readonly StateShootHold m_StateShootHold;
        readonly StateShootPatrol m_StateShootPatrol;
        IShoot m_ActualState;
    #endregion

        GamePlayManager m_GamePlayManager;
        EnemiesMaster m_EnemiesMaster;
        EnemiesSetDifficulty m_EnemyDifficult;

        internal Transform spawnPoint;

        public EnemiesShoot()
        {
            m_StateShoot = new StateShoot(this);
            m_StateShootHold = new StateShootHold();
            m_StateShootPatrol = new StateShootPatrol(this, null);
        }

        #region UNITYMETHODS
        void OnEnable()
        {
            SetInitialReferences();
            m_EnemiesMaster.EventObstacleMasterHit += StopFire;
            m_GamePlayManager.EventEnemiesMasterKillPlayer += StopFire;
        }
        void Start()
        {
            // setup inicial do status
            StartMyPool(bullet,startPool);
            ChangeState(m_StateShootHold);
            var enemiesShootSpawn = GetComponentInChildren<EnemiesShootSpawn>();
            if (enemiesShootSpawn == null) return;
            spawnPoint = enemiesShootSpawn.transform ? enemiesShootSpawn.transform : transform;
        }

        void Update()
        {
            if (!m_GamePlayManager.shouldBePlayingGame || !m_EnemiesMaster.shouldObstacleBeReady || m_EnemiesMaster.isDestroyed || !meshRenderer.isVisible)
                return;
            switch (shouldBeFire)
            {
                case true when shouldBeApproach && !target:
                    target = null;
                    ChangeState(m_StateShootPatrol);
                    target = m_StateShootPatrol.target;
                    break;
                case true:
                    ChangeState(m_StateShoot);
                    break;
                case false:
                    target = null;
                    ChangeState(m_StateShootHold);
                    break;
            }
            m_ActualState.UpdateState();
        }
        void OnDisable()
        {
            m_EnemiesMaster.EventObstacleMasterHit -= StopFire;
            m_GamePlayManager.EventEnemiesMasterKillPlayer -= StopFire;
        }
  #endregion
        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            m_GamePlayManager = GamePlayManager.instance;
            m_EnemiesMaster = GetComponent<EnemiesMaster>();
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
            target = null;
            ChangeState(m_StateShootHold);
        }
        bool shouldBeFire
        {
            get
            {
                return shootCadence > 0 && bulletSpeed > 0;
            }
        }
        public void StartMyPool(GameObject bullets, int quantity, bool isPersistent = false)
        {
            PoolObjectManager.CreatePool(this, bullets, quantity, transform, isPersistent);
        }
    }
}
