using System;
using System.Collections;
using ImmersiveGames.DebugManagers;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace ImmersiveGames.AudioEvents
{
    [CreateAssetMenu(fileName = "AudioEventClip", menuName = "Audio Events/Audio Clip", order = 1)]
    public class AudioEvent : AudioEventSo
    {
        [SerializeField] private AudioEventClip audioSample;
        [SerializeField] private AudioMixerGroup audioMixerGroup;

        private MonoBehaviour _cachedMonoBehaviour;

        #region Unity Methods

        private void OnDisable()
        {
            Cleanup();
        }

        #endregion

        

        public void PreviewPlay(AudioSource source)
        {
            if (audioSample?.audioClip == null) return;
            SetupSource(source);
            source.Play();
        }
        public void PreviewStop(AudioSource source)
        {
            source.Stop();
        }
        public override void Play(AudioSource source, MonoBehaviour monoBehaviour, float fadeTime = 1.0f)
        {
            if (audioSample?.audioClip == null || monoBehaviour == null)
            {
                DebugManager.LogError<AudioEvent>("Audio clip not assigned or MonoBehaviour not set in AudioEvent.");
                return;
            }
            if(source == null) return;

            _cachedMonoBehaviour = monoBehaviour;

            if (source.isPlaying)
            {
                DebugManager.LogWarning<AudioEvent>($"Ja esta tocando: {audioSample.name}");
                _cachedMonoBehaviour.StartCoroutine(TransitionAudio(source, fadeTime));
            }
            else
            {
                DebugManager.Log<AudioEvent>($"Não tocando: {audioSample.name}");
                SetupSource(source);
                _cachedMonoBehaviour.StartCoroutine(FadeAudio(source, fadeTime, 0f, GetVolume));
                source.Play();
            }
        }

        private IEnumerator TransitionAudio(AudioSource source, float fadeTime)
        {
            if (source == null) yield break;
            var startVolume = source.volume;

            // Fade out da música atual
            yield return _cachedMonoBehaviour.StartCoroutine(FadeAudio(source, fadeTime, startVolume, 0f));

            // Configura e toca a nova música
            SetupSource(source);
            source.Play();

            // Fade in da nova música
            yield return _cachedMonoBehaviour.StartCoroutine(FadeAudio(source, fadeTime, 0f, GetVolume));
        }

        public override void PlayOnShot(AudioSource source)
        {
            if (audioSample.audioClip == null || source == null) return;
            if (audioMixerGroup != null) source.outputAudioMixerGroup = audioMixerGroup;
            source.PlayOneShot(audioSample.audioClip, GetVolume);
        }

        public override void Stop(AudioSource source, MonoBehaviour monoBehaviour, float fadeTime = 1.0f)
        {
            if (audioSample.audioClip == null || source == null) return;
            if (IsPlaying(source) && monoBehaviour != null)
            {
                monoBehaviour.StartCoroutine(FadeAudio(source, fadeTime, source.volume, 0f));
            }
        }

        private bool IsPlaying(AudioSource source)
        {
            return source.isPlaying && source.clip == audioSample.audioClip;
        }

        private static IEnumerator FadeAudio(AudioSource source, float fadeTime, float starts, float ends)
        {
            var i = 0.0f;
            var step = 1.0f / fadeTime;

            while (i <= 1.0f)
            {
                i += step * Time.deltaTime;
                source.volume = Mathf.Lerp(starts, ends, i);
                yield return new WaitForSeconds(step * Time.deltaTime);
            }

            if (ends <= 0)
            {
                source.Stop();
            }
        }

        private void SetupSource(AudioSource source)
        {
            source.outputAudioMixerGroup = audioMixerGroup;
            source.clip = audioSample.audioClip;
            source.volume = GetVolume;
            source.pitch = GetPitch;
            source.loop = audioSample.loop;
        }

        private float GetVolume => Random.Range(audioSample.volume.x, audioSample.volume.y);

        private float GetPitch => Random.Range(audioSample.pitch.x, audioSample.pitch.y);

        private void Cleanup()
        {
            // Limpar o cache do MonoBehaviour
            if (_cachedMonoBehaviour == null) return;
            _cachedMonoBehaviour.StopAllCoroutines();
            _cachedMonoBehaviour = null;
        }
    }
}
