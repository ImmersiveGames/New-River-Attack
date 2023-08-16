using UnityEngine;
using UnityEngine.UI;

namespace RiverAttack 
{
    public class GasDisplay : MonoBehaviour
    {
        [SerializeField] AudioEventSample playerAlert;
        [SerializeField] Image gasBarImage;
        [SerializeField] Color highGasColor;        
        [SerializeField] Color mediumGasColor;        
        [SerializeField] Color lowGasColor;

        const float MEDIUM_GAS_VALUE = 0.6f;
        const float LOW_GAS_VALUE = 0.2f;
        
        AudioSource m_AudioSource;
        GamePlayManager m_GamePlayManager;
        PlayerSettings m_PlayerSettings;

        #region UNITYMETHODS
        void OnEnable()
        {
            SetInitialReferences();
        }
        void Update()
        {
            UpdateDisplay();
        }
        #endregion
        void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_PlayerSettings = m_GamePlayManager.GetNoPlayerPlayerSettings();
            m_AudioSource = GetComponent<AudioSource>();
        }
        void UpdateDisplay() 
        {
            float gasAmount = ((float)m_PlayerSettings.actualFuel) / 100;
            gasBarImage.fillAmount = gasAmount ;

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
            if(m_AudioSource.isPlaying && gasAmount > LOW_GAS_VALUE)
            {
                playerAlert.Stop(m_AudioSource);
            }
        }
    }
}

