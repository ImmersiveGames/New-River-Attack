using UnityEngine;

namespace RiverAttack
{
    public class LevelChangeBGM : MonoBehaviour
    {
        const float TIME_TO_FADE_BGM = 0.1f;
        GameAudioManager m_AudioManager;
        GamePlayManager m_PlayMaster;
        #region UNITY METHODS
        void OnEnable()
        {
            m_AudioManager = GameAudioManager.instance;
            m_PlayMaster = GamePlayManager.instance;
        }

        void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponentInParent<PlayerMaster>()) return;
            //Debug.Log($"Stat Music: {GamePlayManager.instance.actualLevels.bgmStartLevel}");
            m_PlayMaster.OnEventBuildPathUpdate(transform.position.z);
        }
  #endregion
        
    }
}
