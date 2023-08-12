using UnityEngine;
using UnityEngine.UI;

namespace RiverAttack 
{
    public class GasDisplay : MonoBehaviour
    {
        [SerializeField] Image gasBarImage;
        [SerializeField] Color highGasColor;        
        [SerializeField] Color mediumGasColor;        
        [SerializeField] Color lowGasColor;

        const float MEDIUM_GAS_VALUE = 0.6f;
        const float LOW_GAS_VALUE = 0.2f;
        
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
        }

        void UpdateDisplay() 
        {
            float gasAmount = ((float)m_PlayerSettings.actualHp) / 100;
            gasBarImage.fillAmount = gasAmount ;

            gasBarImage.color = gasAmount switch
            {
                < MEDIUM_GAS_VALUE and > LOW_GAS_VALUE => mediumGasColor,
                <= LOW_GAS_VALUE => lowGasColor,
                _ => highGasColor
            };
        }
    }
}

