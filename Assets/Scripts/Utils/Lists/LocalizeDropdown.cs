using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using TMPro;
using UnityEngine.UI;
public class LocalizeDropdown : MonoBehaviour
{
 
    [Serializable]
    public class LocalizedDropdownOption
    {
        public LocalizedString text;
 
        public LocalizedSprite sprite; //not implemented yet!
    }
 
    public List<LocalizedDropdownOption> options;
    public int selectedOptionIndex = 0;
    private Locale m_CurrentLocale = null;

    private Dropdown dropdown
    {
        get
        {
            return GetComponent<Dropdown>();
        }
    }


    private void Start()
    {
        GetLocale();
        UpdateDropdown(m_CurrentLocale);
        LocalizationSettings.SelectedLocaleChanged += UpdateDropdown;
    }


    private void OnEnable()=> LocalizationSettings.SelectedLocaleChanged += UpdateDropdown;
    private void OnDisable()=> LocalizationSettings.SelectedLocaleChanged -= UpdateDropdown;
    private void OnDestroy() => LocalizationSettings.SelectedLocaleChanged -= UpdateDropdown;


    private void GetLocale()
    {
        var locale = LocalizationSettings.SelectedLocale;
        if (m_CurrentLocale != null && locale != m_CurrentLocale)
        {
            m_CurrentLocale = locale;
        }
    }


    private void UpdateDropdown(Locale locale)
    {
        selectedOptionIndex = dropdown.value;
        dropdown.ClearOptions();
 
        foreach (string localizedText in options.Select(t => t.text.GetLocalizedString()))
        {
            dropdown.options.Add(new Dropdown.OptionData(localizedText, null));
        }
 
        dropdown.value = selectedOptionIndex;
        dropdown.RefreshShownValue();
    }
 
}
