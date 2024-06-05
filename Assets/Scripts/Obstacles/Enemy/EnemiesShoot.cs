using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class EnemiesShoot : ObstacleDetectApproach, IHasPool
    {
        [SerializeField] private GameObject bullet;
        [SerializeField] private int startPool;
        [SerializeField] internal float shootCadence;
        [Header("Bullet Settings")]
        [SerializeField]
        internal float bulletSpeed;
        [SerializeField] internal float bulletLifeTime;

        #region IShoots

        private readonly StateShoot m_StateShoot;
        private readonly StateShootHold m_StateShootHold;
        private readonly StateShootPatrol m_StateShootPatrol;
        private IShoot m_ActualState;
    #endregion

    private GamePlayManager m_GamePlayManager;
    private EnemiesMasterOld _mEnemiesMasterOld;
    private EnemiesSetDifficulty m_EnemyDifficult;

        internal Transform spawnPoint;

        public EnemiesShoot()
        {
            m_StateShoot = new StateShoot(this);
            m_StateShootHold = new StateShootHold();
            m_StateShootPatrol = new StateShootPatrol(this, null);
        }

        #region UNITYMETHODS

        private void OnEnable()
        {
            SetInitialReferences();
            _mEnemiesMasterOld.EventObstacleMasterHit += StopFire;
            m_GamePlayManager.EventEnemiesMasterKillPlayer += StopFire;
        }

        private void Start()
        {
            // setup inicial do status
            StartMyPool(bullet,startPool);
            ChangeState(m_StateShootHold);
            var enemiesShootSpawn = GetComponentInChildren<EnemiesShootSpawn>();
            if (enemiesShootSpawn == null) return;
            spawnPoint = enemiesShootSpawn.transform ? enemiesShootSpawn.transform : transform;
        }

        private void Update()
        {
            if (!m_GamePlayManager.shouldBePlayingGame || !_mEnemiesMasterOld.shouldObstacleBeReady || _mEnemiesMasterOld.isDestroyed || !meshRenderer.isVisible)
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

        private void OnDisable()
        {
            _mEnemiesMasterOld.EventObstacleMasterHit -= StopFire;
            m_GamePlayManager.EventEnemiesMasterKillPlayer -= StopFire;
        }
  #endregion
        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            m_GamePlayManager = GamePlayManager.instance;
            _mEnemiesMasterOld = GetComponent<EnemiesMasterOld>();
        }

        private void ChangeState(IShoot newState)
        {
            if (m_ActualState == newState) return;
            m_ActualState?.ExitState();

            m_ActualState = newState;
            m_ActualState?.EnterState(_mEnemiesMasterOld);
        }

        private void StopFire()
        {
            target = null;
            ChangeState(m_StateShootHold);
        }

        private bool shouldBeFire
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
