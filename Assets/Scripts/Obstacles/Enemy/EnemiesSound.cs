using System.Collections;
using System.Collections.Generic;
using RiverAttack;
using UnityEngine;
namespace RiverAttack
{
    
    [RequireComponent(typeof(EnemiesMaster))]
    [RequireComponent(typeof(AudioSource))]
    public sealed class EnemiesSound : MonoBehaviour {

       EnemiesMaster m_EnemyMaster;
       AudioSource m_AudioSource;
        [SerializeField]
        AudioEventSample enemyExplodeAudio;

        GamePlayManager m_GamePlayManager;

        void OnEnable()
        {
            SetInitialReferences();
            m_EnemyMaster.EventDestroyEnemy += ExplodeSound;
            m_GamePlayManager.EventResetEnemies += StopSound;
        }
        void SetInitialReferences()
        {
            m_EnemyMaster = GetComponent<EnemiesMaster>();
            m_AudioSource = GetComponent<AudioSource>();
            m_GamePlayManager = GamePlayManager.instance;
        }
        void OnDisable()
        {
            m_EnemyMaster.EventDestroyEnemy -= ExplodeSound;
            m_GamePlayManager.EventResetEnemies -= StopSound;
        }
        void StopSound()
        {
            if (m_AudioSource != null && m_AudioSource.isPlaying)
                m_AudioSource.Stop();
        }
        void ExplodeSound()
        {
            if (m_AudioSource != null && enemyExplodeAudio != null)
                enemyExplodeAudio.Play(m_AudioSource);
        }
    }
}
