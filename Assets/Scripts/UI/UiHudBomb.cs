using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace RiverAttack
{
    public class UiHudBomb : MonoBehaviour
    {
        private GamePlayManager m_GamePlayManager;
        private PlayerSettings m_PlayerSettings;
        public TMP_Text tmpTextBomb;
        public Image bombOn;
        public Image bombOff;

        #region UNITYMETHODS

        private void OnEnable()
        {
            SetInitialReferences();
            UpdateBombs(m_PlayerSettings.bombs);
            m_GamePlayManager.EventUpdateBombs += UpdateBombs;
        }

        private void Start()
        {
            UpdateBombs(m_PlayerSettings.bombs);
        }

        private void OnDisable()
        {
            m_GamePlayManager.EventUpdateBombs -= UpdateBombs;
        }
  #endregion

  private void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_PlayerSettings = PlayerManager.instance.GetPlayerSettingsByIndex();
        }

        private void UpdateBombs(int bombs)
        {
            bombOff.enabled = bombs <= 0;
            bombOn.enabled = bombs > 0;
         
            tmpTextBomb.text = $"X {bombs}";
        }
    }
}
