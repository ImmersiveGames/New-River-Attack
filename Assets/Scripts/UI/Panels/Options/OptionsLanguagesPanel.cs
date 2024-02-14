using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
namespace RiverAttack
{
    public class OptionsLanguagesPanel: MonoBehaviour
    {
        [SerializeField] private Locale myLocale;

        private Button m_FlagButton;
        [SerializeField] private Color normalColor;
        [SerializeField] private Color disableColor;
        private static bool _activeLocaleButton;
        private GameSettings m_GameSettings;

        private void Awake()
        {
            m_FlagButton = GetComponent<Button>();
        }

        private void OnEnable()
        {
            m_GameSettings = GameSettings.instance;
            //Debug.Log($"Locale: {myLocale}, {LocalizationSettings.SelectedLocale}");
            ChangeNormalColor(myLocale);
        }

        private void Start()
        {
            LocalizationSettings.SelectedLocaleChanged += ChangeNormalColor;
        }

        private void OnDisable()
        {
            LocalizationSettings.SelectedLocaleChanged -= ChangeNormalColor;
        }

        private void ChangeNormalColor(Locale locale)
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

        private IEnumerator ChangeLocale(Locale locale)
        {
            //Debug.Log($"MEU Local: {locale}, {myLocale}");
            _activeLocaleButton = true;
            yield return LocalizationSettings.InitializationOperation;
            LocalizationSettings.SelectedLocale = m_GameSettings.startLocale = locale;
            _activeLocaleButton = false;
        }
    }
}
