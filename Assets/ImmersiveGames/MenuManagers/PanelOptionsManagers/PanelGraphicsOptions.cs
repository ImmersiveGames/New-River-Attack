using ImmersiveGames.DebugManagers;
using ImmersiveGames.MenuManagers.UI;
using NewRiverAttack.SaveManagers;
using UnityEngine;

namespace ImmersiveGames.MenuManagers.PanelOptionsManagers
{
    public class PanelGraphicsOptions : UIDropDown
    {
        protected override void Awake()
        {
            base.Awake();
            LoadSavedQuality();
        }

        protected override void OnDropdownChanged(int value)
        {
            base.OnDropdownChanged(value);
            SaveSelectedQuality(value);
            UpdateQuality(value);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            SaveSelectedQuality(SelectedOptionIndex); // Salvar o índice da qualidade ao desativar
        }

        private void LoadSavedQuality()
        {
            SelectedOptionIndex = GameOptionsSave.instance.selectedQualityIndex;
        }

        private static void SaveSelectedQuality(int selectedQualityIndex)
        {
            GameOptionsSave.instance.selectedQualityIndex = selectedQualityIndex;
        }

        private static void UpdateQuality(int selectedQualityIndex)
        {
            QualitySettings.SetQualityLevel(selectedQualityIndex);
            DebugManager.Log<PanelGraphicsOptions>("Apliquei a qualidade gráfica: " + selectedQualityIndex);
        }
    }
}