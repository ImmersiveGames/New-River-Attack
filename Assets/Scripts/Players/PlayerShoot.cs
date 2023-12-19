using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;
using MeshRenderer = UnityEngine.MeshRenderer;
namespace RiverAttack
{
    [RequireComponent(typeof(PlayerMaster))]
    public class PlayerShoot : MonoBehaviour, ICommand, IHasPool
    {
        [Header("Shoot Settings")]
        [SerializeField]
        GameObject prefabBullet;
        [SerializeField]
        int startBulletPool;

        [Header("Bullets Settings")]
        public Color bulletColor;
        public Color rapidFireColor;
        
        float m_ShootCadence;
        bool m_CanExecuteAction;
        
        PlayersInputActions m_PlayersInputActions;
        GamePlayManager m_GamePlayManager;
        PlayerMaster m_PlayerMaster;
        PlayerSettings m_PlayerSettings;
        static GamePlayingLog _gamePlayingLog;

        #region UNITYMETHODS
        void OnEnable()
        {
            SetInitialReferences();
            m_ShootCadence = m_PlayerSettings.cadenceShoot;
        }
        void Start()
        {
            StartMyPool(prefabBullet, startBulletPool);
            m_PlayersInputActions = GamePlayManager.instance.inputSystem;
            m_CanExecuteAction = true;
            m_PlayersInputActions.Player.Shoot.performed += Execute;
        }
        void OnDestroy()
        {
            m_PlayersInputActions.Player.Shoot.performed -= Execute;
        }
  #endregion
        
        void SetInitialReferences()
        {
            _gamePlayingLog = GamePlayingLog.instance;
            m_GamePlayManager = GamePlayManager.instance;
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_PlayerSettings = m_PlayerMaster.getPlayerSettings;
        }
        public void StartMyPool(GameObject bullets, int quantity, bool isPersistent = false)
        {
            PoolObjectManager.CreatePool(this, bullets, startBulletPool, transform);
            var pool = PoolObjectManager.GetPool(this);
            pool.SetAsLastSibling();
        }
        public void Execute(InputAction.CallbackContext callbackContext)
        {
            if (!m_CanExecuteAction || !m_GamePlayManager.shouldBePlayingGame || !m_PlayerMaster.shouldPlayerBeReady)
                return;
            m_GamePlayManager.OnEventPlayerPushButtonShoot();
            m_ShootCadence = m_PlayerSettings.cadenceShoot;
            if (m_PlayerSettings.cadenceShootPowerUp != 0)
                m_ShootCadence = m_PlayerSettings.cadenceShootPowerUp;
            Fire();
            StartCoroutine(Cooldown());

        }
        void Fire()
        {
            var myShoot = PoolObjectManager.GetObject(this);
            var bulletPlayer = myShoot.GetComponent<BulletPlayer>();
            bulletPlayer.SetMyPool(PoolObjectManager.GetPool(this));
            bulletPlayer.ownerShoot = m_PlayerMaster;
            bulletPlayer.Init(m_PlayerSettings.shootVelocity, m_PlayerSettings.shootLifeTime);
            var meshRenderer = bulletPlayer.GetComponent<MeshRenderer>();
            //TODO: Animação no tiro e um feedback auditivo para o tiro;
            if (meshRenderer != null)
                meshRenderer.material.color = (m_PlayerMaster.inPowerUp) ? rapidFireColor : bulletColor;
            
            myShoot.transform.parent = null;
            LogGamePlay();

        }
        IEnumerator Cooldown()
        {
            m_CanExecuteAction = false;

            yield return new WaitForSeconds(m_ShootCadence);
            m_CanExecuteAction = true;
        }
        public void UnExecute(InputAction.CallbackContext callbackContext)
        {
            
        }
        static void LogGamePlay()
        {
            _gamePlayingLog.shootSpent += 1;
        }
        
    }
}
