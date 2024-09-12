using System.Collections;
using ImmersiveGames.DebugManagers;
using NewRiverAttack.SaveManagers;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace ImmersiveGames.MenuManagers.PanelOptionsManagers
{
    public class PanelLanguagesOptions: MonoBehaviour
    {
        [SerializeField] private Locale myLocale;
        private Button _languageButton;
        private GameOptionsSave _gameOptionsSave;
        private PanelGameOptionManager _panelGameOptionManager;

        private void Awake()
        {
            _languageButton = GetComponent<Button>();
            _panelGameOptionManager = GetComponentInParent<PanelGameOptionManager>();
        }

        private void OnEnable()
        {
            _gameOptionsSave = GameOptionsSave.Instance;
            AddButtonOnClick();
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

        private void OnDestroy()
        {
            LocalizationSettings.SelectedLocaleChanged -= ChangeNormalColor;
        }

        private void AddButtonOnClick()
        {
            _languageButton.onClick.RemoveAllListeners();
            _languageButton.onClick.AddListener(ButtonChangeLocale);
        }

        private void ChangeNormalColor(Locale locale)
        {
            var buttonColors = _languageButton.colors;

            buttonColors.normalColor = (LocalizationSettings.SelectedLocale != myLocale)
                ? _panelGameOptionManager.disabledButtonColor
                : _panelGameOptionManager.normalButtonColor; 
            _languageButton.colors = buttonColors;
        }

        private void ButtonChangeLocale()
        {
            if (LocalizationSettings.SelectedLocale == myLocale)
                return;

            AudioManager.instance.PlayMouseClick();
            StartCoroutine(ChangeLocale(myLocale));
        }

        private IEnumerator ChangeLocale(Locale locale)
        {
            yield return LocalizationSettings.InitializationOperation;

            LocalizationSettings.SelectedLocale = _gameOptionsSave.startLocale = locale;

            DebugManager.Log<PanelLanguagesOptions>($"Locale changed to: {locale.name}");
        }
    }
}