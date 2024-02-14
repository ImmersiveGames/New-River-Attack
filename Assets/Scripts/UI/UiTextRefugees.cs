using TMPro;
using UnityEngine;

namespace RiverAttack
{
    public class UiTextRefugees : MonoBehaviour
    {
        private GamePlayManager m_GamePlayManager;
        private PlayerSettings m_PlayerSettings;
        private TMP_Text m_TMPTextRefugees;
        private Animator m_Animator;
        private static readonly int RefugeesBounce = Animator.StringToHash("Bounce");
        #region UNITYMETHODS

        private void OnEnable()
        {
            SetInitialReferences();
            m_GamePlayManager.EventUpdateRefugees += UpdateRefugees;
        }

        private void Start()
        {
            int refugees = m_PlayerSettings.wealth;
            UpdateRefugees(refugees);
        }

        private void OnDisable()
        {
            m_GamePlayManager.EventUpdateRefugees -= UpdateRefugees;
        }
  #endregion

  private void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_PlayerSettings = PlayerManager.instance.GetPlayerSettingsByIndex();
            m_TMPTextRefugees = GetComponent<TMP_Text>();
            m_Animator = GetComponent<Animator>();
        }

        private void UpdateRefugees(int refugee)
        {
            m_Animator.SetTrigger(RefugeesBounce);
            m_TMPTextRefugees.text = refugee.ToString();
        }
    }
}
