using UnityEngine;

namespace RiverAttack
{
    [RequireComponent(typeof(PlayerMaster))]
    public class PlayerHealth : MonoBehaviour
    {
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
        PlayerStats m_PlayerStats;
    #endregion

        #region UNITYMETHODS
        void OnEnable()
        {
            SetInitialReferences();
            PlayerSetup();
            m_PlayerMaster.EventIncreaseHealth += IncreaseFuel;
            m_PlayerMaster.EventPlayerReload += PlayerSetup;
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
            m_PlayerMaster.EventPlayerReload -= PlayerSetup;
        }
  #endregion

        void SetInitialReferences()
        {
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_PlayerStats = m_PlayerMaster.GetPlayersSettings();
            m_AudioSource = GetComponent<AudioSource>();
        }
        private void AutoReduceHp(int reduce, float time)
        {
            if (!m_PlayerMaster.shouldPlayerBeReady || m_PlayerStats.actualHp <= 0 || !(Time.time > m_NextDescesseFuel))
                return;
            m_NextDescesseFuel = Time.time + time;
            ReduceFuel(reduce);
            CheckHealth();
            LogFuelSpent(reduce);
        }

        private void CheckHealth()
        {
            if (playerAlert && m_PlayerMaster.shouldPlayerBeReady && m_PlayerStats.actualHp <= alertHp && !playerAlert.IsPlaying(m_AudioSource))
            {
                playerAlert.Play(m_AudioSource);
            }
            else if (playerAlert && m_PlayerMaster.shouldPlayerBeReady && m_PlayerStats.actualHp > alertHp && playerAlert.IsPlaying(m_AudioSource))
            {
                playerAlert.Stop(m_AudioSource);
            }
        }

        static void LogFuelSpent(int reduce)
        {
            GamePlaySettings.instance.fuelSpents += reduce;
        }

        void ReduceFuel(int fuel)
        {
            m_PlayerStats.actualHp -= fuel;
            if (m_PlayerStats.actualHp > 0) return;
            m_PlayerStats.actualHp = 0;
            m_PlayerMaster.CallEventPlayerDestroy();
        }
        void IncreaseFuel(int fuel)
        {
            m_PlayerStats.actualHp = fuel;
            if (m_PlayerStats.actualHp > m_PlayerStats.maxHp)
            {
                m_PlayerStats.actualHp = m_PlayerStats.maxHp;
            }
        }
        void PlayerSetup()
        {
            m_PlayerStats.actualHp = m_PlayerStats.maxHp;
        }


    }
}
