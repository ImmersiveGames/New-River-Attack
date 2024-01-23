using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
namespace RiverAttack
{
    public abstract class OptionsDropDown: MonoBehaviour
    {
        [SerializeField] protected List<LocalizedString> options;
        public int selectedOptionIndex;
        protected TMP_Dropdown dropdown
        {
            get
            {
                return GetComponent<TMP_Dropdown>();
            }
        }
        Locale m_CurrentLocale;
        internal GameSettings gameSettings;
        protected virtual void Awake()
        {
            gameSettings = GameSettings.instance;
        }
        protected virtual void OnEnable()
        {
            LocalizationSettings.SelectedLocaleChanged += UpdateDropdown;
        }

        void Start()
        {
            GetLocale();
            UpdateDropdown(m_CurrentLocale);
        }
        
        void OnDisable()
        {
            LocalizationSettings.SelectedLocaleChanged -= UpdateDropdown;
        }

        void OnDestroy()
        {
            LocalizationSettings.SelectedLocaleChanged -= UpdateDropdown;
        }

        protected virtual void UpdateDropdown(Locale locale)
        {
            dropdown.ClearOptions();
            foreach (string localizedText in options.Select(t => t.GetLocalizedString()))
            {
                dropdown.options.Add(new TMP_Dropdown.OptionData(localizedText, null));
            }
            dropdown.value = selectedOptionIndex;
            dropdown.RefreshShownValue();
            dropdown.onValueChanged.AddListener(delegate
            {
                OnDropdownChanged(dropdown);
            });
        }
        protected virtual void OnDropdownChanged(TMP_Dropdown tmpDropdown)
        {
            //Debug.Log($"Mudou o Valor: {tmpDropdown.value}");
            selectedOptionIndex = tmpDropdown.value;
        }

        void GetLocale()
        {
            var locale = LocalizationSettings.SelectedLocale;
            m_CurrentLocale = locale;
            if (gameSettings.startLocale == null)
            {
                m_CurrentLocale = gameSettings.startLocale = locale;
            }else
            if (m_CurrentLocale != null && locale != m_CurrentLocale)
            {
                m_CurrentLocale = gameSettings.startLocale = locale;
            }
        }
        
        
        /*[SerializeField] protected List<LocalizedString> dropdownOptions;
        protected TMP_Dropdown graphicsDropdown;
        
        protected Locale actualLocal;
        
        void Awake()
        {
            gameSettings = GameSettings.instance;
            if (gameSettings.startLocale == null)
                gameSettings.startLocale = LocalizationSettings.SelectedLocale;
            if (!graphicsDropdown) graphicsDropdown = GetComponentInChildren<TMP_Dropdown>();
        }
        protected virtual void OnEnable()
        {
            SetDropdown(gameSettings.startLocale);
            LocalizationSettings.SelectedLocaleChanged += SetDropdown;
        }

        void OnDisable()
        {
            LocalizationSettings.SelectedLocaleChanged -= SetDropdown;
        }
        void OnDestroy()
        {
            LocalizationSettings.SelectedLocaleChanged -= SetDropdown;
        }

        protected virtual void SetDropdown(Locale newLocale)
        {
            actualLocal = newLocale;
            graphicsDropdown.ClearOptions();
            graphicsDropdown.options = dropdownOptions.Select(t => new TMP_Dropdown.OptionData(t.GetLocalizedString())).ToList();
        }*/
    }
}
