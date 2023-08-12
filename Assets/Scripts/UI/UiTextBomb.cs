using UnityEngine;
using TMPro;

namespace RiverAttack 
{
    public class UiTextBomb : MonoBehaviour
    {
        GamePlayManager m_GamePlayManager;
        PlayerSettings m_PlayerSettings;
        TMP_Text m_TMPTextBomb;
        #region UNITYMETHODS
        void OnEnable()
        {
            SetInitialReferences();
            m_GamePlayManager.EventUpdateBombs += UpdateBombs;
        }
        void Start()
        {
            int bombs = m_PlayerSettings.bombs;
            UpdateBombs(bombs);
        }
        void OnDisable()
        {
            m_GamePlayManager.EventUpdateBombs -= UpdateBombs;
        }
  #endregion

        void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_PlayerSettings = m_GamePlayManager.GetNoPlayerPlayerSettings();
            m_TMPTextBomb = GetComponent<TMP_Text>();
        }

        void UpdateBombs(int bombs)
        {
            m_TMPTextBomb.text = $"X {bombs}";
        }
    }
}

