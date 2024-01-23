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
        [SerializeField] protected List<LocalizedString> dropdownOptions;
        protected TMP_Dropdown graphicsDropdown;
        internal GameSettings gameSettings;
        protected Locale actualLocal;
        
        void Awake()
        {
            graphicsDropdown = GetComponentInChildren<TMP_Dropdown>();
            gameSettings = GameSettings.instance;
            if (gameSettings.startLocale == null)
                gameSettings.startLocale = LocalizationSettings.SelectedLocale;
        }
        protected virtual void OnEnable()
        {
            actualLocal = gameSettings.startLocale;
            LocalizationSettings.SelectedLocaleChanged += SetDropdown;
            SetDropdown(actualLocal);
        }

        protected virtual void SetDropdown(Locale newLocale)
        {
            var tmpDropdownOptions = dropdownOptions.Select(t => new TMP_Dropdown.OptionData(t.GetLocalizedString())).ToList();
            if (!graphicsDropdown) graphicsDropdown = GetComponentInChildren<TMP_Dropdown>();
            ResetDropDown(tmpDropdownOptions);
        }
        
        internal void ResetDropDown(List<TMP_Dropdown.OptionData> tmpDropdownOptions)
        {
            graphicsDropdown.ClearOptions();
            graphicsDropdown.options = tmpDropdownOptions;
        }
    }
}
