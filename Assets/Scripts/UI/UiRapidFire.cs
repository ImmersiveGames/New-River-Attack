using System;
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
        float spendTimer;
        GamePlayManager m_GamePlayManager;

        #region UNITYMETHODS
        void OnEnable()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_GamePlayManager.EventStartRapidFire += StartRapidFire;
            m_GamePlayManager.EventEndRapidFire += EndRapidFire;
            m_GamePlayManager.EventUpdatePowerUpDuration += CountRapidFire;
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
            if (!(spendTimer > 0))
                return;
            spendTimer -= Time.deltaTime;
            if (spendTimer <= 0)
            {
                spendTimer = 0;
            }
            rapidFireCounter.text = spendTimer.ToString("F2");
        }
        void OnDisable()
        {
            m_GamePlayManager.EventStartRapidFire -= StartRapidFire;
            m_GamePlayManager.EventEndRapidFire -= EndRapidFire;
            m_GamePlayManager.EventUpdatePowerUpDuration -= CountRapidFire;
        }
  #endregion

        void StartRapidFire()
        {
            rapidFireEnable.gameObject.SetActive(true);
        }
        void EndRapidFire()
        {
            rapidFireEnable.gameObject.SetActive(false);
            rapidFireCounter.text = "00";
        }
        void CountRapidFire(PowerUp powerUp, float timer)
        {
            if (powerUp != listenPowerUp)
                return;
            spendTimer = (timer > 0) ? timer : 0.0f;
        }
    }
}
