﻿using UnityEngine;
using Utils;
namespace RiverAttack
{
    [RequireComponent(typeof(EnemiesMaster),typeof(Renderer))]
    public class EnemiesShoot : ObstacleDetectApproach, IShoot, IHasPool
    {
        [Tooltip("Identifica se o jogador em modo rapidfire")]
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
        float m_ShootCadence;
        const float START_TO_SHOOT = 0.2f;
        [SerializeField] Transform target;
        

        [Header("Bullet Settings")]
        [SerializeField]
        float bulletSpeed;
        [SerializeField] float bulletLifeTime;

        //private ControllerMap controllerMap;
        GameObject m_MyShoot;
        Transform m_SpawnPoint;
        GamePlayManager m_GamePlayManager;
        EnemiesMaster m_EnemiesMaster;
        EnemiesSetDifficulty m_EnemyDifficult;
        MeshRenderer m_Renderer;

        #region UNITY METHODS
        void OnEnable()
        {
            SetInitialReferences();
            activeShootState = SetActualShootState();
            m_EnemiesMaster.EventDestroyEnemy += StopFire;
            m_GamePlayManager.EventEnemyDestroyPlayer += StopFire;
            m_GamePlayManager.EventResetEnemies += StartFire;
        }
        void Start()
        {
            StartMyPool();
            playerApproachRadius = SetPlayerApproachRadius();
            if (m_EnemiesMaster == null) return;
            if (m_EnemiesMaster.enemy == null || m_EnemiesMaster.enemy.enemiesSetDifficultyListSo == null) return;
            DifficultUpdates();
            //if (playerApproachRadius != 0) InvokeRepeating(nameof(HasPlayerApproach), 0, timeToCheck);
        }
        void OnBecameVisible()
        {
            activeShootState = SetActualShootState();
            if (IsInvoking(nameof(Fire))) return;
            InvokeRepeating(nameof(Fire), START_TO_SHOOT, shootCadence);
        }
        void OnBecameInvisible()
        {
            //activeShootState = SetActualShootState();
            CancelInvoke(nameof(Fire));
        }
        void Update()
        {
            if (!m_GamePlayManager.shouldBePlayingGame || !m_EnemiesMaster.shouldEnemyBeReady || m_EnemiesMaster.isDestroyed) return;
            if (m_EnemyDifficult.enemyDifficult != m_EnemiesMaster.getDifficultName)
                DifficultUpdates();

            if (activeShootState != ShootState.Patrolling) return;
            if (!m_Renderer.isVisible) return;
            HasPlayerApproach();
            Debug.Log("Patrulhando: "+ transform.name);
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
            activeShootState = ShootState.Hold;
            m_ShootCadence = shootCadence;
            startApproachRadius = SetPlayerApproachRadius();
        }
        public void Fire()
        {
            ///////
            /// Mudar o tempo do Fire esta somando as duas cadencia quando esta em patrulling
            /// 
            //activeShootState = SetActualShootState();
            Debug.Log(" Attempt to FIRE: " + transform.name);
            if (!m_GamePlayManager.shouldBePlayingGame || !m_EnemiesMaster.shouldEnemyBeReady || m_EnemiesMaster.isDestroyed) return;
            if (!shouldBeFire) return;
            if (playerApproachRadius == 0 && playerApproachRadiusRandom == Vector2.zero)
                activeShootState = ShootState.Shooting;
            if (shouldBeFireByApproach) activeShootState = (target)? ShootState.Shooting:ShootState.Patrolling;
            if (activeShootState != ShootState.Shooting) return;

            Debug.Log("FIRE: "+ transform.name);
            //Pick a bullet
            m_MyShoot = PoolObjectManager.GetObject<BulletEnemy>(this, m_EnemiesMaster);
            //setting bullet entity
            var myBullet = m_MyShoot.GetComponent<BulletEnemy>();
            myBullet.Init(bulletSpeed, bulletLifeTime);
            //Deattached bullet
            var myShootTransform = m_MyShoot.transform;
            myShootTransform.parent = null;
            TransformBullets(ref myShootTransform, m_SpawnPoint);
            /*
            
            // O jogo permite atirar.
            //Checar se o inimigo pode atirar
            if (!shouldBeFire || !shouldBeFireByApproach) return;
            if (shouldBeFireByApproach && activeShootState == ShootState.Patrolling && target == null) return;
            activeShootState = ShootState.Shooting;
                Debug.Log("FIRE");
            //Pick a bullet
            m_MyShoot = PoolObjectManager.GetObject<BulletEnemy>(this, m_EnemiesMaster);
            //setting bullet entity
            var myBullet = m_MyShoot.GetComponent<BulletEnemy>();
            myBullet.Init(bulletSpeed, bulletLifeTime);
            //Deattached bullet
            var myShootTransform = m_MyShoot.transform;
            myShootTransform.parent = null;
            TransformBullets(ref myShootTransform, m_SpawnPoint);

            bulletEnemy.spawnPoint = spawnPoint.position;
                bulletEnemy.target = target;*/
            /*if (hasTarget)
            bulletEnemy.transform.LookAt(target); */
        }

        void TransformBullets(ref Transform transformBullet, Transform transformSpawn)
        {
            if (transformSpawn == null) transformSpawn = transform;
            var transformPosition = transformSpawn.position;
            var transformRotation = transformSpawn.rotation;
            var bulletTransform = transformBullet;
            bulletTransform.position = new Vector3(transformPosition.x, transformPosition.y, transformPosition.z);
            bulletTransform.rotation = new Quaternion(transformRotation.x, transformRotation.y, transformRotation.z, transformRotation.w);
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
                return shootCadence > 0 && bulletSpeed > 0;
            }
        }
        bool shouldBeFireByApproach
        {
            get
            {
                return shouldBeFire && (playerApproachRadius != 0 || playerApproachRadiusRandom != Vector2.zero);
            }
        }
        public void StartMyPool(bool isPersistent = false)
        {
            PoolObjectManager.CreatePool(this, bullet, startPool, transform, isPersistent);
        }
        public void SetTarget(Transform toTarget)
        {
            target = toTarget;
        }
        protected override void HasPlayerApproach()
        {
            SetTarget(FindTarget<PlayerMaster>(GameManager.instance.layerPlayer));
        }
        protected override void DifficultUpdates()
        {
            m_EnemyDifficult = m_EnemiesMaster.enemy.enemiesSetDifficultyListSo.GetDifficultByEnemyDifficult(m_EnemiesMaster.getDifficultName);
            shootCadence = m_ShootCadence * m_EnemyDifficult.multiplyEnemiesShootCadence;
            playerApproachRadius = startApproachRadius * m_EnemyDifficult.multiplyPlayerDistanceRadius;
        }

        ShootState SetActualShootState(ShootState forceChange = ShootState.Hold)
        {
            if (forceChange != ShootState.Hold) return forceChange;
            if (!m_GamePlayManager.shouldBePlayingGame || !m_EnemiesMaster.shouldEnemyBeReady) return ShootState.Hold;
            if(shouldBeFire) forceChange = ShootState.Shooting;
            if (shouldBeFireByApproach) forceChange = ShootState.Patrolling;
            return forceChange;
        }

    }
}
