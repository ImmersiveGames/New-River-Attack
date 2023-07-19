using UnityEngine;
using Utils;

namespace RiverAttack
{
    [RequireComponent(typeof(PlayerMaster))]
    public class PlayerShoot : MonoBehaviour, ICommand, IHasPool
    {
        // ReSharper disable once StringLiteralTypo
        [Tooltip("Identifica se o jogador em modo rapidfire")]
        float m_ShootCadence;
        [Tooltip("Objeto disparado pelo player")]
        [SerializeField]
        GameObject bullet;
        [SerializeField]
        int startPool;

        //private ControllerMap controllerMap;
        float m_NextShoot;
        GameObject m_MyShoot;
        PlayerMaster m_PlayerMaster;
        PlayerSettings m_PlayerSettings;
        PlayerController m_PlayerController;
       #region UNITY METHODS
        void OnEnable()
        {
            SetInitialReferences();
            StartMyPool();
        }
        void FixedUpdate()
        {
            this.Execute();
        }
  #endregion
        
        void SetInitialReferences()
        {
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_PlayerController = GetComponent<PlayerController>();
            m_PlayerSettings = m_PlayerMaster.GetPlayersSettings();
        }
        
        public void Execute()
        {
            if (!m_PlayerMaster.shouldPlayerBeReady) return;
            m_ShootCadence -= Time.deltaTime;
            //Debug.Log("SHOOT Cadenciado:" + m_shootCadence);
            if (!(m_ShootCadence <= 0))
                return;
            m_PlayerMaster.CallEventPlayerShoot();

            m_ShootCadence = m_PlayerSettings.cadenceShoot;
            Fire();
        }
        void Fire()
        {
            m_MyShoot = PoolObjectManager.GetObject(this);
            //double sp = Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("player_speed_shoot").DoubleValue;
            //m_MyShoot.GetComponent<BulletPlayer>().SetSpeedShoot(sp);
            m_MyShoot.transform.parent = null;
            m_MyShoot.GetComponent<BulletPlayer>().ownerShoot = m_PlayerMaster;
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
        public void StartMyPool(bool isPersistent = false)
        {
            PoolObjectManager.CreatePool(this, bullet, startPool, transform, isPersistent);
        }
    }
}
