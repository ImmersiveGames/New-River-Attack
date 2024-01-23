using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
namespace RiverAttack
{
    public class OptionsLanguagesPanel: MonoBehaviour
    {
        [SerializeField] Locale myLocale;
        
        Button m_FlagButton;
        [SerializeField] Color normalColor;
        [SerializeField] Color disableColor;
        static bool _activeLocaleButton;
        GameSettings m_GameSettings;
        void Awake()
        {
            m_FlagButton = GetComponent<Button>();
        }

        void OnEnable()
        {
            m_GameSettings = GameSettings.instance;
            //Debug.Log($"Locale: {myLocale}, {LocalizationSettings.SelectedLocale}");
            ChangeNormalColor(myLocale);
        }

        void Start()
        {
            LocalizationSettings.SelectedLocaleChanged += ChangeNormalColor;
        }

        void OnDisable()
        {
            LocalizationSettings.SelectedLocaleChanged -= ChangeNormalColor;
        }

        void ChangeNormalColor(Locale locale)
        {
            var flagButtonColors = m_FlagButton.colors;
            flagButtonColors.normalColor = LocalizationSettings.SelectedLocale != myLocale ? disableColor : normalColor;
            GetComponent<Button>().colors = flagButtonColors;
        }
        public void ButtonChangeLocale()
        {
            if (_activeLocaleButton) return;
            StartCoroutine(ChangeLocale(myLocale));
        }

        IEnumerator ChangeLocale(Locale locale)
        {
            //Debug.Log($"MEU Local: {locale}, {myLocale}");
            _activeLocaleButton = true;
            yield return LocalizationSettings.InitializationOperation;
            LocalizationSettings.SelectedLocale = m_GameSettings.startLocale = locale;
            _activeLocaleButton = false;
        }
    }
}
