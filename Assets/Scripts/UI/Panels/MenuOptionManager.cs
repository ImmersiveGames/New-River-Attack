using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System.Collections.Generic;

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
        [SerializeField] TMP_Dropdown gpx_QualityDropdown;
        [SerializeField] DialogObject dialogQualityLocalization;
        [SerializeField] TMP_Dropdown gpx_ResolutionDropdown;
        [SerializeField] TMP_Text debugText;
        [SerializeField] TMP_Dropdown framerateDropdown;

        int actualQuality;
        Resolution actualResolution;
        RefreshRate actualRefreshRate;

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
            SetInitialGrapicsValues();
            SetGraphicsQualityDropDwon();
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
            SetGraphicsQualityDropDwon();
        }

        public void SetSfxVolume()
        {
            float volume = Mathf.Log10(sfxVolumeSlider.value) * 20f;
            m_GameSettings.sfxVolume = sfxVolumeSlider.value;
            mixerGroup.SetFloat("SFXVolume", volume);

            //Debug.Log("Volume de SFX: " + volume.ToString());
        }

        void SetInitialGrapicsValues() {
            actualQuality = m_GameSettings.actualQuality;
            actualResolution.width = m_GameSettings.actualResolutionWidth;
            actualResolution.height = m_GameSettings.actualResolutionHeight;
        }

        void SetGraphicsQualityDropDwon()
        {
            string[] qualityLevels;

            Debug.Log(m_ActualLocal.Identifier.Code);

            if (dialogQualityLocalization != null)
            {
                if (m_ActualLocal.Identifier.Code == "en")
                {
                    qualityLevels = dialogQualityLocalization.dialogSentences_EN;
                }
                else if (m_ActualLocal.Identifier.Code == "pt-BR")
                {
                    qualityLevels = dialogQualityLocalization.dialogSentences_PT_BR;
                }
                else
                {
                    qualityLevels = QualitySettings.names;
                }
            }

            else
            {
                qualityLevels = QualitySettings.names;
            }
            
            
            gpx_QualityDropdown.ClearOptions();

            gpx_QualityDropdown.AddOptions(new List<string>(qualityLevels));

            gpx_QualityDropdown.value = actualQuality;

            gpx_QualityDropdown.onValueChanged.AddListener(delegate
            {
                OnQualityChanged(gpx_QualityDropdown);
            });
        }

        void OnQualityChanged(TMP_Dropdown dropdown)
        {
            // Obter o valor selecionado e aplicar a qualidade gráfica.
            QualitySettings.SetQualityLevel(dropdown.value);
            actualQuality = dropdown.value;

            m_GameSettings.actualQuality = actualQuality;

            Debug.Log("Apliquei as qualidade grafica: " + QualitySettings.GetQualityLevel());
        }

        void SetResolutionDropDown()
        {
            Resolution[] allResolutions = Screen.resolutions;

            // Remover duplicatas do array de resoluções.
            Resolution[] resolutions = RemoveDuplicateResolutions(allResolutions);

            gpx_ResolutionDropdown.ClearOptions();

            List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>();

            // Adicionar cada opção de resolução à lista em ordem inversa, evitando duplicatas.
            foreach (Resolution resolution in resolutions)
            {
                string optionText = resolution.width + " x " + resolution.height;
                dropdownOptions.Add(new TMP_Dropdown.OptionData(optionText));
            }

            gpx_ResolutionDropdown.AddOptions(dropdownOptions);

            gpx_ResolutionDropdown.onValueChanged.AddListener(delegate
            {
                OnResolutionChanged(gpx_ResolutionDropdown, resolutions);
            });

            SetInitialResolutionValue(gpx_ResolutionDropdown, resolutions);
        }

        Resolution[] RemoveDuplicateResolutions(Resolution[] resolutions)
        {
            HashSet<string> uniqueResolutions = new HashSet<string>();
            
            List<Resolution> uniqueList = new List<Resolution>();

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

            Debug.Log(actualResolution);

            for (int i = 0; i < resolutions.Length; i++)
            {
                if (currentResolution.width == resolutions[i].width &&
                    currentResolution.height == resolutions[i].height)
                {
                    dropdown.value = i;
                    break;
                }
            }
        }

        void OnResolutionChanged(TMP_Dropdown dropdown, Resolution[] resolutions)
        {
            // Obter o valor selecionado e aplicar a resolução.
            actualResolution.width = resolutions[dropdown.value].width;
            actualResolution.height = resolutions[dropdown.value].height;

            m_GameSettings.actualResolutionWidth = actualResolution.width;
            m_GameSettings.actualResolutionHeight = actualResolution.height;
            
            Screen.SetResolution(actualResolution.width, actualResolution.height, FullScreenMode.FullScreenWindow, actualRefreshRate);

            Debug.Log("Apliquei resolução: " + actualResolution);
        }

        void SetFrameRateDropdown()
        {
            // Limpar as opções existentes no Dropdown.
            framerateDropdown.ClearOptions();

            // Criar uma lista de opções para o Dropdown.
            List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>();

            // Adicionar as opções de taxa de quadros à lista.
            dropdownOptions.Add(new TMP_Dropdown.OptionData("60 FPS"));
            dropdownOptions.Add(new TMP_Dropdown.OptionData("30 FPS"));


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
            RefreshRate currentFramerate = GetRefreshRateForCurrentResolution();
            actualRefreshRate = currentFramerate;

            // Definir o valor inicial com base na taxa de quadros atual.
            if (currentFramerate.value == 60)
            {
                dropdown.value = 0; // 60 FPS
            }
            else
            {
                dropdown.value = 1; // 30 FPS
            }
        }

        void OnFramerateChanged(TMP_Dropdown dropdown)
        {
            // Obter o valor selecionado e aplicar a taxa de quadros.
            int frameRate = dropdown.value == 0 ? 60 : 30;

            RefreshRate selectedFramerate;

            uint numerator = (uint)((uint)dropdown.value == 0 ? 60 : 30);
            uint denominator = (uint)1;

            selectedFramerate.numerator = numerator;
            selectedFramerate.denominator = denominator;

            Debug.Log(selectedFramerate.value);

            actualRefreshRate = selectedFramerate;

            Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.FullScreenWindow, selectedFramerate);

            Application.targetFrameRate = frameRate;
        }

        RefreshRate GetRefreshRateForCurrentResolution()
        {
            Resolution currentResolution = Screen.currentResolution;
            return currentResolution.refreshRateRatio;
        }

    }   

}
