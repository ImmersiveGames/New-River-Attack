using System.Collections;
using ImmersiveGames;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;
using MeshRenderer = UnityEngine.MeshRenderer;
namespace RiverAttack
{
    [RequireComponent(typeof(PlayerMasterOld))]
    public class PlayerShoot : MonoBehaviour, ICommand, IHasPool
    {
        [Header("Shoot Settings")]
        [SerializeField]
        private GameObject prefabBullet;
        [SerializeField] private int startBulletPool;

        [Header("Bullets Settings")]
        public Color bulletColor;
        public Color rapidFireColor;

        private float m_ShootCadence;
        private float lastActionTime;

        private PlayersInputActions m_PlayersInputActions;
        private GamePlayManager m_GamePlayManager;
        private PlayerMasterOld _mPlayerMasterOld;
        private PlayerSettings m_PlayerSettings;
        private static GamePlayingLog _gamePlayingLog;

        #region UNITYMETHODS

        private void OnEnable()
        {
            SetInitialReferences();
            //m_ShootCadence = m_PlayerSettings.cadenceShoot;
        }

        private void Start()
        {
            StartMyPool(prefabBullet, startBulletPool);
            m_PlayersInputActions = GameManager.instance.inputSystem;
            m_PlayersInputActions.Player.Shoot.performed += Execute;
        }

        private void OnDestroy()
        {
            m_PlayersInputActions.Player.Shoot.performed -= Execute;
        }
  #endregion

  private void SetInitialReferences()
        {
            _gamePlayingLog = GamePlayingLog.instance;
            m_GamePlayManager = GamePlayManager.instance;
            _mPlayerMasterOld = GetComponent<PlayerMasterOld>();
            m_PlayerSettings = _mPlayerMasterOld.getPlayerSettings;
        }
        public void StartMyPool(GameObject bullets, int quantity, bool isPersistent = false)
        {
            PoolObjectManager.CreatePool(this, bullets, startBulletPool, transform);
            var pool = PoolObjectManager.GetPool(this);
            pool.SetAsLastSibling();
        }
        public void Execute(InputAction.CallbackContext callbackContext)
        {
            //Debug.Log($"ShotCadence: {m_ShootCadence}, Should Play {m_GamePlayManager.shouldBePlayingGame}, Should Player Ready {m_PlayerMaster.shouldPlayerBeReady}");
            var cooldown = (m_PlayerSettings.cadenceShootPowerUp != 0)? m_PlayerSettings.cadenceShootPowerUp:m_PlayerSettings.cadenceShoot;
            if (!m_GamePlayManager.shouldBePlayingGame || !_mPlayerMasterOld.ShouldPlayerBeReady)
                return;
            if (!IsOnCooldown(cooldown))
            {
                // Faça a ação aqui
                m_GamePlayManager.OnEventPlayerPushButtonShoot();
                Fire();
                //Debug.Log("Ação realizada!");

                // Atualize o tempo da última ação
                lastActionTime = Time.realtimeSinceStartup;
            }
            else
            {
                // Ação está em cooldown, você pode optar por lidar de alguma forma
                //Debug.Log("Ação em cooldown!");
            }
        }

        private void Fire()
        {
            var myShoot = PoolObjectManager.GetObject(this);
            var bulletPlayer = myShoot.GetComponent<BulletPlayer>();
            bulletPlayer.SetMyPool(PoolObjectManager.GetPool(this));
            bulletPlayer.ownerShoot = _mPlayerMasterOld;
            bulletPlayer.Init(m_PlayerSettings.shootVelocity, m_PlayerSettings.shootLifeTime);
            var meshRenderer = bulletPlayer.GetComponent<MeshRenderer>();
            //TODO: Animação no tiro e um feedback auditivo para o tiro;
            if (meshRenderer != null)
                meshRenderer.material.color = (_mPlayerMasterOld.inPowerUp) ? rapidFireColor : bulletColor;
            
            myShoot.transform.parent = null;
            LogGamePlay();

        }
        private bool IsOnCooldown(float cooldown)
        {
            return Time.realtimeSinceStartup - lastActionTime < cooldown;
        }
        public void UnExecute(InputAction.CallbackContext callbackContext)
        {
            
        }

        private static void LogGamePlay()
        {
            _gamePlayingLog.shootSpent += 1;
        }
        
    }
}
