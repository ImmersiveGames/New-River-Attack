using System;
using UnityEngine;

namespace RiverAttack
{
    public class LevelChangeBGM : LevelCheck
    {
        const float TIME_TO_FADE_BGM = 0.1f;
        GameAudioManager m_GameAudioManager;
        GamePlayManager m_GamePlayManager;
        #region UNITY METHODS
        void OnEnable()
        {
            m_GameAudioManager = GameAudioManager.instance;
            m_GamePlayManager = GamePlayManager.instance;
        }

        void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponentInParent<PlayerMaster>()) return;
            //Debug.Log($"Stat Music: {GamePlayManager.instance.actualLevels.bgmStartLevel}");
            var actualLevelSetup = GameMissionBuilder.instance.GetActualLevelSetup();
            m_GameAudioManager.ChangeBGM(actualLevelSetup.bgmLevel, TIME_TO_FADE_BGM);
            Debug.Log($"Actual Level: {m_GamePlayManager.actualLevels}");
            m_GamePlayManager.OnEventBuildPathUpdate(transform.position.z);
        }

        void OnDestroy()
        {
            m_GameAudioManager = null;
            m_GamePlayManager = null;
        }
  #endregion
        
    }
}
