using UnityEngine;

namespace RiverAttack
{
    public class LevelFinish : MonoBehaviour
    {
        GamePlayManager m_GamePlayManager;
        #region UNITY METHODS
        void OnEnable()
        {
            m_GamePlayManager = GamePlayManager.instance;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerMaster>() == null) return;
            if (!m_GamePlayManager.levelsFinish.Contains(m_GamePlayManager.actualLevel))
                m_GamePlayManager.levelsFinish.Add(m_GamePlayManager.actualLevel);
            m_GamePlayManager.CallEventCompletePath();
        }
  #endregion
        
    }
}
