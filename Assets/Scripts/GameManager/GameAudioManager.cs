using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using Utils;
namespace RiverAttack
{
    [RequireComponent(typeof(AudioSource))]
    public class GameAudioManager : Singleton<GameAudioManager>
    {
        public AudioMixer mixerGroup;
        [SerializeField] private AudioSource bgmAudioSource;
        [SerializeField] protected AudioSource sfxAudioSource;
        [SerializeField] internal Tools.SerializableDictionary<LevelTypes, AudioEventSample> bgmLevels = new Tools.SerializableDictionary<LevelTypes, AudioEventSample>();
        [Header("Menu SFX")]
        public AudioClip missionFailSound;
        public AudioClip missionSuccessSound;
        private GameSettings m_GameSettings;

        #region UNITYMETHODS

        private void Start()
        {
            m_GameSettings = GameSettings.instance;
            bgmAudioSource.pitch = 1;
            RecoveryAudioSettings();
        }
        protected override void OnDestroy()
        {
            //base.OnDestroy();
        }
  #endregion

  private void RecoveryAudioSettings()
        {
            float volumeMusic = Tools.SoundBase10(m_GameSettings.musicVolume);
            float volumeSfx = Tools.SoundBase10(m_GameSettings.sfxVolume);
            mixerGroup.SetFloat("MusicVolume", volumeMusic);
            mixerGroup.SetFloat("SfxVolume", volumeSfx);
        }
        public void PlayBGM(LevelTypes typeLevel)
        {
            bgmLevels.TryGetValue(typeLevel, out var audioSource);
            audioSource.Play(bgmAudioSource);
        }

        private IEnumerator PlayBGM(AudioSource source, AudioEvent track, float time)
        {
            if (source.isPlaying)
                yield return StartCoroutine(FadeAudio(source, time, source.volume, 0));
            track.Play(source);
        }

        private IEnumerator StopBGM(AudioSource source, AudioEvent track, float time)
        {
            if (source.isPlaying)
                yield return StartCoroutine(FadeAudio(source, time, source.volume, 0));
            track.Stop(source);
        }
        public void ChangeBGM(LevelTypes typeLevel, float time)
        {
            bgmLevels.TryGetValue(typeLevel, out var audioSource);
            if (bgmAudioSource.isPlaying && bgmAudioSource.clip == audioSource.audioSample.audioClip)
                return;
            bgmAudioSource.pitch = audioSource.getPitch;
            bgmAudioSource.volume = audioSource.getVolume;
            StartCoroutine(PlayBGM(bgmAudioSource, audioSource, time));
        }

        public void PlayVoice(AudioClip audioClip)
        {
            PlayOneShot(sfxAudioSource,audioClip);
        }

        private static void PlayOneShot(AudioSource audioSource, AudioClip audioClip)
        {
            audioSource.PlayOneShot(audioClip);
        }
        public void PlaySfx(AudioEvent audioEventSample)
        {
            audioEventSample.PlayOnShot(sfxAudioSource);
        }
        public void StopBGM(LevelTypes typeLevel)
        {
            if (bgmAudioSource == null)
                return;
            bgmLevels.TryGetValue(typeLevel, out var audioSource);
            StartCoroutine(StopBGM(bgmAudioSource, audioSource, 1f));
        }

        public void AccelPinch(float accel)
        {
            if(Math.Abs(accel - bgmAudioSource.pitch) > 0.01f)
                StartCoroutine(FadePitch(bgmAudioSource, 0.1f, bgmAudioSource.pitch, accel));
        }

        private static IEnumerator FadePitch(AudioSource source, float timer, float starts, float ends)
        {
            float i = 0.0F;
            float step = 1.0F / timer;
            while (i <= 1.0F)
            {
                i += step * Time.deltaTime;
                source.pitch = Mathf.Lerp(starts, ends, i);
                yield return new WaitForSeconds(step * Time.deltaTime);
            }
            if (ends <= 0)
                source.Stop();
        }

        private static IEnumerator FadeAudio(AudioSource source, float timer, float starts, float ends)
        {
            float i = 0.0F;
            float step = 1.0F / timer;
            while (i <= 1.0F)
            {
                i += step * Time.deltaTime;
                source.volume = Mathf.Lerp(starts, ends, i);
                yield return new WaitForSeconds(step * Time.deltaTime);
            }
            if (ends <= 0)
                source.Stop();
        }
    }
}
