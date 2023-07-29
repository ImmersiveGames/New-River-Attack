using UnityEngine;
using UnityEngine.InputSystem;
using Utils;
namespace RiverAttack
{
    [RequireComponent(typeof(PlayerMaster))]
    public class PlayerShoot : MonoBehaviour, ICommand, IHasPool
    {
        [Tooltip("Identifica se o jogador em modo rapidfire")]
        float m_ShootCadence;
        
        [SerializeField]
        GameObject bullet;
        [SerializeField]
        int startPool;
        [SerializeField]
        bool autoShoot;

        [Header("Bullet Settings")]
        [SerializeField] float bulletSpeed;
        [SerializeField] float bulletLifeTime;

        //private ControllerMap controllerMap;
        float m_NextShoot;
        GameObject m_MyShoot;
        GamePlayManager m_GamePlayManager;
        PlayerMaster m_PlayerMaster;
        PlayerSettings m_PlayerSettings;
        PlayersInputActions m_PlayersInputActions;
       #region UNITY METHODS
        void Awake()
        {
            m_PlayersInputActions = new PlayersInputActions();
        }
        void OnEnable()
        {
            SetInitialReferences();
            StartMyPool();
        }
        void Start()
        {
            m_PlayersInputActions = m_PlayerMaster.playersInputActions;
            m_PlayersInputActions.Player.Shoot.performed += Execute;
        }
        
        void FixedUpdate()
        {
            if(autoShoot) Execute();
        }
  #endregion
        
        void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_PlayerSettings = m_PlayerMaster.GetPlayersSettings();
        }
        
        public void Execute()
        {
            if (!m_GamePlayManager.shouldBePlayingGame || !m_PlayerMaster.ShouldPlayerBeReady())
                return;
            m_ShootCadence -= Time.deltaTime;
            if (!(m_ShootCadence <= 0))
                return;
            m_PlayerMaster.CallEventPlayerShoot();
            m_ShootCadence = m_PlayerSettings.cadenceShoot;
            Fire();
        }
        public void Execute(InputAction.CallbackContext callbackContext)
        {
            if (!m_GamePlayManager.shouldBePlayingGame || !m_PlayerMaster.ShouldPlayerBeReady() || autoShoot)
                return;
            m_PlayerMaster.CallEventPlayerShoot();
            Fire();
        }
        void Fire()
        {
            m_MyShoot = PoolObjectManager.GetObject(this);
            m_MyShoot.transform.parent = null;
            var transform1 = transform;
            var transformPosition = transform1.localPosition;
            var transformRotation = transform1.rotation;
            m_MyShoot.transform.position = new Vector3(transformPosition.x, transformPosition.y, transformPosition.z);
            m_MyShoot.transform.rotation = new Quaternion(transformRotation.x, transformRotation.y, transformRotation.z, transformRotation.w);
        }
        public void UnExecute()
        {
            throw new System.NotImplementedException();
        }
        public void UnExecute(InputAction.CallbackContext callbackContext)
        {
            throw new System.NotImplementedException();
        }
        public void StartMyPool(bool isPersistent = false)
        {
            PoolObjectManager.CreatePool(this, bullet, startPool, transform, isPersistent);
            var pool = PoolObjectManager.GetPool(this);
            for (int i = 0; i < pool.childCount; i++)
            {
                var bulletPlayer = pool.GetChild(i).GetComponent<BulletPlayer>();
                bulletPlayer.ownerShoot = m_PlayerMaster;
                bulletPlayer.SetMyPool(pool);
                bulletPlayer.Init(bulletSpeed, bulletLifeTime);
            }
        }
    }
}
