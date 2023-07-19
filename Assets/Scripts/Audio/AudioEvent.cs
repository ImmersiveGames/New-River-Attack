using GD.MinMaxSlider;
using UnityEngine;
using Utils;
namespace RiverAttack
{
    public abstract class AudioEvent : ScriptableObject
    {
        public abstract void Play(AudioSource source);
        public abstract void PlayOnShot(AudioSource source);
        public abstract void Stop(AudioSource source);
    }
    [System.Serializable]
    public class AudioEventClip
    {
        public string name;
        public AudioClip audioClip;
        [MinMaxSlider(0f,1f)]
        public Vector2 volume;
        [MinMaxSlider(-3f,3f)]
        public Vector2 pitch;
        public bool loop;
    }
}
