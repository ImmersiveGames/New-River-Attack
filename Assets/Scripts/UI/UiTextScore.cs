using TMPro;
using UnityEngine;

namespace RiverAttack
{
    public class UiTextScore : MonoBehaviour
    {
        GamePlayManager m_GamePlayManager;
        PlayerSettings m_PlayerSettings;
        TMP_Text m_TMPTextScore;
        #region UNITYMETHODS
        void OnEnable()
        {
            SetInitialReferences();
            m_GamePlayManager.EventUpdateScore += UpdateScore;
            int score = m_PlayerSettings.score;
            UpdateScore(score);
        }
        void Start()
        {
            int score = m_PlayerSettings.score;
            UpdateScore(score);
        }
        void OnDisable()
        {
            m_GamePlayManager.EventUpdateScore -= UpdateScore;
        }
  #endregion

        void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_PlayerSettings = PlayerManager.instance.GetPlayerSettingsByIndex();
            m_TMPTextScore = GetComponent<TMP_Text>();
        }

        void UpdateScore(int score)
        {
            m_TMPTextScore.text = score.ToString();

        }
    }
}
