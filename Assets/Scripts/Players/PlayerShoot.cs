using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace RiverAttack
{
    [RequireComponent(typeof(PlayerMaster))]
public class PlayerShoot : MonoBehaviour, ICommand, IHasPool
{
    [Tooltip("Identifica se o jogador em modo rapidfire")]
    
    [SerializeField]
    float shootCadence;
    [SerializeField]
    float idButtonMap;
    [Tooltip("Objeto disparado pelo player")]
    [SerializeField]
    GameObject bullet;
    [SerializeField]
    int startPool;

    //private ControllerMap controllerMap;
    float m_NextShoot;
    GameObject m_MyShoot;
    PlayerMaster m_PlayerMaster;
    GameInputs m_GameInputs;

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
        m_GameInputs = GameInputs.instance;
        //controllerMap = m_PlayerMaster.playerSettings.controllerMap;
    }
    
    // pensar num callback para botões
    private void Update()
    {
        // Auto shoot timers and colddown
        
        /*if (controllerMap.ButtonUp(idButtonMap.Value))
        {
            this.Execute();
        }*/
    }

    public void Execute()
    {
        /*if (shootCadence < 0 || !m_PlayerMaster.shouldPlayerBeReady) return;
        m_PlayerMaster.CallEventPlayerShoot();
        if (shootCadence.Value > 0 && m_NextShoot < Time.time)
        {
            m_NextShoot = Time.time + shootCadence.Value;
            Fire();
        }
        else if (!m_MyShoot || m_MyShoot.activeSelf == false)
            Fire();*/
    }

    private void Fire()
    {
        /*m_MyShoot = PoolObjectManager.Instance.GetObject(this);
        double sp = Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("player_speed_shoot").DoubleValue;
        m_MyShoot.GetComponent<PlayerBullet>().SetSpeedShoot(sp);
        m_MyShoot.transform.parent = null;
        m_MyShoot.GetComponent<PlayerBullet>().OwnerShoot = m_PlayerMaster;
        m_MyShoot.transform.position = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        m_MyShoot.transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);*/
    }

    public void UnExecute()
    {
        throw new System.NotImplementedException();
    }

    public void StartMyPool(bool isPersistent = false)
    {
        //PoolObjectManager.Instance.CreatePool(this, bullet, startPool, transform, isPersistent);
    }
}

}
