using UnityEngine;
namespace RiverAttack
{
    public class EnemiesSound : MonoBehaviour
    {

        protected ObstacleMasterOld ObstacleMasterOld;
        protected AudioSource audioSource;
        [SerializeField] private AudioEventSample enemyExplodeAudio;

        private GamePlayManager m_GamePlayManager;

        #region UNITY METHODS
        protected virtual void OnEnable()
        {
            SetInitialReferences();
            ObstacleMasterOld.EventObstacleMasterHit += ExplodeSound;
            m_GamePlayManager.EventDeactivateEnemiesMaster += StopSound;
        }
        protected virtual void OnDisable()
        {
            ObstacleMasterOld.EventObstacleMasterHit -= ExplodeSound;
            m_GamePlayManager.EventDeactivateEnemiesMaster -= StopSound;
        }
  #endregion

        protected virtual void SetInitialReferences()
        {
            ObstacleMasterOld = GetComponent<ObstacleMasterOld>();
            audioSource = GetComponentInChildren<AudioSource>();
            m_GamePlayManager = GamePlayManager.instance;
        }

        private void StopSound()
        {
            if (audioSource == null && !audioSource.isPlaying) return;
            audioSource.Stop();
        }
        internal void ExplodeSound()
        {
            if (audioSource != null && enemyExplodeAudio != null)
                enemyExplodeAudio.Play(audioSource);
        }
    }
}
