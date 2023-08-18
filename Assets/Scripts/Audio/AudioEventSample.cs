using System.Collections;
using UnityEngine.Audio;
using UnityEngine;
using GD.MinMaxSlider;
namespace RiverAttack
{

    [CreateAssetMenu(fileName = "AudioEventSample", menuName = "Audio Events/Sample Audio", order = 1)]
    public class AudioEventSample : AudioEvent
    {
        [SerializeField]
        public AudioEventClip audioSample;
        [SerializeField]
        public AudioMixerGroup audioMixerGroup;

        public override void Play(AudioSource source)
        {
            if (audioSample?.audioClip == null) return;
            SetupSource(source);
            source.Play();
        }

        public float getVolume
        {
            get
            {
                var volume = audioSample.volume;
                return Random.Range(volume.x, volume.y);
            }
        }
        public float getPitch
        {
            get
            {
                return Random.Range(audioSample.pitch.x, audioSample.pitch.y);
            }
        }
        void SetupSource(AudioSource source)
        {
            source.outputAudioMixerGroup = audioMixerGroup;
            source.clip = audioSample.audioClip;
            source.volume = Random.Range(audioSample.volume.x, audioSample.volume.y);
            source.pitch = Random.Range(audioSample.pitch.x, audioSample.pitch.y);
            source.loop = audioSample.loop;
        }
        public override void PlayOnShot(AudioSource source)
        {
            if (audioSample.audioClip == null) return;
            if (audioMixerGroup != null) source.outputAudioMixerGroup = audioMixerGroup;
            source.PlayOneShot(audioSample.audioClip, Random.Range(audioSample.volume.x, audioSample.volume.y));
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
