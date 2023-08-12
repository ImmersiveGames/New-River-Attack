using TMPro;
using UnityEngine;

namespace RiverAttack 
{
    public class UiTextRefugees : MonoBehaviour
    {
        GamePlayManager m_GamePlayManager;
        PlayerSettings m_PlayerSettings;
        TMP_Text m_TMPTextRefugees;
        #region UNITYMETHODS
        void OnEnable()
        {
            SetInitialReferences();
            m_GamePlayManager.EventUpdateRefugees += UpdateRefugees;
        }
        void Start()
        {
            int refugees = m_PlayerSettings.wealth;
            UpdateRefugees(refugees);
        }
        void OnDisable()
        {
            m_GamePlayManager.EventUpdateRefugees -= UpdateRefugees;
        }
  #endregion

        void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_PlayerSettings = m_GamePlayManager.GetNoPlayerPlayerSettings();
            m_TMPTextRefugees = GetComponent<TMP_Text>();
        }

        void UpdateRefugees(int distance)
        {
            m_TMPTextRefugees.text = distance.ToString();
            
        }
    }
}

