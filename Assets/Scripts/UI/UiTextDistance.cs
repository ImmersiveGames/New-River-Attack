using TMPro;
using UnityEngine;
namespace RiverAttack
{
    public class UiTextDistance : MonoBehaviour
    {
        private GamePlayManager m_GamePlayManager;
        private PlayerSettings m_PlayerSettings;
        private TMP_Text m_TMPTextDistance;
        #region UNITYMETHODS

        private void OnEnable()
        {
            SetInitialReferences();
            m_GamePlayManager.EventUpdateDistance += UpdateDistance;
        }

        private void Start()
        {
            int distance = Mathf.FloorToInt(m_PlayerSettings.distance);
            UpdateDistance(distance);
        }

        private void OnDisable()
        {
            m_GamePlayManager.EventUpdateDistance -= UpdateDistance;
        }
  #endregion

  private void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_PlayerSettings = PlayerManager.instance.GetPlayerSettingsByIndex();
            m_TMPTextDistance = GetComponent<TMP_Text>();
        }

        private void UpdateDistance(int distance)
        {
            m_TMPTextDistance.text = distance.ToString();
        }
    }
}
