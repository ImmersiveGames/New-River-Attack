using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace RiverAttack
{
    public class UiRapidFire: MonoBehaviour
    {
        [SerializeField] Image rapidFireDisable;
        [SerializeField] Image rapidFireEnable;
        [SerializeField] TMP_Text rapidFireCounter;

        [SerializeField] PowerUp listenPowerUp;
        float m_SpendTimer;
        GamePlayManager m_GamePlayManager;

        #region UNITYMETHODS
        void OnEnable()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_GamePlayManager.EventStartRapidFire += StartRapidFire;
            m_GamePlayManager.EventEndRapidFire += EndRapidFire;
            m_GamePlayManager.EventUpdatePowerUpDuration += CountRapidFire;
            m_GamePlayManager.EventEnemiesMasterKillPlayer += EndRapidFire;
            m_GamePlayManager.EventOtherEnemiesKillPlayer += EndRapidFire;
        }

        void Start()
        {
            rapidFireDisable.gameObject.SetActive(true);
            rapidFireEnable.gameObject.SetActive(false);
            rapidFireCounter.gameObject.SetActive(true);
            rapidFireCounter.text = "00";
        }
        void Update()
        {
            if (!(m_SpendTimer > 0))
                return;
            m_SpendTimer -= Time.deltaTime;
            if (m_SpendTimer <= 0)
            {
                m_SpendTimer = 0;
            }
            rapidFireCounter.text = m_SpendTimer.ToString("F2");
        }
        void OnDisable()
        {
            m_GamePlayManager.EventStartRapidFire -= StartRapidFire;
            m_GamePlayManager.EventEndRapidFire -= EndRapidFire;
            m_GamePlayManager.EventUpdatePowerUpDuration -= CountRapidFire;
            m_GamePlayManager.EventEnemiesMasterKillPlayer -= EndRapidFire;
            m_GamePlayManager.EventOtherEnemiesKillPlayer -= EndRapidFire;
        }
  #endregion

        void StartRapidFire()
        {
            if(rapidFireEnable.gameObject.activeSelf == false)
                rapidFireEnable.gameObject.SetActive(true);
        }
        void EndRapidFire()
        {
            if (!rapidFireEnable.gameObject.activeSelf)
                return;
            rapidFireEnable.gameObject.SetActive(false);
            rapidFireCounter.text = "00";
        }
        void CountRapidFire(PowerUp powerUp, float timer)
        {
            if (powerUp != listenPowerUp)
                return;
            m_SpendTimer = (timer > 0) ? timer : 0.0f;
        }
    }
}
