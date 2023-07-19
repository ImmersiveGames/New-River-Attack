using System.Collections;
using System.Collections.Generic;
using RiverAttack;
using UnityEngine;
namespace RiverAttack
{

    [RequireComponent(typeof(EnemiesMaster))]
    [RequireComponent(typeof(AudioSource))]
    public class EnemiesSound : MonoBehaviour
    {

        EnemiesMaster m_EnemyMaster;
        protected AudioSource audioSource;
        [SerializeField]
        AudioEventSample enemyExplodeAudio;

        GamePlayManager m_GamePlayManager;

        #region UNITY METHODS
        protected virtual void OnEnable()
        {
            SetInitialReferences();
            m_EnemyMaster.EventDestroyEnemy += ExplodeSound;
            m_GamePlayManager.EventResetEnemies += StopSound;
        }
        protected virtual void OnDisable()
        {
            m_EnemyMaster.EventDestroyEnemy -= ExplodeSound;
            m_GamePlayManager.EventResetEnemies -= StopSound;
        }
  #endregion

        protected virtual void SetInitialReferences()
        {
            m_EnemyMaster = GetComponent<EnemiesMaster>();
            audioSource = GetComponent<AudioSource>();
            m_GamePlayManager = GamePlayManager.instance;
        }
        void StopSound()
        {
            if (audioSource != null && audioSource.isPlaying)
                audioSource.Stop();
        }
        void ExplodeSound()
        {
            if (audioSource != null && enemyExplodeAudio != null)
                enemyExplodeAudio.Play(audioSource);
        }
    }
}
