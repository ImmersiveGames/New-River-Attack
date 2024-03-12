using System.Collections;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.SaveManagers;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace ImmersiveGames.MenuManagers.GameOptionsPanels
{
    public class PanelLanguagesOptions: MonoBehaviour
    {
        [SerializeField] private Locale myLocale;
        private Button _languageButton;
        private GameOptionsSave _gameOptionsSave;
        private enum ButtonState { Active, Inactive }
        private ButtonState _buttonState;

        private void Awake()
        {
            _languageButton = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _gameOptionsSave = GameOptionsSave.instance;
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
            _buttonState = (LocalizationSettings.SelectedLocale != myLocale) ? ButtonState.Inactive : ButtonState.Active;
            buttonColors.normalColor = (_buttonState == ButtonState.Active) ? GameOptionsManager.instance.normalButtonColor : GameOptionsManager.instance.disabledButtonColor;
            _languageButton.colors = buttonColors;
        }

        private void ButtonChangeLocale()
        {
            if (_buttonState == ButtonState.Active)
                return;

            AudioManager.PlayMouseClick();
            StartCoroutine(ChangeLocale(myLocale));
        }

        private IEnumerator ChangeLocale(Locale locale)
        {
            _buttonState = ButtonState.Active;

            yield return LocalizationSettings.InitializationOperation;

            LocalizationSettings.SelectedLocale = _gameOptionsSave.startLocale = locale;
            _buttonState = ButtonState.Inactive;

            DebugManager.Log($"Locale changed to: {locale.name}");
        }
    }
}