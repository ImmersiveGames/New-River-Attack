using UnityEngine;
using Utils;
// ReSharper disable StringLiteralTypo

namespace RiverAttack
{
    [RequireComponent(typeof(PlayerMaster))]
    public class PlayerShoot : MonoBehaviour, ICommand, IHasPool
    {
        // ReSharper disable once StringLiteralTypo
        [Tooltip("Identifica se o jogador em modo rapidfire")]
        float m_ShootCadence;
        /*[SerializeField]
        float idButtonMap;*/
        [Tooltip("Objeto disparado pelo player")]
        [SerializeField]
        GameObject bullet;
        [SerializeField]
        int startPool;

        //private ControllerMap controllerMap;
        float m_NextShoot;
        GameObject m_MyShoot;
        PlayerMaster m_PlayerMaster;
        PlayerStats m_PlayerStats;
        PlayerController m_PlayerController;
        /// <summary>
        /// Executa quando ativa o objeto
        /// </summary>
        /// 
        private void OnEnable()
        {
            SetInitialReferences();
            StartMyPool();
        }
        /// <summary>
        /// Configura as referencias iniciais
        /// </summary>
        /// 
        private void SetInitialReferences()
        {
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_PlayerController = GetComponent<PlayerController>();
            m_PlayerStats = m_PlayerMaster.PlayersSettings();
            //controllerMap = m_PlayerMaster.playerSettings.controllerMap;
        }
        void FixedUpdate()
        {
            this.Execute();
        }
        public void Execute()
        {
            if (!m_PlayerMaster.shouldPlayerBeReady) return;
            m_ShootCadence -= Time.deltaTime;
            //Debug.Log("SHOOT Cadenciado:" + m_shootCadence);
            if (!(m_ShootCadence <= 0))
                return;
            m_PlayerMaster.CallEventPlayerShoot();

            m_ShootCadence = m_PlayerStats.cadenceShoot;
            Fire();
        }
        private void Fire()
        {
            m_MyShoot = PoolObjectManager.instance.GetObject(this);
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
            PoolObjectManager.instance.CreatePool(this, bullet, startPool, transform, isPersistent);
        }
    }
}
