using UnityEngine;
namespace RiverAttack
{
    public class EnemiesSound : MonoBehaviour
    {

        protected ObstacleMaster m_ObstacleMaster;
        protected AudioSource audioSource;
        [SerializeField]
        AudioEventSample enemyExplodeAudio;

        GamePlayManager m_GamePlayManager;

        #region UNITY METHODS
        protected virtual void OnEnable()
        {
            SetInitialReferences();
            m_ObstacleMaster.EventObstacleMasterHit += ExplodeSound;
            m_GamePlayManager.EventDeactivateEnemiesMaster += StopSound;
        }
        protected virtual void OnDisable()
        {
            m_ObstacleMaster.EventObstacleMasterHit -= ExplodeSound;
            m_GamePlayManager.EventDeactivateEnemiesMaster -= StopSound;
        }
  #endregion

        protected virtual void SetInitialReferences()
        {
            m_ObstacleMaster = GetComponent<ObstacleMaster>();
            audioSource = GetComponentInChildren<AudioSource>();
            m_GamePlayManager = GamePlayManager.instance;
        }
        void StopSound()
        {
            if (audioSource == null && !audioSource.isPlaying) return;
            audioSource.Stop();
        }
        void ExplodeSound()
        {
            if (audioSource != null && enemyExplodeAudio != null)
                enemyExplodeAudio.Play(audioSource);
        }
    }
}
