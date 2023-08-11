using UnityEngine;

namespace RiverAttack
{
    [RequireComponent(typeof(PlayerMaster))]
    public class PlayerHealth : MonoBehaviour
    {/*
    #region Variable Private Inspector
        [Header("Fuel")]
        [SerializeField]
        bool autoDecreaseHp;
        [SerializeField]
        int reduceFuelRate;
        [SerializeField]
        public int alertHp;
        [SerializeField]
        AudioEventSample playerAlert;
        [SerializeField]
        [Range(0, 5)]
        float reduceFuelCadence;
    #endregion
    #region Variable Private System
        float m_NextDescesseFuel;
        AudioSource m_AudioSource;
    #endregion
    #region Variable Private References
        PlayerMaster m_PlayerMaster;
        PlayerSettings m_PlayerSettings;
    #endregion

        #region UNITY METHODS
        void OnEnable()
        {
            SetInitialReferences();
            PlayerSetup();
            m_PlayerMaster.EventIncreaseHealth += IncreaseFuel;
            m_PlayerMaster.EventPlayerMasterReSpawn += PlayerSetup;
        }
        void Update()
        {
            if (autoDecreaseHp && !GamePlayManager.instance.getGodMode)
            {
                AutoReduceHp(reduceFuelRate, reduceFuelCadence);
            }
        }
        void OnDisable()
        {
            m_PlayerMaster.EventIncreaseHealth -= IncreaseFuel;
            m_PlayerMaster.EventPlayerMasterReSpawn -= PlayerSetup;
        }
  #endregion

        void SetInitialReferences()
        {
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_PlayerSettings = m_PlayerMaster.GetPlayersSettings();
            m_AudioSource = GetComponent<AudioSource>();
        }
        void AutoReduceHp(int reduce, float time)
        {
            if (!m_PlayerMaster.ShouldPlayerBeReady() || m_PlayerSettings.actualHp <= 0 || !(Time.time > m_NextDescesseFuel))
                return;
            m_NextDescesseFuel = Time.time + time;
            ReduceFuel(reduce);
            CheckHealth();
            LogFuelSpent(reduce);
        }

        void CheckHealth()
        {
            if (playerAlert && m_PlayerMaster.ShouldPlayerBeReady() && m_PlayerSettings.actualHp <= alertHp && !playerAlert.IsPlaying(m_AudioSource))
            {
                playerAlert.Play(m_AudioSource);
            }
            else if (playerAlert && m_PlayerMaster.ShouldPlayerBeReady() && m_PlayerSettings.actualHp > alertHp && playerAlert.IsPlaying(m_AudioSource))
            {
                playerAlert.Stop(m_AudioSource);
            }
        }

        static void LogFuelSpent(int reduce)
        {
            GamePlaySettings.instance.fuelSpent += reduce;
        }

        void ReduceFuel(int fuel)
        {
            if(!m_PlayerMaster.inEffectArea)
                m_PlayerSettings.actualHp -= fuel;
            if (m_PlayerSettings.actualHp > 0) return;
            m_PlayerSettings.actualHp = 0;
            m_PlayerMaster.CallEventPlayerMasterOnDestroy();
        }
        void IncreaseFuel(int fuel)
        {
            m_PlayerSettings.actualHp = fuel;
            if (m_PlayerSettings.actualHp > m_PlayerSettings.maxHp)
            {
                m_PlayerSettings.actualHp = m_PlayerSettings.maxHp;
            }
        }
        void PlayerSetup()
        {
            m_PlayerSettings.actualHp = m_PlayerSettings.maxHp;
        }*/
    }
}
