using System;
using ImmersiveGames.Utils;
using UnityEngine;
using UnityEngine.Localization;

namespace ImmersiveGames.SaveManagers
{
    [CreateAssetMenu(fileName = "GameOptionsSave", menuName = "ImmersiveGames/GameOptionsSave", order = 1)]
    public class GameOptionsSave : SingletonScriptable<GameOptionsSave>
    {
        [Header("Options Localization")]
        public Locale startLocale;
        
        [Header("Options Sound And Music")]
        public float bgmVolume;
        public float sfxVolume;
        private new static string GetResourcePath() => "SavesSO/GameOptionsSave";
        public float GetVolumeLog10(AudioMixGroup type, float volumeDefault = 1f)
        {
            var volume = GetVolume(type, volumeDefault);
            return AudioUtils.SoundBase10(volume);
        }
        public float GetVolume(AudioMixGroup type, float volumeDefault = 1f)
        {
            return type switch
            {
                AudioMixGroup.BgmVolume => bgmVolume > 0.0f ? bgmVolume : volumeDefault,
                AudioMixGroup.SfxVolume => sfxVolume > 0.0f ? sfxVolume : volumeDefault,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        // Define o volume com base no tipo de volume.
        public void SetVolume(AudioMixGroup type, float volume)
        {
            switch (type)
            {
                case AudioMixGroup.BgmVolume:
                    bgmVolume = volume;
                    break;
                case AudioMixGroup.SfxVolume:
                    sfxVolume = volume;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}