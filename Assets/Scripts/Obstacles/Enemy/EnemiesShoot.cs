using System;
using UnityEngine;
using Utils;
namespace RiverAttack
{
    [RequireComponent(typeof(EnemiesMaster),typeof(Renderer))]
    public class EnemiesShoot : ObstacleDetectApproach, IShoot, IHasPool
    {
        [Tooltip("Identifica se o jogador em modo rapidfire")]
        [SerializeField] bool canShoot;
        [SerializeField] bool canShootByApproach;
        enum ShootState
        {
            Hold,
            Shooting,
            Patrolling
        }
        [SerializeField] ShootState activeShootState;
        [SerializeField] GameObject bullet;
        [SerializeField] int startPool;
        [SerializeField] float shootCadence;
        const float START_TO_SHOOT = 0.2f;
        [SerializeField] Transform target;
        

        [Header("Bullet Settings")]
        [SerializeField]
        float bulletSpeed;
        [SerializeField] float bulletLifeTime;

        //private ControllerMap controllerMap;
        float m_NextShoot;
        GameObject m_MyShoot;
        Transform m_SpawnPoint;
        GamePlayManager m_GamePlayManager;
        EnemiesMaster m_EnemiesMaster;
        EnemiesSetDifficulty m_EnemyDifficult;
        Renderer m_Renderer;

        #region UNITY METHODS
        void OnEnable()
        {
            SetInitialReferences();
            activeShootState = (canShoot) ? ShootState.Shooting : ShootState.Hold;
            activeShootState = (canShootByApproach) ? ShootState.Patrolling : activeShootState;
            m_EnemiesMaster.EventDestroyEnemy += StopFire;
            m_GamePlayManager.EventEnemyDestroyPlayer += StopFire;
            m_GamePlayManager.EventResetEnemies += StartFire;
            StartMyPool();
        }
        void Start()
        {
            playerApproachRadius = SetPlayerApproachRadius();
            if (m_EnemiesMaster == null) return;
            LogInitialShootStatus(m_EnemiesMaster.enemy);
            
            canShoot = playerApproachRadius == 0;
            
            if (m_EnemiesMaster.enemy == null || m_EnemiesMaster.enemy.enemiesSetDifficultyListSo == null) return;
            DifficultUpdates();
            
            if (playerApproachRadius != 0) InvokeRepeating(nameof(HasPlayerApproach), 0, timeToCheck);
        }
        void OnBecameVisible()
        {
            if (IsInvoking(nameof(Fire))) return;
            InvokeRepeating(nameof(Fire), START_TO_SHOOT, shootCadence);
        }
        void OnBecameInvisible()
        {
            CancelInvoke(nameof(Fire));
        }
        void LateUpdate()
        {
            if (!canShoot || !GamePlayManager.instance.shouldBePlayingGame || activeShootState != ShootState.Shooting || m_EnemiesMaster.isDestroyed) return;
            if (m_EnemiesMaster != null && m_EnemyDifficult.enemyDifficult != m_EnemiesMaster.getDifficultName)
                DifficultUpdates();
            activeShootState = ShootState.Shooting;
            Fire();
        }
        void OnDisable()
        {
            m_EnemiesMaster.EventDestroyEnemy -= StopFire;
            //m_GamePlayManager.EventEnemyDestroyPlayer -= StopFire;
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
            
        }
        public void Fire()
        {
            if (!shouldBeFire || !m_GamePlayManager.shouldBePlayingGame || !m_EnemiesMaster.ShouldEnemyBeReady())
                return;
            m_MyShoot = PoolObjectManager.GetObject(this);
            m_MyShoot.transform.parent = null;
            var transform1 = m_SpawnPoint;
            var transformPosition = transform1.position;
            var transformRotation = transform1.rotation;
            m_MyShoot.transform.position = new Vector3(transformPosition.x, transformPosition.y, transformPosition.z);
            m_MyShoot.transform.rotation = new Quaternion(transformRotation.x, transformRotation.y, transformRotation.z, transformRotation.w);
        }
        public void SetTarget(Transform toTarget)
        {
            target = toTarget;
        }

        void StopFire()
        {
            CancelInvoke(nameof(Fire));
        }

        void StartFire()
        {
            if (IsInvoking(nameof(Fire))) return;
            InvokeRepeating(nameof(Fire), START_TO_SHOOT, shootCadence);
        }
        bool shouldBeFire
        {
            get
            {
                return shootCadence > 0 && bulletSpeed > 0 && activeShootState == ShootState.Shooting;
            }
        }
        public void StartMyPool(bool isPersistent = false)
        {
            PoolObjectManager.CreatePool(this, bullet, startPool, transform, isPersistent);
            var pool = PoolObjectManager.GetPool(this);
            for (int i = 0; i < pool.childCount; i++)
            {
                var bulletEnemy = pool.GetChild(i).GetComponent<BulletEnemy>();
                bulletEnemy.ownerShoot = m_EnemiesMaster;
                bulletEnemy.SetMyPool(pool);
                bulletEnemy.Init(bulletSpeed, bulletLifeTime);
                pool.GetChild(i).transform.position = m_SpawnPoint.transform.position;
                pool.GetChild(i).transform.rotation = m_SpawnPoint.transform.rotation;
                /*bulletEnemy.spawnPoint = spawnPoint.position;
                bulletEnemy.target = target;*/
                /*if (hasTarget)
                bulletEnemy.transform.LookAt(target); */
            }
        }
        protected override void HasPlayerApproach()
        {
            SetTarget(FindTarget<PlayerMaster>(GameManager.instance.layerPlayer));
            canShoot = target != null;
        }
        protected override void DifficultUpdates()
        {
            m_EnemyDifficult = m_EnemiesMaster.enemy.enemiesSetDifficultyListSo.GetDifficultByEnemyDifficult(m_EnemiesMaster.getDifficultName);
            shootCadence = m_EnemiesMaster.enemy.shootCadence * m_EnemyDifficult.multiplyEnemiesShootCadence;
            
            if (canShootByApproach == false || (m_EnemiesMaster.enemy.radiusToApproach == 0 || playerApproachRadius == 0)) return;
            if(m_EnemiesMaster.enemy.radiusToApproach != 0 && m_EnemyDifficult.multiplyPlayerDistanceRadius != 0)
                playerApproachRadius = m_EnemiesMaster.enemy.radiusToApproach * m_EnemyDifficult.multiplyPlayerDistanceRadius;
        }
        void LogInitialShootStatus(EnemiesScriptable enemy)
        {
            enemy.shootCadence = shootCadence;
            enemy.radiusToApproach = playerApproachRadius;
        }
    
    }
}
