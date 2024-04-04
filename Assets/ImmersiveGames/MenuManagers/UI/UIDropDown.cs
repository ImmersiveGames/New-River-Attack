﻿using System.Collections.Generic;
using System.Linq;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.MenuManagers.PanelOptionsManagers;
using ImmersiveGames.SaveManagers;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace ImmersiveGames.MenuManagers.UI
{
    public abstract class UIDropDown : MonoBehaviour
    {
        [SerializeField] protected List<LocalizedString> options;
        protected int SelectedOptionIndex { get; set; }
        private TMP_Dropdown Dropdown => GetComponent<TMP_Dropdown>();

        private Locale _currentLocale;
        private GameOptionsSave _gameOptionsSave;

        protected virtual void Awake()
        {
            
            _gameOptionsSave = GameOptionsSave.instance;
        }

        protected virtual void OnEnable()
        {
            LocalizationSettings.SelectedLocaleChanged += UpdateDropdown;
        }

        private void Start()
        {
            GetLocale();
            UpdateDropdown(_currentLocale);
        }

        protected virtual void OnDisable()
        {
            LocalizationSettings.SelectedLocaleChanged -= UpdateDropdown;
        }

        protected virtual void OnDestroy()
        {
            LocalizationSettings.SelectedLocaleChanged -= UpdateDropdown;
        }

        protected virtual void UpdateDropdown(Locale locale)
        {
            Dropdown.onValueChanged.RemoveListener(OnDropdownChanged);
            Dropdown.ClearOptions();

            foreach (var localizedText in options.Select(t => t.GetLocalizedString()))
            {
                Dropdown.options.Add(new TMP_Dropdown.OptionData(localizedText, null));
            }

            Dropdown.value = SelectedOptionIndex;
            Dropdown.RefreshShownValue();
            Dropdown.onValueChanged.AddListener(OnDropdownChanged);
        }

        protected virtual void OnDropdownChanged(int value)
        {
            SelectedOptionIndex = value;
        }

        private void GetLocale()
        {
            _currentLocale = LocalizationSettings.SelectedLocale;
            if (_gameOptionsSave.startLocale == null || _currentLocale != null && _currentLocale != _gameOptionsSave.startLocale)
            {
                _gameOptionsSave.startLocale = _currentLocale;
            }
        }
    }
}
