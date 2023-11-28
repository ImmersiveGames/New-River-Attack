using System;
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
            if (!GamePlayingLog.instance.finishLevels.Contains(m_GamePlayManager.actualLevels))
            {
                m_GamePlayManager.actualLevels.levelsStates = LevelsStates.Complete;
                GamePlayingLog.instance.finishLevels.Add(m_GamePlayManager.actualLevels);
            }
                
            GameTimelineManager.instance.CompletePathEndCutScene();
        }
  #endregion
        
    }
}
