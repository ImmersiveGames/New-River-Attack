using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ImmersiveGames.SaveManagers;
using ImmersiveGames.StateManager;
using ImmersiveGames.Utils;
using UnityEngine;
using UnityEngine.Audio;

namespace ImmersiveGames
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : Singleton<AudioManager>
    { 
        [SerializeField] private AudioSource bgmAudioSource;
        [SerializeField] private AudioSource sfxAudioSource;
        [SerializeField] private AudioMixer mixerGroup;
        [SerializeField] private AudioIndex mapStateBgm;
        [SerializeField] private AudioIndex mapMenuSfx;
        
        [Header("Fade Sounds")] public float fadeSound = 0.1f;

        private static AudioSource _bgmAudioSource;
        private static AudioSource _sfxAudioSource;
        private static MapAudioEvent[] _mapStateBgm;
        private static MapAudioEvent[] _mapMenuSfx;
        private const string SfxMouseClick = "SFX MouseClick";
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
            var volumeMusic = SoundBase10(GameOptionsSave.instance.musicVolume);
            var volumeSfx = SoundBase10(GameOptionsSave.instance.sfxVolume);
            mixerGroup.SetFloat("MusicVolume", volumeMusic);
            mixerGroup.SetFloat("SfxVolume", volumeSfx);
        }

        // Adicione esta função para obter o AudioEvent com base no IState fornecido
        private static AudioEvent GetAudioEventForState(IState state, IEnumerable<MapAudioEvent> mapAudioEvent)
        {
            return (from stateBgm in mapAudioEvent where stateBgm.stateName == state.stateName select stateBgm.soundAudioEvent).FirstOrDefault();
            // Retorna null se não encontrar correspondência
        }
        private static AudioEvent GetAudioEventForState(string stateName, IEnumerable<MapAudioEvent> mapAudioEvent)
        {
            return (from stateBgm in mapAudioEvent where stateBgm.stateName == stateName select stateBgm.soundAudioEvent).FirstOrDefault();
            // Retorna null se não encontrar correspondência
        }

        public static void PlayMouseClick()
        {
            var mouseClick = GetAudioEventForState(SfxMouseClick, _mapMenuSfx);
            if (mouseClick == null)
            {
                Debug.Log($"Não Encontrou um audio relativo ao nome: {SfxMouseClick}");
                return;
            }
            mouseClick.PlayOnShot(_sfxAudioSource);
        }
        public static void PlayNotifications()
        {
            var mouseClick = GetAudioEventForState(SfxNotifications, _mapMenuSfx);
            if (mouseClick == null)
            {
                Debug.Log($"Não Encontrou um audio relativo ao nome: {SfxNotifications}");
                return;
            }
            mouseClick.PlayOnShot(_sfxAudioSource);
        }

        public void PlayBGM(IState state)
        {
            // Obtém o AudioEvent correspondente ao estado
            var bgm = GetAudioEventForState(state, _mapStateBgm);
            if (bgm == null)
            {
                Debug.Log($"Não Encontrou um audio relativo ao nome: {state.stateName}");
            }

            // Verifica se há algo tocando atualmente
            if (bgmAudioSource.isPlaying)
            {
                // Se houver uma nova música para tocar, inicia uma transição suave (fade-out)
                StartCoroutine(FadeOut(_bgmAudioSource, fadeSound));
            }
            // Se houver uma nova música para tocar, inicia a reprodução com uma transição suave (fade-in)
            if (bgm != null)
            {
                bgm.Play(_bgmAudioSource, this, fadeSound);
            }
        }

        private static IEnumerator FadeOut(AudioSource source, float fadeTime)
        {
            var startVolume = source.volume;

            // Fade-out gradual
            while (source.volume > 0)
            {
                source.volume -= startVolume * Time.deltaTime / fadeTime;
                yield return null;
            }

            // Pára a música
            source.Stop();

            // Restaura o volume para evitar problemas futuros
            source.volume = startVolume;
        }
        
        /*
         * SoundBase10(float normalizeNumber)
         *  - transforma um numero base de 0.0 à 1.0 em um valor de metrica para sons (dB)
         */
        private static float SoundBase10(float normalizeNumber)
        {
            return Mathf.Log10(normalizeNumber) * 20f;
        }
    }
}
