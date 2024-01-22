using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System.Collections.Generic;
using System.Linq;
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
        GameSettings m_GameSettings;
        
        [Header("Graphics Options References")]
        [SerializeField] TMP_Dropdown gpxQualityDropdown;
        [SerializeField] DialogObject dialogQualityLocalization;
        [SerializeField] TMP_Dropdown gpxResolutionDropdown;
        [SerializeField] TMP_Text debugText;
        [SerializeField] TMP_Dropdown framerateDropdown;

        int m_ActualQuality;
        Resolution m_ActualResolution;
        RefreshRate m_ActualRefreshRate;
        
        //Translate
        [SerializeField] string tableReference = "StringTableCollection";
        
        
        LocalizedString m_QualitySettingsPerformant;
        LocalizedString m_QualitySettingsBalanced;
        LocalizedString m_QualitySettingsFidelity;

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

            SetFrameRateDropdown();
            SetInitialGraphicsValues();
            SetGraphicsQualityDropDown();
            SetResolutionDropDown();
        }
        void Start()
        {
            SetMusicVolume();
            SetSfxVolume();
        }

        void Update()
        {
            debugText.text = Screen.currentResolution.ToString();
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
            SetGraphicsQualityDropDown();
        }

        public void SetSfxVolume()
        {
            float volume = Mathf.Log10(sfxVolumeSlider.value) * 20f;
            m_GameSettings.sfxVolume = sfxVolumeSlider.value;
            mixerGroup.SetFloat("SFXVolume", volume);

            //Debug.Log("Volume de SFX: " + volume.ToString());
        }

        void SetInitialGraphicsValues() {
            m_ActualQuality = m_GameSettings.actualQuality;
            m_ActualResolution.width = m_GameSettings.actualResolutionWidth;
            m_ActualResolution.height = m_GameSettings.actualResolutionHeight;
        }

        void SetGraphicsQualityDropDown()
        {
            string[] qualityLevels =
            {
                new LocalizedString
                {
                    TableReference = tableReference,
                    TableEntryReference = "QualitySettings_Performant"
                }.ToString(),
                new LocalizedString
                {
                    TableReference = tableReference,
                    TableEntryReference = "QualitySettings_Balanced"
                }.ToString(),
                new LocalizedString
                {
                    TableReference = tableReference,
                    TableEntryReference = "QualitySettings_High Fidelity"
                }.ToString()
            };

            gpxQualityDropdown.ClearOptions();

            gpxQualityDropdown.AddOptions(new List<string>(qualityLevels));

            gpxQualityDropdown.value = m_ActualQuality;

            gpxQualityDropdown.onValueChanged.AddListener(delegate
            {
                OnQualityChanged(gpxQualityDropdown);
            });
        }

        void OnQualityChanged(TMP_Dropdown dropdown)
        {
            // Obter o valor selecionado e aplicar a qualidade gráfica.
            QualitySettings.SetQualityLevel(dropdown.value);
            m_ActualQuality = dropdown.value;

            m_GameSettings.actualQuality = m_ActualQuality;

            Debug.Log("Apliquei as qualidade grafica: " + QualitySettings.GetQualityLevel());
        }

        void SetResolutionDropDown()
        {
            var allResolutions = Screen.resolutions;

            // Remover duplicatas do array de resoluções.
            var resolutions = RemoveDuplicateResolutions(allResolutions);

            gpxResolutionDropdown.ClearOptions();

            var dropdownOptions = resolutions.Select(resolution => resolution.width + " x " + resolution.height).Select(optionText => new TMP_Dropdown.OptionData(optionText)).ToList();

            // Adicionar cada opção de resolução à lista em ordem inversa, evitando duplicatas.

            gpxResolutionDropdown.AddOptions(dropdownOptions);

            gpxResolutionDropdown.onValueChanged.AddListener(delegate
            {
                OnResolutionChanged(gpxResolutionDropdown, resolutions);
            });

            SetInitialResolutionValue(gpxResolutionDropdown, resolutions);
        }

        static Resolution[] RemoveDuplicateResolutions(Resolution[] resolutions)
        {
            var uniqueResolutions = new HashSet<string>();
            
            var uniqueList = new List<Resolution>();

            for (int i = resolutions.Length - 1; i >= 0; i--)
            {
                string resolutionString = resolutions[i].width + "x" + resolutions[i].height;

                if (uniqueResolutions.Add(resolutionString))
                {
                    uniqueList.Add(resolutions[i]);
                }
            }
            
            return uniqueList.ToArray();
        }

        void SetInitialResolutionValue(TMP_Dropdown dropdown, Resolution[] resolutions)
        {
            Resolution currentResolution = currentResolution = Screen.currentResolution;

            Debug.Log(m_ActualResolution);

            for (int i = 0; i < resolutions.Length; i++)
            {
                if (currentResolution.width != resolutions[i].width ||
                    currentResolution.height != resolutions[i].height)
                    continue;
                dropdown.value = i;
                break;
            }
        }

        void OnResolutionChanged(TMP_Dropdown dropdown, IReadOnlyList<Resolution> resolutions)
        {
            // Obter o valor selecionado e aplicar a resolução.
            m_ActualResolution.width = resolutions[dropdown.value].width;
            m_ActualResolution.height = resolutions[dropdown.value].height;

            m_GameSettings.actualResolutionWidth = m_ActualResolution.width;
            m_GameSettings.actualResolutionHeight = m_ActualResolution.height;
            
            Screen.SetResolution(m_ActualResolution.width, m_ActualResolution.height, FullScreenMode.FullScreenWindow, m_ActualRefreshRate);

            Debug.Log("Apliquei resolução: " + m_ActualResolution);
        }

        void SetFrameRateDropdown()
        {
            // Limpar as opções existentes no Dropdown.
            framerateDropdown.ClearOptions();

            // Criar uma lista de opções para o Dropdown.
            var dropdownOptions = new List<TMP_Dropdown.OptionData>
            {
                // Adicionar as opções de taxa de quadros à lista.
                new TMP_Dropdown.OptionData("60 FPS"),
                new TMP_Dropdown.OptionData("30 FPS")
            };


            // Adicionar as opções ao Dropdown.
            framerateDropdown.AddOptions(dropdownOptions);

            // Configurar o callback para a mudança na opção do Dropdown.
            framerateDropdown.onValueChanged.AddListener(delegate
            {
                OnFramerateChanged(framerateDropdown);
            });

            // Definir o valor inicial do Dropdown com base na taxa de quadros atual.
            SetInitialFramerateValue(framerateDropdown);
        }

        void SetInitialFramerateValue(TMP_Dropdown dropdown)
        {
            var currentFramerate = GetRefreshRateForCurrentResolution();
            m_ActualRefreshRate = currentFramerate;

            dropdown.value = currentFramerate.value switch
            {
                // Definir o valor inicial com base na taxa de quadros atual.
                60 => 0,
                _ => 1
            };
        }

        void OnFramerateChanged(TMP_Dropdown dropdown)
        {
            // Obter o valor selecionado e aplicar a taxa de quadros.
            int frameRate = dropdown.value == 0 ? 60 : 30;

            RefreshRate selectedFramerate;

            uint numerator = (uint)((uint)dropdown.value == 0 ? 60 : 30);
            const uint denominator = 1;

            selectedFramerate.numerator = numerator;
            selectedFramerate.denominator = denominator;

            Debug.Log(selectedFramerate.value);

            m_ActualRefreshRate = selectedFramerate;

            Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.FullScreenWindow, selectedFramerate);

            Application.targetFrameRate = frameRate;
        }

        static RefreshRate GetRefreshRateForCurrentResolution()
        {
            var currentResolution = Screen.currentResolution;
            return currentResolution.refreshRateRatio;
        }

    }   

}
