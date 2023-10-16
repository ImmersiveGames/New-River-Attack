using TMPro;
using UnityEngine;
namespace RiverAttack
{
    public class UiTextDistance : MonoBehaviour
    {
        GamePlayManager m_GamePlayManager;
        PlayerSettings m_PlayerSettings;
        TMP_Text m_TMPTextDistance;
        #region UNITYMETHODS
        void OnEnable()
        {
            SetInitialReferences();
            m_GamePlayManager.EventUpdateDistance += UpdateDistance;
        }
        void Start()
        {
            int distance = Mathf.FloorToInt(m_PlayerSettings.distance);
            UpdateDistance(distance);
        }
        void OnDisable()
        {
            m_GamePlayManager.EventUpdateDistance -= UpdateDistance;
        }
  #endregion

        void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_PlayerSettings = PlayerManager.instance.GetPlayerSettingsByIndex();
            m_TMPTextDistance = GetComponent<TMP_Text>();
        }

        void UpdateDistance(int distance)
        {
            m_TMPTextDistance.text = distance.ToString();

        }
    }
}
