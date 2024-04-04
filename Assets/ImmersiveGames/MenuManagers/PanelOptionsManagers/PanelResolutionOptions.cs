using System;
using System.Collections.Generic;
using System.Linq;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.SaveManagers;
using TMPro;
using UnityEngine;

namespace ImmersiveGames.MenuManagers.PanelOptionsManagers
{
    public class PanelResolutionOptions : MonoBehaviour
    {
        private static Resolution _actualResolution;
        private const FullScreenMode FullScreenMode = UnityEngine.FullScreenMode.FullScreenWindow;

        private Resolution[] _uniqueResolutions;

        [SerializeField] private TMP_Dropdown resolutionDropdown;

        private void Awake()
        {
            _uniqueResolutions = GetUniqueResolutions(Screen.resolutions, PanelFrameRateOptions.ActualFrameRate);
            _actualResolution = GetResolutionFromOptionsSave();

            // Obtém as opções do dropdown
            var resolutionOptions = _uniqueResolutions.Select(resolution => resolution.width + " x " + resolution.height).ToList();
    
            // Inicializa o dropdown
            InitializeDropdown(resolutionOptions);

            // Define o valor selecionado do dropdown
            // Se a resolução salva for zero ou nula, selecione a resolução atual
            SetDropdownValue(_actualResolution is { width: 0, height: 0 }
                ? Array.IndexOf(_uniqueResolutions, Screen.currentResolution)
                // Caso contrário, selecione a resolução salva
                : Array.IndexOf(_uniqueResolutions, _actualResolution));

            // Depuração para verificar resolução atual e resolução salva
            DebugManager.Log<PanelResolutionOptions>("Resolução atual do sistema: " + Screen.currentResolution.width + " x " + Screen.currentResolution.height);
            DebugManager.Log<PanelResolutionOptions>("Resolução salva: " + _actualResolution.width + " x " + _actualResolution.height);
        }

        private void InitializeDropdown(List<string> options)
        {
            resolutionDropdown.ClearOptions();
            resolutionDropdown.AddOptions(options);
            resolutionDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }

        private void SetDropdownValue(int value)
        {
            resolutionDropdown.value = value;
            OnValueChanged?.Invoke(value);
        }

        private void OnDropdownValueChanged(int value)
        {
            UpdateResolution(value);
        }

        private void UpdateResolution(int indexFrame)
        {
            var resolution = _uniqueResolutions[indexFrame];
            _actualResolution = resolution;
            SetResolution(new Vector2Int(resolution.width, resolution.height), FullScreenMode);
            SaveResolutionToOptions();

            // Depuração para verificar a resolução atual após a atualização
            DebugManager.Log<PanelResolutionOptions>("Resolução atualizada: " + resolution.width + " x " + resolution.height);
        }

        private static void SetResolution(Vector2Int dimension, FullScreenMode fullScreenMode)
        {
            Screen.SetResolution(dimension.x, dimension.y, fullScreenMode);
        }

        private static Resolution[] GetUniqueResolutions(IEnumerable<Resolution> resolutionsList, int targetFps)
        {
            return resolutionsList
                .Where(resolution => Mathf.Approximately(targetFps, (float)resolution.refreshRateRatio.value))
                .Distinct()
                .ToArray();
        }

        private Resolution GetResolutionFromOptionsSave()
        {
            // Se GameOptionsSave.instance for nulo, retorna a primeira resolução única
            if (GameOptionsSave.instance == null)
            {
                return _uniqueResolutions[0];
            }

            // Recupera a resolução salva
            var savedResolution = GameOptionsSave.instance.actualResolution;

            // Procura a resolução correspondente
            var matchingResolution = _uniqueResolutions.FirstOrDefault(resolution =>
                resolution.width == savedResolution.x && resolution.height == savedResolution.y);

            // Retorna a resolução correspondente se existir, caso contrário, retorna a resolução padrão
            return !matchingResolution.Equals(default(Resolution)) ? matchingResolution : _uniqueResolutions[0];
        }
        private static void SaveResolutionToOptions()
        {
            if (GameOptionsSave.instance != null)
            {
                GameOptionsSave.instance.actualResolution = new Vector2Int(_actualResolution.width, _actualResolution.height);
            }
        }

        // Evento para notificar mudanças na resolução selecionada
        public Action<int> OnValueChanged { get; set; }
    }
}
