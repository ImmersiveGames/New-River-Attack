using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
namespace RiverAttack
{
    public class OptionsResolutionPanel : OptionsDropDown
    {
        static Resolution _actualResolution;
        public const FullScreenMode FullScreenMode = UnityEngine.FullScreenMode.FullScreenWindow;
        static Resolution[] _resolutions;
        
        protected override void Awake()
        {
            base.Awake();
            _resolutions ??= GetResolutions();
            _actualResolution = Screen.currentResolution;
            selectedOptionIndex = Array.IndexOf(_resolutions, _actualResolution);
        }

        static Resolution[] GetResolutions()
        {
            var allResolution = Screen.resolutions;
            return RemoveDuplicateResolutions(allResolution);
        }
        protected override void UpdateDropdown(Locale locale)
        {
            dropdown.ClearOptions();
            
            foreach (string txtResolution in _resolutions.Select(resolution => resolution.width + " x " + resolution.height))
            {
                //Debug.Log($"Resolutions: {txtResolution}");
                dropdown.options.Add(new TMP_Dropdown.OptionData(txtResolution, null));
            }
            dropdown.value = selectedOptionIndex;
            dropdown.RefreshShownValue();
            dropdown.onValueChanged.AddListener(delegate
            {
                OnDropdownChanged(dropdown);
            });
        }
        protected override void OnDropdownChanged(TMP_Dropdown tmpDropdown)
        {
            base.OnDropdownChanged(tmpDropdown);
            gameSettings.indexResolution = selectedOptionIndex;
            UpdateResolution(selectedOptionIndex);
        }
        static void UpdateResolution(int indexFrame)
        {
            var resolution = _resolutions[indexFrame];
            _actualResolution = resolution;
            var dimension = new Vector2Int(resolution.width, resolution.height);
            SetResolution(dimension, FullScreenMode, OptionsFrameRatePanel.actualFrameRate);
            GameSettings.instance.actualResolution = dimension;
        }
        static void SetResolution(Vector2Int dimension, FullScreenMode fullScreenMode, uint fps)
        {
            var selectedFramerate = GetRefreshRate(fps, 1);
            Screen.SetResolution(dimension.x, dimension.y, fullScreenMode, selectedFramerate);
        }

        internal static RefreshRate GetRefreshRate(uint fps, uint denominator)
        {
            RefreshRate selectedFramerate;
            selectedFramerate.numerator = fps;
            selectedFramerate.denominator = denominator;
            return selectedFramerate;
        }

        static Resolution[] RemoveDuplicateResolutions(IReadOnlyList<Resolution> resolutionsList)
        {
            var uniqueResolutions = new HashSet<string>();
            var uniqueList = new List<Resolution>();

            for (int i = resolutionsList.Count - 1; i >= 0; i--)
            {
                string resolutionString = resolutionsList[i].width + "x" + resolutionsList[i].height;

                if (uniqueResolutions.Add(resolutionString))
                {
                    uniqueList.Add(resolutionsList[i]);
                }
            }
            
            return uniqueList.ToArray();
        }
    }
}
