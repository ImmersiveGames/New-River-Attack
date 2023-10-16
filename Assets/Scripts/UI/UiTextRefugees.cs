using TMPro;
using UnityEngine;

namespace RiverAttack
{
    public class UiTextRefugees : MonoBehaviour
    {
        GamePlayManager m_GamePlayManager;
        PlayerSettings m_PlayerSettings;
        TMP_Text m_TMPTextRefugees;
        Animator m_Animator;
        static readonly int RefugieBounce = Animator.StringToHash("Bounce");
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
            m_PlayerSettings = PlayerManager.instance.GetPlayerSettingsByIndex();
            m_TMPTextRefugees = GetComponent<TMP_Text>();
            m_Animator = GetComponent<Animator>();
        }

        void UpdateRefugees(int refugie)
        {
            m_Animator.SetTrigger(RefugieBounce);
            m_TMPTextRefugees.text = refugie.ToString();
        }
    }
}
