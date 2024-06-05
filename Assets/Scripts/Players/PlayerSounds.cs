using System.Collections;
using UnityEngine;

namespace RiverAttack
{
    public class PlayerSounds : MonoBehaviour
    {
        [SerializeField] private AudioEventSample audioEngineLoop;
        [SerializeField] private AudioEventSample audioStartAccelEngine;
        [SerializeField] private AudioEventSample audioEngineAccelerator;
        [SerializeField] private AudioEventSample audioDeceleratorEngine;
        [SerializeField, Range(.1f, 3)] private float enginePitchDown = .5f;
        [SerializeField] private AudioEventSample audioPlayerExplosion;

        private bool m_OnAccelerate;
        private bool m_OnDecelerate;

        private PlayerMasterOld _mPlayerMasterOld;

        private AudioSource m_AudioSource;
        //GamePlayManager m_GamePlayManager;

        #region UNITY METHODS

        private void OnEnable()
        {
            SetInitialReferences();
            _mPlayerMasterOld.EventPlayerMasterControllerMovement += SoundEngine;
            _mPlayerMasterOld.EventPlayerMasterHit += SoundExplosion;
            /* m_GamePlayManager.EventPausePlayGame += SoundStop;
             m_GamePlayManager.EventCompletePath += SoundStop;*/
        }

        private void OnDisable()
        {
            _mPlayerMasterOld.EventPlayerMasterControllerMovement -= SoundEngine;
            _mPlayerMasterOld.EventPlayerMasterHit -= SoundExplosion;
            /*
             * m_GamePlayManager.EventPausePlayGame -= SoundStop;
             */
        }
  #endregion

  private void SetInitialReferences()
        {
            _mPlayerMasterOld = GetComponent<PlayerMasterOld>();
            m_AudioSource = GetComponent<AudioSource>();
            //m_GamePlayManager = GamePlayManager.instance;
        }

        private void SoundEngine(Vector2 dir)
        {
            if (!_mPlayerMasterOld.ShouldPlayerBeReady) return;
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

            if (!m_AudioSource.isPlaying)
                audioEngineLoop.Play(m_AudioSource);
        }

        private IEnumerator ChangeEngine(AudioEventSample audioStart, AudioEvent audioFix)
        {
            audioStart.Play(m_AudioSource);
            while (audioStart.IsPlaying(m_AudioSource))
            {
                yield return null;
            }
            audioFix.Play(m_AudioSource);
        }

        private static IEnumerator FadeOutAudio(AudioSource source, float initialVolume, float fadeOutDuration)
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

        private void SoundExplosion()
        {
            if (audioPlayerExplosion == null) return;
            StopAllCoroutines();
            audioPlayerExplosion.Play(m_AudioSource);
        }
    }
}
