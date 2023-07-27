using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class EnemiesShoot : ObstacleDetectApproach, IShoot, IHasPool
    {
        PoolObjectManager m_MyPool;
        [SerializeField]
        protected internal int poolStart;
        [SerializeField]
        protected GameObject prefabShoot;
        [SerializeField]
        protected internal bool hasTarget;
        [SerializeField]
        protected bool canShoot;

        public enum ShootStatus {Hold, Shoot, Patrol}
        [SerializeField]
        ShootStatus enemyShootStatus;

        [Header("Config Bullets")]
        [SerializeField]
        protected internal float bulletSpeedy;
        [SerializeField]
        protected internal float cadenceShoot;
        Transform m_SpawnPosition;
        
        Transform target
        {
            get;
            set;
        }
        Renderer m_MyRenderer;
        GameManager m_GameManager;
        GamePlayManager m_GamePlayManager;
        EnemiesMaster m_EnemiesMaster;
        EnemiesSkinParts m_SkinPart;
        
        #region UNITYMETHODS
        void OnEnable()
        {
            SetInitialReferences();
            StartMyPool();
            /*if (!shootByApproach)
            {
                InvokeRepeating(nameof(Fire), 1, cadenceShoot);
            }*/
        }
        void Start()
        {
            SetTarget(GameManager.instance.GetActivePlayerTransform(0));
            m_SpawnPosition = transform;
        }
        void LateUpdate()
        {
            if (!GamePlayManager.instance.shouldBePlayingGame || !m_EnemiesMaster.ShouldEnemyBeReady() || enemyShootStatus != ShootStatus.Shoot || !m_MyRenderer.isVisible) return;
            /*if (m_SkinPart == null || !hasTarget) return;
            var transform1 = m_SkinPart.transform;
            m_SkinPart.transform.rotation = Quaternion.Lerp(transform1.rotation, Quaternion.LookRotation(target.position - transform1.position), Time.deltaTime);*/
            Fire();
        }
        #endregion
        void SetInitialReferences()
        {
            m_GameManager = GameManager.instance;
            m_GamePlayManager = GamePlayManager.instance;
            m_EnemiesMaster = GetComponent<EnemiesMaster>();
            m_MyRenderer = GetComponentInChildren<Renderer>();
            m_SkinPart = GetComponentInChildren<EnemiesSkinParts>();
        }
        public void Fire()
        {
            Debug.Log("SHOOT");
                /*if (canShoot)
                {
                    var bullet = PoolObjectManager.GetObject(this);
                    bullet.transform.position = m_SpawnPosition.position;
                    bullet.transform.rotation = m_SpawnPosition.rotation;
                    var enemyBullet = bullet.GetComponent<BulletEnemy>();
                    enemyBullet.shootDirection = Vector3.forward;
                    enemyBullet.shootVelocity = bulletSpeedy;
                    if (hasTarget)
                        enemyBullet.transform.LookAt(target);
                }*/
        }
        public void SetTarget(Transform toTarget)
        {
            target = toTarget;
        }
        public void StartMyPool(bool isPersistent = false)
        {
            PoolObjectManager.CreatePool(this, prefabShoot, poolStart, transform, isPersistent);
        }
    }
}
