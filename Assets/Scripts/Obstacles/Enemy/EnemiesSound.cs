using UnityEngine;
namespace RiverAttack
{
    public class EnemiesSound : MonoBehaviour
    {

        protected ObstacleMaster obstacleMaster;
        protected AudioSource audioSource;
        [SerializeField]
        AudioEventSample enemyExplodeAudio;

        GamePlayManager m_GamePlayManager;

        #region UNITY METHODS
        protected virtual void OnEnable()
        {
            SetInitialReferences();
            obstacleMaster.EventObstacleMasterHit += ExplodeSound;
            m_GamePlayManager.EventDeactivateEnemiesMaster += StopSound;
        }
        protected virtual void OnDisable()
        {
            obstacleMaster.EventObstacleMasterHit -= ExplodeSound;
            m_GamePlayManager.EventDeactivateEnemiesMaster -= StopSound;
        }
  #endregion

        protected virtual void SetInitialReferences()
        {
            obstacleMaster = GetComponent<ObstacleMaster>();
            audioSource = GetComponentInChildren<AudioSource>();
            m_GamePlayManager = GamePlayManager.instance;
        }
        void StopSound()
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
