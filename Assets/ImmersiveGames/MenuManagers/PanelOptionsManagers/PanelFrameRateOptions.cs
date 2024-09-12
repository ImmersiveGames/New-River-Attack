﻿using System.Collections.Generic;
using ImmersiveGames.DebugManagers;
using NewRiverAttack.SaveManagers;
using TMPro;
using UnityEngine;

namespace ImmersiveGames.MenuManagers.PanelOptionsManagers
{
    public class PanelFrameRateOptions : MonoBehaviour
    {
        // Propriedade para acessar actualFrameRate de maneira controlada.
        public static int ActualFrameRate { get; private set; }

        [SerializeField] private TMP_Dropdown dropdown;

        [SerializeField] private List<int> frameRates = new List<int> { 30, 60, 90, 120 }; // Lista de taxas de quadros desejadas.

        // Método chamado ao despertar o objeto.
        private void Awake()
        {
            FillDropdown();
            UpdateFrameRate(GameOptionsSave.Instance.frameRate); // Inicializar com a taxa de quadros salva.
        }

        // Método chamado quando o valor do dropdown é alterado.
        private void OnDropdownChanged(int value)
        {
            UpdateFrameRate(frameRates[value]);
            DebugManager.Log<PanelFrameRateOptions>($"Taxa de quadros alterada para: {ActualFrameRate}");
        }

        // Método para preencher o dropdown com a lista de taxas de quadros.
        private void FillDropdown()
        {
            if (dropdown == null) return;
            dropdown.ClearOptions();

            foreach (var frameRate in frameRates)
            {
                dropdown.options.Add(new TMP_Dropdown.OptionData(frameRate.ToString()));
            }

            dropdown.onValueChanged.AddListener(OnDropdownChanged);
            dropdown.value = frameRates.IndexOf(GameOptionsSave.Instance.frameRate); // Definir o valor inicial.
        }

        // Método para atualizar a taxa de quadros com base no valor do dropdown.
        private void UpdateFrameRate(int frameRate)
        {
            ActualFrameRate = frameRate;
            Application.targetFrameRate = frameRate;
            GameOptionsSave.Instance.frameRate = frameRate; // Salvar a taxa de quadros.
        }

        // Método chamado ao habilitar o objeto.
        private void OnEnable()
        {
            if (dropdown != null)
            {
                dropdown.value = frameRates.IndexOf(GameOptionsSave.Instance.frameRate); // Atualizar o valor inicial.
            }
        }

        // Método chamado ao desabilitar o objeto.
        private void OnDisable()
        {
            if (dropdown != null)
                dropdown.onValueChanged.RemoveListener(OnDropdownChanged);
        }
    }
}
