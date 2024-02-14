using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace RiverAttack
{
    public class UiRapidFire: MonoBehaviour
    {
        [SerializeField] private Image rapidFireDisable;
        [SerializeField] private Image rapidFireEnable;
        [SerializeField] private TMP_Text rapidFireCounter;

        [SerializeField] private PowerUp listenPowerUp;
        private float m_SpendTimer;
        private GamePlayManager m_GamePlayManager;

        #region UNITYMETHODS

        private void OnEnable()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_GamePlayManager.EventStartRapidFire += StartRapidFire;
            m_GamePlayManager.EventEndRapidFire += EndRapidFire;
            m_GamePlayManager.EventUpdatePowerUpDuration += CountRapidFire;
            m_GamePlayManager.EventEnemiesMasterKillPlayer += EndRapidFire;
            m_GamePlayManager.EventOtherEnemiesKillPlayer += EndRapidFire;
            m_GamePlayManager.EventReSpawnEnemiesMaster += ClearRapidFire;
        }

        private void Start()
        {
            rapidFireDisable.gameObject.SetActive(true);
            rapidFireEnable.gameObject.SetActive(false);
            rapidFireCounter.gameObject.SetActive(true);
            rapidFireCounter.text = "00";
        }

        private void Update()
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

        private void OnDisable()
        {
            m_GamePlayManager.EventStartRapidFire -= StartRapidFire;
            m_GamePlayManager.EventEndRapidFire -= EndRapidFire;
            m_GamePlayManager.EventUpdatePowerUpDuration -= CountRapidFire;
            m_GamePlayManager.EventEnemiesMasterKillPlayer -= EndRapidFire;
            m_GamePlayManager.EventOtherEnemiesKillPlayer -= EndRapidFire;
            m_GamePlayManager.EventReSpawnEnemiesMaster -= ClearRapidFire;
        }
  #endregion

  private void StartRapidFire()
        {
            if(rapidFireEnable.gameObject.activeSelf == false)
                rapidFireEnable.gameObject.SetActive(true);
        }

        private void EndRapidFire()
        {
            if (rapidFireEnable.gameObject.activeSelf == false || m_SpendTimer > 0)
                return;
            rapidFireEnable.gameObject.SetActive(false);
            rapidFireCounter.text = "00";
        }

        private void ClearRapidFire()
        {
            m_SpendTimer = 0;
            rapidFireEnable.gameObject.SetActive(false);
            rapidFireCounter.text = "00";
        }
        private void CountRapidFire(PowerUp powerUp, float timer)
        {
            if (powerUp != listenPowerUp)
                return;
            m_SpendTimer = (timer > 0) ? timer : 0.0f;
        }
    }
}
