using UnityEngine;

namespace RiverAttack
{
    public class LevelFinish : MonoBehaviour
    {
        GameSettings m_GameSettings;
        GameManager m_GameManager;
        GamePlayManager m_GamePlay;
        void OnEnable()
        {
            m_GamePlay = GamePlayManager.instance;
            m_GameManager = GameManager.instance;
            m_GameSettings = GameSettings.instance;
        }

        void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponent<PlayerMaster>()) return;
            if (!m_GameManager.levelsFinish.Contains(m_GameManager.actualLevel))
                m_GameManager.levelsFinish.Add(m_GameManager.actualLevel);
            m_GamePlay.CallEventCompletePath();
        }
    }
}
