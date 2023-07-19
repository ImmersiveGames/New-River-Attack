using System.Collections;
using UnityEngine.Audio;
using UnityEngine;
using Utils;
namespace RiverAttack
{

    [CreateAssetMenu(fileName = "AudioEventSample", menuName = "Audio Events/Sample Audio", order = 1)]
    public class AudioEventSample : AudioEvent
    {
        [SerializeField]
        public AudioEventClip audioSample;
        [SerializeField]
        public AudioMixerGroup audioMixerGroup;

        //TODO: Habilitar para Grupo de MIXagem;

        public override void Play(AudioSource source)
        {
            if (audioSample?.audioClip) return;
            SetupSource(source);
            source.Play();
        }

        public float getVolume
        {
            get
            {
                var volume = audioSample.volume;
                return Random.Range(volume.minValue, volume.maxValue);
            }
        }

        public float getPitch
        {
            get
            {
                return Random.Range(audioSample.pitch.minValue, audioSample.pitch.maxValue);
            }
        }

        void SetupSource(AudioSource source)
        {
            source.outputAudioMixerGroup = audioMixerGroup;
            source.clip = audioSample.audioClip;
            source.volume = Random.Range(audioSample.volume.minValue, audioSample.volume.maxValue);
            source.pitch = Random.Range(audioSample.pitch.minValue, audioSample.pitch.maxValue);
            source.loop = audioSample.loop;
        }

        public override void PlayOnShot(AudioSource source)
        {
            if (audioSample.audioClip == null) return;
            if (audioMixerGroup != null) source.outputAudioMixerGroup = audioMixerGroup;
            source.PlayOneShot(audioSample.audioClip, Random.Range(audioSample.volume.minValue, audioSample.volume.maxValue));
        }

        public bool IsPlaying(AudioSource source)
        {
            return source.isPlaying && source.clip == audioSample.audioClip;
        }

        public override void Stop(AudioSource source)
        {
            source.Stop();
        }

        public static void UpdateChangePith(AudioSource source, float start, float end)
        {
            source.pitch = Mathf.Clamp(Time.time, start, end);
        }
    }
}
