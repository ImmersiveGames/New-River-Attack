using TMPro;
using UnityEngine;

namespace RiverAttack
{
    public class UiTextScore : MonoBehaviour
    {
        private GamePlayManager m_GamePlayManager;
        private PlayerSettings m_PlayerSettings;
        private TMP_Text m_TMPTextScore;
        #region UNITYMETHODS

        private void OnEnable()
        {
            SetInitialReferences();
            m_GamePlayManager.EventUpdateScore += UpdateScore;
            int score = m_PlayerSettings.score;
            UpdateScore(score);
        }

        private void Start()
        {
            int score = m_PlayerSettings.score;
            UpdateScore(score);
        }

        private void OnDisable()
        {
            m_GamePlayManager.EventUpdateScore -= UpdateScore;
        }
  #endregion

  private void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_PlayerSettings = PlayerManager.instance.GetPlayerSettingsByIndex();
            m_TMPTextScore = GetComponent<TMP_Text>();
        }

        private void UpdateScore(int score)
        {
            m_TMPTextScore.text = score.ToString();

        }
    }
}
