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
        /*protected override void OnEnable()
        {
            base.OnEnable();
            if (gameSettings.actualResolution == Vector2Int.zero)
                return;
            _actualResolution.width = gameSettings.actualResolution.x;
            _actualResolution.height = gameSettings.actualResolution.y;
            SetResolution(gameSettings.actualResolution, gameSettings.actualFullScreenMode, (uint)gameSettings.indexFrameRate);
        }
        protected override void SetDropdown(Locale newLocale)
        {
            var allResolutions = Screen.resolutions;
            // Remover duplicatas do array de resoluções.
            var resolutions = RemoveDuplicateResolutions(allResolutions);

            graphicsDropdown.ClearOptions();
            
            var tmpDropdownOptions = resolutions.Select(resolution => resolution.width + " x " + resolution.height).Select(optionText => new TMP_Dropdown.OptionData(optionText)).ToList();
            // Adicionar cada opção de resolução à lista em ordem inversa, evitando duplicatas.

            graphicsDropdown.ClearOptions();
            
            graphicsDropdown.onValueChanged.AddListener(delegate
            {
                OnResolutionChanged(graphicsDropdown, resolutions);
            });

            SetInitialResolutionValue(graphicsDropdown, resolutions);
        }
        
        static Resolution[] RemoveDuplicateResolutions(IReadOnlyList<Resolution> resolutions)
        {
            var uniqueResolutions = new HashSet<string>();
            var uniqueList = new List<Resolution>();

            for (int i = resolutions.Count - 1; i >= 0; i--)
            {
                string resolutionString = resolutions[i].width + "x" + resolutions[i].height;

                if (uniqueResolutions.Add(resolutionString))
                {
                    uniqueList.Add(resolutions[i]);
                }
            }
            
            return uniqueList.ToArray();
        }
        static void SetInitialResolutionValue(TMP_Dropdown dropdown, IReadOnlyList<Resolution> resolutions)
        {
            var currentResolution = _actualResolution = Screen.currentResolution;

            for (int i = 0; i < resolutions.Count; i++)
            {
                if (currentResolution.width != resolutions[i].width ||
                    currentResolution.height != resolutions[i].height)
                    continue;
                dropdown.value = i;
                break;
            }
        }
        void OnResolutionChanged(TMP_Dropdown dropdown, IReadOnlyList<Resolution> resolutions)
        {
            // Obter o valor selecionado e aplicar a resolução.
            _actualResolution.width = resolutions[dropdown.value].width;
            _actualResolution.height = resolutions[dropdown.value].height;

            gameSettings.actualResolution.x = _actualResolution.width;
            gameSettings.actualResolution.y = _actualResolution.height;

            SetResolution(gameSettings.actualResolution, FullScreenMode.FullScreenWindow, OptionsFrameRatePanel.actualFrameRate);

            Debug.Log("Actual Resolution: " + _actualResolution);
        }

        static void SetResolution(Vector2Int dimension, FullScreenMode fullScreenMode, uint fps)
        {
            RefreshRate selectedFramerate;
            const uint denominator = 1;
            selectedFramerate.numerator = fps;
            selectedFramerate.denominator = denominator;
            Screen.SetResolution(dimension.x, dimension.y, fullScreenMode, selectedFramerate);
        }*/
    }
}
