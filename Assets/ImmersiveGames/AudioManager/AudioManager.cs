using System.Collections;
using System.Linq;
using UnityEngine;

namespace ImmersiveGames
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private MapStateBgm[] mapStateBgm;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Adicione esta função para obter o AudioEvent com base no IState fornecido
        public AudioEvent GetAudioEventForState(IState state)
        {
            return (from stateBgm in mapStateBgm where stateBgm.stateName == state.stateName select stateBgm.backgroundMusic).FirstOrDefault();
            // Retorna null se não encontrar correspondência
        }

        public void PlayBGM(IState state)
        {
            // Obtém o AudioEvent correspondente ao estado
            var bgm = GetAudioEventForState(state);

            // Verifica se há algo tocando atualmente
            if (audioSource.isPlaying)
            {
                // Se houver uma nova música para tocar, inicia uma transição suave (fade-out)
                StartCoroutine(FadeOut(audioSource, 0.1f));
            }
            // Se houver uma nova música para tocar, inicia a reprodução com uma transição suave (fade-in)
            if (bgm != null)
            {
                bgm.Play(audioSource, this, 0.1f);
            }
        }

        private static IEnumerator FadeOut(AudioSource source, float fadeTime)
        {
            var startVolume = source.volume;

            // Fade-out gradual
            while (source.volume > 0)
            {
                source.volume -= startVolume * Time.deltaTime / fadeTime;
                yield return null;
            }

            // Pára a música
            source.Stop();

            // Restaura o volume para evitar problemas futuros
            source.volume = startVolume;
        }
    }

    [System.Serializable]
    public class MapStateBgm
    {
        public string stateName;
        public AudioEvent backgroundMusic;
    }
}


/*
using System;
using System.Collections;
using ImmersiveGames.ScriptableObjects;
using UnityEngine;
using UnityEngine.Audio;
using Utils;

namespace ImmersiveGames
{
    /// <summary>
    /// Manages audio playback, including background music, sound effects, and voiceovers.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioMixer mixerGroup;
        [SerializeField] private AudioSource bgmAudioSource;
        [SerializeField] private AudioSource sfxAudioSource;
        [SerializeField] internal Tools.SerializableDictionary<LevelTypes, AudioEvent> bgmLevels = new Tools.SerializableDictionary<LevelTypes, AudioEvent>();
        [Header("Menu SFX")]
        public AudioClip missionFailSound;
        public AudioClip missionSuccessSound;
        private GameSettings m_GameSettings;

        #region UNITY METHODS

        /// <summary>
        /// Initializes the AudioManager, setting default values and recovering audio settings.
        /// </summary>
        private void Start()
        {
            m_GameSettings = GameSettings.instance;
            bgmAudioSource.pitch = 1;
            RecoveryAudioSettings();
        }

        /// <summary>
        /// Cleanup method, currently commented out.
        /// </summary>
        protected override void OnDestroy()
        {
            // base.OnDestroy();
        }

        #endregion

        /// <summary>
        /// Recovers audio settings based on the game settings.
        /// </summary>
        private void RecoveryAudioSettings()
        {
            float volumeMusic = Tools.SoundBase10(m_GameSettings.musicVolume);
            float volumeSfx = Tools.SoundBase10(m_GameSettings.sfxVolume);
            mixerGroup.SetFloat("MusicVolume", volumeMusic);
            mixerGroup.SetFloat("SfxVolume", volumeSfx);
        }

        /// <summary>
        /// Plays background music for a specific level type.
        /// </summary>
        /// <param name="typeLevel">The level type for which to play the background music.</param>
        public void PlayBGM(LevelTypes typeLevel)
        {
            if (!bgmLevels.TryGetValue(typeLevel, out var audioSource))
            {
                Debug.LogError($"BGM not found for level type: {typeLevel}");
                return;
            }

            audioSource.Play(bgmAudioSource);
        }

        /// <summary>
        /// Coroutine to smoothly play background music with fade.
        /// </summary>
        private IEnumerator PlayBGM(AudioSource source, AudioEventScriptableObject track, float time)
        {
            if (source.isPlaying)
                yield return StartCoroutine(AudioUtils.FadeProperty(source, time, source.volume, 0, AudioUtils.SetVolume));

            track.Play(source);
        }

        /// <summary>
        /// Coroutine to smoothly stop background music with fade.
        /// </summary>
        private IEnumerator StopBGM(AudioSource source, AudioEventScriptableObject track, float time)
        {
            if (source.isPlaying)
                yield return StartCoroutine(AudioUtils.FadeProperty(source, time, source.volume, 0, AudioUtils.SetVolume));

            track.Stop(source);
        }

        /// <summary>
        /// Changes background music for a specific level type with fade.
        /// </summary>
        /// <param name="typeLevel">The level type for which to change the background music.</param>
        /// <param name="time">The duration of the fade effect.</param>
        public void ChangeBGM(LevelTypes typeLevel, float time)
        {
            if (!bgmLevels.TryGetValue(typeLevel, out var audioSource))
            {
                Debug.LogError($"BGM not found for level type: {typeLevel}");
                return;
            }

            if (bgmAudioSource.isPlaying && bgmAudioSource.clip == audioSource.audioSample.audioClip)
                return;

            bgmAudioSource.pitch = audioSource.getPitch;
            bgmAudioSource.volume = audioSource.getVolume;
            StartCoroutine(PlayBGM(bgmAudioSource, audioSource, time));
        }

        /// <summary>
        /// Plays a voice audio clip as a one-shot.
        /// </summary>
        /// <param name="audioClip">The voice audio clip to play.</param>
        public void PlayVoice(AudioClip audioClip)
        {
            PlayOneShot(sfxAudioSource, audioClip);
        }

        /// <summary>
        /// Helper method to play a one-shot audio clip.
        /// </summary>
        private static void PlayOneShot(AudioSource audioSource, AudioClip audioClip)
        {
            audioSource.PlayOneShot(audioClip);
        }

        /// <summary>
        /// Plays a sound effect using an AudioEvent.
        /// </summary>
        /// <param name="audioEventScriptableObjectSample">The AudioEvent representing the sound effect.</param>
        public void PlaySfx(AudioEventScriptableObject audioEventScriptableObjectSample)
        {
            audioEventScriptableObjectSample.PlayOnShot(sfxAudioSource);
        }

        /// <summary>
        /// Stops background music for a specific level type with fade.
        /// </summary>
        /// <param name="typeLevel">The level type for which to stop the background music.</param>
        public void StopBGM(LevelTypes typeLevel)
        {
            if (bgmAudioSource == null)
                return;

            if (!bgmLevels.TryGetValue(typeLevel, out var audioSource))
            {
                Debug.LogError($"BGM not found
                */
