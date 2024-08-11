using System.Collections.Generic;
using System.Linq;
using ImmersiveGames.AudioEvents;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.StateManagers.Interfaces;
using ImmersiveGames.Utils;
using NewRiverAttack.LevelBuilder;
using NewRiverAttack.SaveManagers;
using UnityEngine;
using UnityEngine.Audio;

namespace ImmersiveGames
{
    public partial class AudioManager : Singleton<AudioManager>
    {
        private const float BGMVolumeDefault = 1f;
        private const float SfxVolumeDefault = 1f;

        [SerializeField] private AudioSource bgmAudioSource;
        [SerializeField] private AudioSource sfxAudioSource;
        [SerializeField] private AudioMixer mixerGroup;
        [SerializeField] private AudioIndex mapStateBgm;
        [SerializeField] private AudioIndex mapMenuSfx;

        [Header("Fade Sounds")] 
        public float fadeSoundDuration = 0.5f;

        private MapAudioEvent[] _mapStateBgm;
        private MapAudioEvent[] _mapMenuSfx;

        private void OnEnable()
        {
            _mapStateBgm = mapStateBgm.mapAudioEvents;
            _mapMenuSfx = mapMenuSfx.mapAudioEvents;
        }

        private void Start()
        {
            RecoverAudioSettings();
        }

        /// <summary>
        /// Recovers audio settings based on the game settings.
        /// </summary>
        private void RecoverAudioSettings()
        {
            var gameOptionsSave = GameOptionsSave.instance;
            mixerGroup.SetFloat(EnumAudioMixGroup.BgmVolume.ToString(), 
                gameOptionsSave.GetVolumeLog10(EnumAudioMixGroup.BgmVolume, BGMVolumeDefault));
            mixerGroup.SetFloat(EnumAudioMixGroup.SfxVolume.ToString(), 
                gameOptionsSave.GetVolumeLog10(EnumAudioMixGroup.SfxVolume, SfxVolumeDefault));
        }

        // Optimized method to get the AudioEvent based on IState
        private AudioEvent GetAudioEventForState(string stateName, IEnumerable<MapAudioEvent> mapAudioEvent)
        {
            return (from stateBgm in mapAudioEvent where stateBgm.stateName == stateName select stateBgm.soundAudioEvent).FirstOrDefault();
        }

        public void PlaySfx(string sfxName)
        {
            var audioEventForState = GetAudioEventForState(sfxName, _mapMenuSfx);
            if (audioEventForState == null)
            {
                #if UNITY_EDITOR
                DebugManager.Log<AudioManager>($"Audio not found for name: {sfxName}");
                #endif
                return;
            }
            audioEventForState.PlayOnShot(sfxAudioSource);
        }

        public void PlayBGMOneShot(string state)
        {
            bgmAudioSource.Stop();
            var audioEventForState = GetAudioEventForState(state, _mapStateBgm);
            if (audioEventForState == null)
            {
                #if UNITY_EDITOR
                DebugManager.Log<AudioManager>($"Audio not found for state: {state}");
                #endif
                return;
            }
            audioEventForState.PlayOnShot(bgmAudioSource);
        }

        public void PlayBGM(IState state)
        {
            PlayBGM(state.StateName);
        }

        public void PlayBGM(string state)
        {
            var audioEventForState = GetAudioEventForState(state, _mapStateBgm);
            if (audioEventForState == null)
            {
                #if UNITY_EDITOR
                DebugManager.LogWarning<AudioManager>($"Audio not found for state: {state}");
                #endif
                return;
            }
            audioEventForState.Play(bgmAudioSource, this, fadeSoundDuration);
        }

        public void StopBGM()
        {
            bgmAudioSource.Stop();
        }
    }
}