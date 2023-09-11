using UnityEngine;

namespace RiverAttack
{
    public class LevelChangeBGM : MonoBehaviour
    {
        const float TIME_TO_FADE_BGM = 0.1f;
        GamePlayAudio m_PlayAudio;
        GamePlayManager m_PlayMaster;
        #region UNITY METHODS
        void OnEnable()
        {
            m_PlayAudio = GamePlayAudio.instance;
            m_PlayMaster = GamePlayManager.instance;
        }

        void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponentInParent<PlayerMaster>()) return;
            Debug.Log($"Stat Music: {GamePlayManager.instance.actualLevels.bgmStartLevel}");
            m_PlayMaster.OnEventBuildPathUpdate(transform.position.z);
        }
  #endregion
        
    }
}
