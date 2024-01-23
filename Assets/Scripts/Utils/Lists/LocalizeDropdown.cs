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
    Locale m_CurrentLocale = null;
    Dropdown dropdown
    {
        get
        {
            return GetComponent<Dropdown>();
        }
    }


    void Start()
    {
        GetLocale();
        UpdateDropdown(m_CurrentLocale);
        LocalizationSettings.SelectedLocaleChanged += UpdateDropdown;
    }
 
 
    void OnEnable()=> LocalizationSettings.SelectedLocaleChanged += UpdateDropdown;
    void OnDisable()=> LocalizationSettings.SelectedLocaleChanged -= UpdateDropdown;
    void OnDestroy() => LocalizationSettings.SelectedLocaleChanged -= UpdateDropdown;
 
 
 
    void GetLocale()
    {
        var locale = LocalizationSettings.SelectedLocale;
        if (m_CurrentLocale != null && locale != m_CurrentLocale)
        {
            m_CurrentLocale = locale;
        }
    }
 
 
    void UpdateDropdown(Locale locale)
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
