using UnityEngine;
using GD.MinMaxSlider;

namespace ImmersiveGames.AudioEvents
{
    public abstract class AudioEventSo : ScriptableObject
    {
        public abstract void Play(AudioSource source, MonoBehaviour monoBehaviour, float fadeTime = 1.0f);
        public abstract void PlayOnShot(AudioSource source);
        public abstract void Stop(AudioSource source, MonoBehaviour monoBehaviour, float fadeTime = 1.0f);
    }
    [System.Serializable]
    public class AudioEventClip
    {
        public string name;
        public AudioClip audioClip;
        [MinMaxSlider(0f, 1f)]
        public Vector2 volume;
        [MinMaxSlider(-3f, 3f)]
        public Vector2 pitch;
        public bool loop;
    }
}