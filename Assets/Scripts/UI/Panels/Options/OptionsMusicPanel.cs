using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
namespace RiverAttack
{
    public class OptionsMusicPanel : MonoBehaviour
    {
        private enum TypeMusicVolume{
            MusicVolume,
            SfxVolume
        }
        [SerializeField] private TypeMusicVolume typeMusicVolume;
        [SerializeField] private AudioMixer mixerGroup;
        private Slider m_SliderControl;
        [SerializeField, Range(0.0001f, 1f)] private float defaultVolume = .5f;

        private AudioSource m_AudioSource;
        private GameSettings m_GameSettings;
        #region UNITYMETHODS

        private void Awake()
        {
            m_SliderControl = GetComponentInChildren<Slider>();
        }

        private void Start()
        {

        }

        private void OnEnable()
        {
            m_GameSettings = GameSettings.instance;
            m_SliderControl.value = (m_GameSettings.musicVolume == 0) ? defaultVolume : m_GameSettings.musicVolume;
        }

        private void OnDisable()
        {
            switch (typeMusicVolume)
            {
                case TypeMusicVolume.MusicVolume:
                    m_GameSettings.musicVolume = m_SliderControl.value;
                    break;
                case TypeMusicVolume.SfxVolume:
                    m_GameSettings.sfxVolume = m_SliderControl.value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion
        public void SetVolume(float slideValue)
        {
            float volume = Mathf.Log10(m_SliderControl.value) * 20f;
            switch (typeMusicVolume)
            {
                case TypeMusicVolume.MusicVolume:
                    m_GameSettings.musicVolume = m_SliderControl.value;
                    break;
                case TypeMusicVolume.SfxVolume:
                    m_GameSettings.sfxVolume = m_SliderControl.value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            mixerGroup.SetFloat(typeMusicVolume.ToString(), volume);
            //Debug.Log("Volume da Musica: " + volume.ToString());
        }
        
    }
}
