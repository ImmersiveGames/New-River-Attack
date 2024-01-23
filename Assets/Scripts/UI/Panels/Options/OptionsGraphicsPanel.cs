using TMPro;
using UnityEngine;
using UnityEngine.Localization;
namespace RiverAttack
{
    public class OptionsGraphicsPanel : OptionsDropDown
    {
        int m_ActualQuality;
        /*void Start()
        {
            SetDropdown(actualLocal);
            UpdateQuality(gameSettings.indexQuality);
        }
        protected override void SetDropdown(Locale newLocale)
        {
            base.SetDropdown(newLocale);
            graphicsDropdown.value = m_ActualQuality;

            graphicsDropdown.onValueChanged.AddListener(delegate
            {
                OnQualityChanged(graphicsDropdown);
            });
        }
        void OnQualityChanged(TMP_Dropdown dropdown)
        {
            // Obter o valor selecionado e aplicar a qualidade gráfica.
            gameSettings.indexQuality = m_ActualQuality = dropdown.value;
            UpdateQuality(dropdown.value);

        }
        static void UpdateQuality(int indexQuality)
        {
            QualitySettings.SetQualityLevel(indexQuality);
            Debug.Log("Apliquei as qualidade grafica: " + QualitySettings.GetQualityLevel());
        }*/
    }
}
