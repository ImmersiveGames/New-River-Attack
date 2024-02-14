using System;
using UnityEngine;

namespace RiverAttack
{
    public class LevelChangeBGM : LevelCheck
    {
        private const float TIME_TO_FADE_BGM = 0.1f;
        private GameAudioManager m_GameAudioManager;
        private GamePlayManager m_GamePlayManager;
        #region UNITY METHODS

        private void OnEnable()
        {
            m_GameAudioManager = GameAudioManager.instance;
            m_GamePlayManager = GamePlayManager.instance;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponentInParent<PlayerMaster>()) return;
            var actualLevelSetup = GameMissionBuilder.instance.GetActualLevelSetup();
            m_GameAudioManager.ChangeBGM(actualLevelSetup.bgmLevel, TIME_TO_FADE_BGM);
            m_GamePlayManager.OnEventBuildPathUpdate(transform.position.z);
        }

        private void OnDestroy()
        {
            m_GameAudioManager = null;
            m_GamePlayManager = null;
        }
  #endregion
        
    }
}
