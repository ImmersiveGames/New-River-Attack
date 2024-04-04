using UnityEngine;

namespace ImmersiveGames.AudioEvents
{
    [CreateAssetMenu(fileName = "AudioIndex", menuName = "ImmersiveGames/AudioIndex", order = 2)]
    public class AudioIndex : ScriptableObject
    {
        public MapAudioEvent[] mapAudioEvents;
    }
    [System.Serializable]
    public class MapAudioEvent
    {
        public string stateName;
        public AudioEvent soundAudioEvent;
    }
}