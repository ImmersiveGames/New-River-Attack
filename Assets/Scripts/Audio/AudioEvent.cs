using System.Collections;
using System.Collections.Generic;
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
        [SerializeField]
        public string name;
        [SerializeField]
        public AudioClip audioClip;
        [Tools.MinMaxRangeAttribute(0, 1)]
        public Tools.FloatRanged volume;
        [Tools.MinMaxRangeAttribute(-3, 3)]
        public Tools.FloatRanged pitch;
        public bool loop;
    }
}
