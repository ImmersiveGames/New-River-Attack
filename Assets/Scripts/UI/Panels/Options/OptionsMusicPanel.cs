using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
namespace RiverAttack
{
    public class OptionsMusicPanel : MonoBehaviour
    {
        enum TypeMusicVolume{
            MusicVolume,
            SfxVolume
        }
        [SerializeField] TypeMusicVolume typeMusicVolume;
        [SerializeField] AudioMixer mixerGroup;
        Slider m_SliderControl;
        [SerializeField, Range(0.0001f, 1f)]  float defaultVolume = .5f;
        
        AudioSource m_AudioSource;
        GameSettings m_GameSettings;
        #region UNITYMETHODS
        void Awake()
        {
            m_SliderControl = GetComponentInChildren<Slider>();
        }

        void Start()
        {
            SetVolume();
        }
        void OnEnable()
        {
            m_GameSettings = GameSettings.instance;
            m_SliderControl.value = (m_GameSettings.musicVolume == 0) ? defaultVolume : m_GameSettings.musicVolume;
        }

        void OnDisable()
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
        public void SetVolume()
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
