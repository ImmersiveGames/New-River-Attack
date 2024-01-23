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
        static bool _activeLocaleButton;
        GameSettings m_GameSettings;
        void Awake()
        {
            m_FlagButton = GetComponent<Button>();
        }

        void OnEnable()
        {
            m_GameSettings = GameSettings.instance;
        }

        void Start()
        {
            Initiate(myLocale);
            LocalizationSettings.SelectedLocaleChanged += Initiate;
        }

        void Initiate(Locale locale)
        {
            if (locale == null || locale != LocalizationSettings.SelectedLocale)
                return;
            //Debug.Log($"EU: {locale.name}");
            m_FlagButton.Select();
        }
        public void ButtonChangeLocale()
        {
            if (_activeLocaleButton) return;
            StartCoroutine(ChangeLocale());
        }

        IEnumerator ChangeLocale()
        {
            _activeLocaleButton = true;
            yield return LocalizationSettings.InitializationOperation;
            LocalizationSettings.SelectedLocale = m_GameSettings.startLocale = myLocale;
            _activeLocaleButton = false;
        }
    }
}
