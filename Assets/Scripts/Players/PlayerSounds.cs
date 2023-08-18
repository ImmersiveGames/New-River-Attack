using System.Collections;
using UnityEngine;

namespace RiverAttack
{
    public class PlayerSounds : MonoBehaviour
    {
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

        bool m_OnAccelerate;
        bool m_OnDecelerate;
        
        PlayerMaster m_PlayerMaster;
        AudioSource m_AudioSource;
        //GamePlayManager m_GamePlayManager;

        #region UNITY METHODS
        void OnEnable()
        {
            SetInitialReferences();
            m_PlayerMaster.EventPlayerMasterControllerMovement += SoundEngine;
            m_PlayerMaster.EventPlayerMasterHit += SoundExplosion;
           /* m_GamePlayManager.EventPausePlayGame += SoundStop;
            m_GamePlayManager.EventCompletePath += SoundStop;*/
        }
        void OnDisable()
        {
            m_PlayerMaster.EventPlayerMasterControllerMovement -= SoundEngine;
            m_PlayerMaster.EventPlayerMasterHit -= SoundExplosion;
            /*
             * m_GamePlayManager.EventPausePlayGame -= SoundStop;
             */
        }
  #endregion
        void SetInitialReferences()
        {
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_AudioSource = GetComponent<AudioSource>();
            //m_GamePlayManager = GamePlayManager.instance;
        }

        void SoundEngine(Vector2 dir)
        {
            if (!m_PlayerMaster.shouldPlayerBeReady) return;
            switch (dir.y)
            {
                case > 0 when m_OnAccelerate == false:
                    m_OnAccelerate = true;
                    StartCoroutine(ChangeEngine(audioStartAccelEngine, audioEngineAccelerator));
                    break;
                case < 0 when m_OnDecelerate == false:
                    m_OnDecelerate = true;
                    audioDeceleratorEngine.Play(m_AudioSource);
                    AudioEventSample.UpdateChangePith(m_AudioSource, audioEngineLoop.audioSample.pitch.y, enginePitchDown);
                    break;
                case 0 when (m_OnAccelerate || m_OnDecelerate):
                    m_OnAccelerate = false;
                    m_OnDecelerate = false;
                    StartCoroutine(FadeOutAudio(m_AudioSource, audioEngineLoop.getVolume, 0.01f));
                    break;
            }

            if(!m_AudioSource.isPlaying)
                audioEngineLoop.Play(m_AudioSource);
        }
        
        IEnumerator ChangeEngine(AudioEventSample audioStart, AudioEvent audioFix)
        {
            audioStart.Play(m_AudioSource);
            while (audioStart.IsPlaying(m_AudioSource))
            {
                yield return null;
            }
            audioFix.Play(m_AudioSource);
        }

        static IEnumerator FadeOutAudio(AudioSource source,float initialVolume, float fadeOutDuration)
        {
            float elapsedTime = 0.0f;
            while (elapsedTime < fadeOutDuration)
            {
                source.volume = Mathf.Lerp(initialVolume, 0, elapsedTime / fadeOutDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            source.Stop();
            source.volume = initialVolume; // Restaurar o volume original
        }

        void SoundExplosion()
        {
            if (audioPlayerExplosion == null) return;
            StopAllCoroutines();
            audioPlayerExplosion.Play(m_AudioSource);
        }
        /*
         void SoundStop()
        {
            StopAllCoroutines();
            m_AudioSource.Stop();
        }
        */
    }
}
