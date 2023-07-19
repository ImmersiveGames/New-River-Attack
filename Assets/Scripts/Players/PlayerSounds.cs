using System.Collections;
using UnityEngine;

namespace RiverAttack
{
    [RequireComponent(typeof(PlayerMaster))]
    [RequireComponent(typeof(AudioSource))]
    public class PlayerSounds : MonoBehaviour
    {
    #region Variable Private Inspector
        [SerializeField]
        AudioEventSample audioEngineLoop;
        [SerializeField]
        AudioEventSample audioStartAccelEngine;
        [SerializeField]
        AudioEventSample audioEngineAccelerator;
        [SerializeField]
        AudioEventSample audioDeceleratorEngine;
        [SerializeField, Range(.1f, 3)]
        float enginePitchDown = .5f;
        [SerializeField]
        AudioEventSample audioPlayerExplosion;
    #endregion

    #region Variable Private References
        PlayerMaster m_PlayerMaster;
        AudioSource m_AudioSource;
        GamePlayManager m_GamePlayManager;
    #endregion

        #region UNITY METHODS
        void OnEnable()
        {
            SetInitialReferences();
            m_PlayerMaster.EventControllerMovement += SoundEngine;
            m_PlayerMaster.EventPlayerDestroy += SoundExplosion;
            m_GamePlayManager.EventPausePlayGame += SoundStop;
            m_GamePlayManager.EventCompletePath += SoundStop;
        }
        void OnDisable()
        {
            m_PlayerMaster.EventControllerMovement -= SoundEngine;
            m_PlayerMaster.EventPlayerDestroy -= SoundExplosion;
            m_GamePlayManager.EventPausePlayGame -= SoundStop;
        }
  #endregion
        void SetInitialReferences()
        {
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_AudioSource = GetComponent<AudioSource>();
            m_GamePlayManager = GamePlayManager.instance;
        }

        void SoundEngine(Vector2 dir)
        {
            if (audioEngineLoop == null || m_PlayerMaster.shouldPlayerBeReady == false) return;
            if (dir.y > 0 && m_PlayerMaster.playerStatus != PlayerMaster.Status.Accelerate)
            {
                m_PlayerMaster.playerStatus = PlayerMaster.Status.Accelerate;
                StartCoroutine(ChangeEngine(audioStartAccelEngine, audioEngineAccelerator));
            }
            switch (dir.y)
            {
                case < 0 when m_PlayerMaster.playerStatus != PlayerMaster.Status.Reduce:
                    m_PlayerMaster.playerStatus = PlayerMaster.Status.Reduce;
                    AudioEventSample.UpdateChangePith(m_AudioSource, audioEngineLoop.audioSample.pitch.y, enginePitchDown);
                    break;
                case 0 when m_PlayerMaster.playerStatus != PlayerMaster.Status.None:
                {
                    if (m_PlayerMaster.playerStatus == PlayerMaster.Status.Accelerate)
                        StartCoroutine(ChangeEngine(audioDeceleratorEngine, audioEngineLoop));
                    if (m_PlayerMaster.playerStatus == PlayerMaster.Status.Reduce)
                        AudioEventSample.UpdateChangePith(m_AudioSource, m_AudioSource.pitch, audioEngineLoop.audioSample.pitch.y);
                    m_PlayerMaster.playerStatus = PlayerMaster.Status.None;
                    break;
                }
                default:
                {
                    if (m_PlayerMaster.shouldPlayerBeReady && !m_AudioSource.isPlaying && m_PlayerMaster.playerStatus == PlayerMaster.Status.None)
                    {
                        //StopAllCoroutines();
                        audioEngineLoop.Play(m_AudioSource);
                    }
                    break;
                }
            }
        }

        IEnumerator ChangeEngine(AudioEvent audioStart, AudioEvent audioFix)
        {
            audioStart.Play(m_AudioSource);
            while (m_AudioSource.isPlaying)
            {
                yield return null;
            }
            audioFix.Play(m_AudioSource);
        }

        void SoundExplosion()
        {
            if (audioPlayerExplosion == null) return;
            StopAllCoroutines();
            audioPlayerExplosion.Play(m_AudioSource);
        }
        void SoundStop()
        {
            StopAllCoroutines();
            m_AudioSource.Stop();
        }
    }
}
