using TMPro;
using UnityEngine;
namespace RiverAttack
{
    public class OptionsGraphicsPanel : OptionsDropDown
    {
        private int m_ActualQuality;
        protected override void Awake()
        {
            base.Awake();
            selectedOptionIndex = gameSettings.indexQuality;
            Debug.Log($"Awake: {m_ActualQuality}");
        }
        protected override void OnDropdownChanged(TMP_Dropdown tmpDropdown)
        {
            base.OnDropdownChanged(tmpDropdown);
            gameSettings.indexQuality = selectedOptionIndex;
            UpdateQuality(selectedOptionIndex);
        }
        internal static void UpdateQuality(int indexQuality)
        {
            QualitySettings.SetQualityLevel(indexQuality);
            Debug.Log("Apliquei as qualidade grafica: " + QualitySettings.GetQualityLevel());
        }
    }
}
