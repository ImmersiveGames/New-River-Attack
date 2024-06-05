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
        
        [Header("Fade Sounds")] public float fadeSoundDuration = 1f;

        private static AudioSource _bgmAudioSource;
        private static AudioSource _sfxAudioSource;
        private static MapAudioEvent[] _mapStateBgm;
        private static MapAudioEvent[] _mapMenuSfx;
        private static float _fadeSoundDuration;
        
        private void OnEnable()
        {
            _mapStateBgm = mapStateBgm.mapAudioEvents;
            _mapMenuSfx = mapMenuSfx.mapAudioEvents;
            _bgmAudioSource = bgmAudioSource;
            _sfxAudioSource = sfxAudioSource;
            _fadeSoundDuration = fadeSoundDuration;
        }

        private void Start()
        {
            RecoveryAudioSettings();
        }

        /// <summary>
        /// Recovers audio settings based on the game settings.
        /// </summary>
        private void RecoveryAudioSettings()
        {
            var gameOptionsSave = GameOptionsSave.instance;
            mixerGroup.SetFloat(EnumAudioMixGroup.BgmVolume.ToString(), 
                gameOptionsSave.GetVolumeLog10(EnumAudioMixGroup.BgmVolume, BGMVolumeDefault));
            mixerGroup.SetFloat(EnumAudioMixGroup.SfxVolume.ToString(), 
                gameOptionsSave.GetVolumeLog10(EnumAudioMixGroup.SfxVolume, SfxVolumeDefault));
        }

        // Adicione esta função para obter o AudioEvent com base no IState fornecido
        private static AudioEvent GetAudioEventForState(IState state, IEnumerable<MapAudioEvent> mapAudioEvent)
        {
            return GetAudioEventForState(state.StateName, mapAudioEvent);
            // Retorna null se não encontrar correspondência
        }
        
        private static AudioEvent GetAudioEventForState(string stateName, IEnumerable<MapAudioEvent> mapAudioEvent)
        {
            return (from stateBgm in mapAudioEvent where stateBgm.stateName == stateName select stateBgm.soundAudioEvent).FirstOrDefault();
            // Retorna null se não encontrar correspondência
        }

        public static void PlayOneShot(string stateName)
        {
            var audioEventForState = GetAudioEventForState(stateName, _mapMenuSfx);
            if (audioEventForState == null)
            {
                DebugManager.Log<AudioManager>($"Não Encontrou um audio relativo ao nome: {stateName}");
                return;
            }
            audioEventForState.PlayOnShot(_sfxAudioSource);
        }
        
        public static void PlayBGM(IState state)
        {
            PlayBGM(state.StateName);
        }
        public static void PlayBGM(string state)
        {
            // Obtém o AudioEvent correspondente ao estado
            var audioEventForState = GetAudioEventForState(state, _mapStateBgm);
            if (audioEventForState == null)
            {
                DebugManager.LogWarning<AudioManager>($"Não Encontrou um audio relativo ao nome: {state}");
                return;
            }
            audioEventForState.Play(_bgmAudioSource, instance, _fadeSoundDuration);
        }
    }
}
