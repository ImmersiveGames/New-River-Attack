using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace RiverAttack
{
    public class OptionsFrameRatePanel : OptionsDropDown
    {
        internal static uint actualFrameRate;

        protected override void Awake()
        {
            base.Awake();
            selectedOptionIndex = gameSettings.indexFrameRate;
            //Debug.Log($"Awake: {actualFrameRate}");
        }

        protected override void OnDropdownChanged(TMP_Dropdown tmpDropdown)
        {
            base.OnDropdownChanged(tmpDropdown);
            gameSettings.indexFrameRate = selectedOptionIndex;
            UpdateFrameRate(selectedOptionIndex);
        }

        internal static int FrameRate(int indexFrame)
        {
            return indexFrame == 0 ? 60 : 30;
        }

        internal static void UpdateFrameRate(int indexFrame)
        {
            int frameRate = FrameRate(indexFrame);
            actualFrameRate = (uint)frameRate;
            Application.targetFrameRate = frameRate;
        }
    }
}
