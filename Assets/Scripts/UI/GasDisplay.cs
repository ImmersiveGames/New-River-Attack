using UnityEngine;
using UnityEngine.UI;

namespace RiverAttack
{
    public class GasDisplay : MonoBehaviour
    {
        [SerializeField] private AudioEventSample playerAlert;
        [SerializeField] private Image gasBarImage;
        [SerializeField] private Color highGasColor;
        [SerializeField] private Color mediumGasColor;
        [SerializeField] private Color lowGasColor;

        private const float MEDIUM_GAS_VALUE = 0.6f;
        private const float LOW_GAS_VALUE = 0.2f;

        private AudioSource m_AudioSource;
        private GamePlayManager m_GamePlayManager;
        private PlayerSettings m_PlayerSettings;

        #region UNITYMETHODS

        private void OnEnable()
        {
            SetInitialReferences();
        }

        private void Update()
        {
            UpdateDisplay();
        }
        #endregion

        private void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_PlayerSettings = PlayerManager.instance.GetPlayerSettingsByIndex();
            m_AudioSource = GetComponent<AudioSource>();
        }

        private void UpdateDisplay()
        {
            float gasAmount = ((float)m_PlayerSettings.actualFuel) / 100;
            gasBarImage.fillAmount = gasAmount;

            gasBarImage.color = gasAmount switch
            {
                < MEDIUM_GAS_VALUE and > LOW_GAS_VALUE => mediumGasColor,
                <= LOW_GAS_VALUE => lowGasColor,
                _ => highGasColor
            };

            if (gasAmount <= LOW_GAS_VALUE && !m_AudioSource.isPlaying)
            {
                playerAlert.Play(m_AudioSource);
            }
            if (m_AudioSource.isPlaying && gasAmount > LOW_GAS_VALUE)
            {
                playerAlert.Stop(m_AudioSource);
            }
        }
    }
}
