using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using Utils;
namespace RiverAttack
{
    public class GamePlayAudio : Singleton<GamePlayAudio>
    {
        [SerializeField] public AudioMixer mixerGroup;
        [SerializeField] AudioSource bgmAudioSource;
        [SerializeField] protected AudioSource voiceAudioSource;
        [SerializeField] internal Tools.SerializableDictionary<LevelTypes, AudioEventSample> bgmLevels = new Tools.SerializableDictionary<LevelTypes, AudioEventSample>();
        [Header("Menu SFX")]
        [SerializeField] AudioClip clickSound;
        [SerializeField] public AudioClip missionFailSound;
        
        GameSettings m_GameSettings;

        #region UNITYMETHODS
        void Awake()
        {
            bgmAudioSource = GetComponentInParent<AudioSource>();
            bgmAudioSource.pitch = 1;
        }
        void Start()
        {
            m_GameSettings = GameSettings.instance;
            SetOptionSound();
        }
  #endregion

        void SetOptionSound()
        {
            float volumeMusic = Mathf.Log10(m_GameSettings.musicVolume) * 20f;
            float volumeSfx = Mathf.Log10(m_GameSettings.sfxVolume) * 20f;
            mixerGroup.SetFloat("MusicVolume", volumeMusic);
            mixerGroup.SetFloat("SFXVolume", volumeSfx);
        }
        public void PlayBGM(LevelTypes typeLevel)
        {
            bgmLevels.TryGetValue(typeLevel, out var audioSource);
            audioSource.Play(bgmAudioSource);
        }
        IEnumerator PlayBGM(AudioSource source, AudioEvent track, float time)
        {
            if (source.isPlaying)
                yield return StartCoroutine(FadeAudio(source, time, source.volume, 0));
            track.Play(source);
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
            PlayOneShot(voiceAudioSource,audioClip);
        }
        static void PlayOneShot(AudioSource audioSource, AudioClip audioClip)
        {
            audioSource.PlayOneShot(audioClip);
        }
        public void PlayClickSfx(AudioSource audioSource)
        {
            PlayOneShot(audioSource, clickSound);
        }
        public void StopBGM()
        {
            if (bgmAudioSource != null)
                bgmAudioSource.Stop();
        }

        public void AccelPinch(float accel)
        {
            if(Math.Abs(accel - bgmAudioSource.pitch) > 0.01f)
                StartCoroutine(FadePitch(bgmAudioSource, 0.1f, bgmAudioSource.pitch, accel));
        }

        static IEnumerator FadePitch(AudioSource source, float timer, float starts, float ends)
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
        static IEnumerator FadeAudio(AudioSource source, float timer, float starts, float ends)
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
