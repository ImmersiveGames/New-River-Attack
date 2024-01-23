using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
namespace RiverAttack
{
    public class OptionsFrameRatePanel : OptionsDropDown
    {
        internal static uint actualFrameRate;

        /*protected override void OnEnable()
        {
            base.OnEnable();
            graphicsDropdown.value = gameSettings.indexFrameRate;
        }

        protected override void SetDropdown(Locale newLocale)
        {
            base.SetDropdown(newLocale);
            graphicsDropdown.value = gameSettings.indexFrameRate;
            graphicsDropdown.onValueChanged.AddListener(delegate
            {
                OnFramerateChanged(graphicsDropdown);
            });
        }
        void OnFramerateChanged(TMP_Dropdown dropdown)
        {
            gameSettings.indexFrameRate = dropdown.value;
            UpdateFrameRate(dropdown.value);
        }

        static void UpdateFrameRate(int valueIndex)
        {
            int frameRate = valueIndex == 0 ? 60 : 30;
            actualFrameRate = (uint)frameRate;
            Debug.Log(frameRate);
            Application.targetFrameRate = frameRate;
        }*/
    }
}
