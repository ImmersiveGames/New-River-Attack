using System.Collections.Generic;
using System.Linq;
using ImmersiveGames.AudioEvents;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.LevelBuilder;
using ImmersiveGames.SaveManagers;
using ImmersiveGames.StateManagers.Interfaces;
using ImmersiveGames.Utils;
using UnityEngine;
using UnityEngine.Audio;

namespace ImmersiveGames
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : Singleton<AudioManager>
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
        private const string SfxMouseClick = "SFX MouseClick";
        private const string SfxMouseOver = "SFX MouseOver";
        private const string SfxNotifications = "SFX Notification";

        protected override void Awake()
        {
            base.Awake();
            _mapStateBgm = mapStateBgm.mapAudioEvents;
            _mapMenuSfx = mapMenuSfx.mapAudioEvents;
            _bgmAudioSource = bgmAudioSource;
            _sfxAudioSource = sfxAudioSource;
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
            mixerGroup.SetFloat(AudioMixGroup.BgmVolume.ToString(), 
                gameOptionsSave.GetVolumeLog10(AudioMixGroup.BgmVolume, BGMVolumeDefault));
            mixerGroup.SetFloat(AudioMixGroup.SfxVolume.ToString(), 
                gameOptionsSave.GetVolumeLog10(AudioMixGroup.SfxVolume, SfxVolumeDefault));
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

        public static void PlayMouseClick()
        {
            PlayOneShot(SfxMouseClick);
        }
        public static void PlayNotifications()
        {
            PlayOneShot(SfxNotifications);
        }
        public static void PlayMouseOver()
        {
            PlayOneShot(SfxMouseOver);
        }

        public void PlayBGM(IState state)
        {
            // Obtém o AudioEvent correspondente ao estado
            var audioEventForState = GetAudioEventForState(state, _mapStateBgm);
            if (audioEventForState == null)
            {
                DebugManager.Log<AudioManager>($"Não Encontrou um audio relativo ao nome: {state.StateName}");
            }
            // Se houver uma nova música para tocar, inicia a reprodução com uma transição suave (fade-in)
            if (audioEventForState != null)
            {
                audioEventForState.Play(_bgmAudioSource, this, fadeSoundDuration);
            }
        }
        // Função para fade de volume usando a função FadeProperty.
    }

    public enum AudioMixGroup
    {
        BgmVolume, SfxVolume
    }

    public enum BgmTypes
    {
        Menu, HUD, GameOver, Complete, Tutorial,
        Grass = LevelTypes.Grass, 
        Forest= LevelTypes.Forest, 
        Swamp= LevelTypes.Swamp, 
        Antique= LevelTypes.Antique, 
        Desert= LevelTypes.Desert, 
        Ice= LevelTypes.Ice, Boss = LevelTypes.Boss
    }
}
