using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Serialization;

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
        [SerializeField]
        GameSettings gameSettings;

        #region UNITYMETHODS
        void OnEnable()
        {
            gameSettings = GameSettings.instance;
            musicVolumeSlider.value = (gameSettings.musicVolume == 0) ? defaultMusicVolume : gameSettings.musicVolume;
            sfxVolumeSlider.value = (gameSettings.sfxVolume == 0) ? defaultSfxVolume : gameSettings.sfxVolume;
            if (gameSettings.startLocale == null)
                gameSettings.startLocale = LocalizationSettings.SelectedLocale;
            m_ActualLocal = gameSettings.startLocale;
            SetLocaleButton(m_ActualLocal);
        }
        void Start()
        {
            SetMusicVolume();
            SetSfxVolume();
        }
        void OnDisable()
        {
            gameSettings.musicVolume = musicVolumeSlider.value;
            gameSettings.sfxVolume = sfxVolumeSlider.value;
            gameSettings.startLocale = m_ActualLocal;
        }
  #endregion

        public void SetMusicVolume()
        {
            float volume = Mathf.Log10(musicVolumeSlider.value) * 20f;
            gameSettings.musicVolume = musicVolumeSlider.value;
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
            m_ActualLocal = gameSettings.startLocale = LocalizationSettings.SelectedLocale;
            m_ActiveLocaleButton = false;
        }

        public void SetSfxVolume()
        {
            float volume = Mathf.Log10(sfxVolumeSlider.value) * 20f;
            gameSettings.sfxVolume = sfxVolumeSlider.value;
            mixerGroup.SetFloat("SFXVolume", volume);

            //Debug.Log("Volume de SFX: " + volume.ToString());
        }
    }
}
