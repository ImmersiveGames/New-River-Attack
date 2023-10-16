using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace RiverAttack
{
    public class MenuOptionManager : MonoBehaviour
    {
        [Header("Audio Options References")]
        [SerializeField] AudioMixer mixerGroup;
        [SerializeField] Slider musicVolumeSlider;
        [SerializeField, Range(0.0001f, 1f)] float defaultMusicVolume = .5f;
        [SerializeField] Slider sfxVolumeSlider;
        [SerializeField, Range(0.0001f, 1f)] float defaultSfxVolume = .5f;

        [Header("Local Options References")]
        [SerializeField] Button engBtn;
        [SerializeField] Button ptBrBtn;
        bool m_ActiveLocaleButton;
        Locale m_ActualLocal;
        AudioSource m_AudioSource;
        GameSettings m_GameSettings;

        #region UNITYMETHODS
        void OnEnable()
        {
            m_GameSettings = GameSettings.instance;
            musicVolumeSlider.value = (m_GameSettings.musicVolume == 0) ? defaultMusicVolume : m_GameSettings.musicVolume;
            sfxVolumeSlider.value = (m_GameSettings.sfxVolume == 0) ? defaultSfxVolume : m_GameSettings.sfxVolume;
            if (m_GameSettings.startLocale == null)
                m_GameSettings.startLocale = LocalizationSettings.SelectedLocale;
            m_ActualLocal = m_GameSettings.startLocale;
            SetLocaleButton(m_ActualLocal);
        }
        void Start()
        {
            SetMusicVolume();
            SetSfxVolume();
        }
        void OnDisable()
        {
            m_GameSettings.musicVolume = musicVolumeSlider.value;
            m_GameSettings.sfxVolume = sfxVolumeSlider.value;
            m_GameSettings.startLocale = m_ActualLocal;
        }
  #endregion

        public void SetMusicVolume()
        {
            float volume = Mathf.Log10(musicVolumeSlider.value) * 20f;
            m_GameSettings.musicVolume = musicVolumeSlider.value;
            mixerGroup.SetFloat("MusicVolume", volume);

            //Debug.Log("Volume da Musica: " + volume.ToString());
        }

        void SetLocaleButton(Locale actualLocale)
        {
            //TODO: Fazer O Botão ser Selecionado
            string localCode = actualLocale.Identifier.Code;
            switch (localCode)
            {
                case "en":
                    EventSystem.current.SetSelectedGameObject(engBtn.gameObject, new BaseEventData(EventSystem.current));
                    //engBtn.Select();
                    break;
                case "pt-BR":
                    EventSystem.current.SetSelectedGameObject(ptBrBtn.gameObject, new BaseEventData(EventSystem.current));
                    //ptBrBtn.Select();
                    break;
            }
        }
        public void ButtonChangeLocale(int localeId)
        {
            if (m_ActiveLocaleButton) return;
            StartCoroutine(SetLocale(localeId));
        }
        IEnumerator SetLocale(int localeId)
        {
            m_ActiveLocaleButton = true;
            yield return LocalizationSettings.InitializationOperation;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeId];
            m_ActualLocal = m_GameSettings.startLocale = LocalizationSettings.SelectedLocale;
            m_ActiveLocaleButton = false;
        }

        public void SetSfxVolume()
        {
            float volume = Mathf.Log10(sfxVolumeSlider.value) * 20f;
            m_GameSettings.sfxVolume = sfxVolumeSlider.value;
            mixerGroup.SetFloat("SFXVolume", volume);

            //Debug.Log("Volume de SFX: " + volume.ToString());
        }
    }
}
