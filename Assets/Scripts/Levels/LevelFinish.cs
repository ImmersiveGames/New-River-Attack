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
            if (other.GetComponentInParent<PlayerMaster>() == null) return;
            if (m_GamePlayManager.readyToFinish != true) return;
            //m_GamePlayManager.completePath = true;
            GameTimelineManager.instance.CompletePathEndCutScene();
        }
  #endregion
        
    }
}
