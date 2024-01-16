using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace RiverAttack
{
    public class UiHudBomb : MonoBehaviour
    {
        GamePlayManager m_GamePlayManager;
        PlayerSettings m_PlayerSettings;
        public TMP_Text tmpTextBomb;
        public Image bombOn;
        public Image bombOff;

        #region UNITYMETHODS
        void OnEnable()
        {
            SetInitialReferences();
            m_GamePlayManager.EventUpdateBombs += UpdateBombs;
        }
        void Start()
        {
            UpdateBombs(m_PlayerSettings.bombs);
        }
        void OnDisable()
        {
            m_GamePlayManager.EventUpdateBombs -= UpdateBombs;
        }
  #endregion

        void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_PlayerSettings = PlayerManager.instance.GetPlayerSettingsByIndex();
        }

        void UpdateBombs(int bombs)
        {
            bombOff.enabled = bombs <= 0;
            bombOn.enabled = bombs > 0;
         
            tmpTextBomb.text = $"X {bombs}";
        }
    }
}
