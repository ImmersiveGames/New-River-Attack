using System;
using System.Collections.Generic;
using System.Linq;
using ImmersiveGames.DebugManagers;
using NewRiverAttack.SaveManagers;
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
            // Popula o array de resoluções únicas
            _uniqueResolutions = GetUniqueResolutions(Screen.resolutions, PanelFrameRateOptions.ActualFrameRate);
            if (_uniqueResolutions == null || _uniqueResolutions.Length == 0)
            {
                DebugManager.LogError<PanelResolutionOptions>("Nenhuma resolução válida disponível!");
                return;
            }

            // Verifica e define a resolução inicial com base no GameOptionsSave
            _actualResolution = GetResolutionFromOptionsSave();

            // Preenche o dropdown com as resoluções
            var resolutionOptions = _uniqueResolutions
                .Select(resolution => resolution.width + " x " + resolution.height)
                .ToList();
            InitializeDropdown(resolutionOptions);

            // Define o valor selecionado no dropdown
            var selectedResolutionIndex = Array.IndexOf(_uniqueResolutions, _actualResolution);
            SetDropdownValue(selectedResolutionIndex >= 0 ? selectedResolutionIndex : 0);

            // Salva a resolução atual (mesmo que não tenha sido alterada)
            SaveResolutionToOptions();

            DebugManager.Log<PanelResolutionOptions>($"Resolução atual do sistema: {Screen.currentResolution.width} x {Screen.currentResolution.height}");
            DebugManager.Log<PanelResolutionOptions>($"Resolução salva: {_actualResolution.width} x {_actualResolution.height}");
        }

        private void InitializeDropdown(List<string> options)
        {
            resolutionDropdown.ClearOptions();
            resolutionDropdown.AddOptions(options);
            resolutionDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }

        private void SetDropdownValue(int value)
        {
            if (value >= 0 && value < _uniqueResolutions.Length)
            {
                resolutionDropdown.value = value;
                OnValueChanged?.Invoke(value);
            }
            else
            {
                DebugManager.LogError<PanelResolutionOptions>("Valor do dropdown fora dos limites.");
            }
        }

        private void OnDropdownValueChanged(int value)
        {
            UpdateResolution(value);
        }

        private void UpdateResolution(int indexFrame)
        {
            if (indexFrame >= 0 && indexFrame < _uniqueResolutions.Length)
            {
                var resolution = _uniqueResolutions[indexFrame];
                _actualResolution = resolution;
                
                // Aplica a resolução e salva em GameOptionsSave.Instance.actualResolution
                SetResolution(new Vector2Int(resolution.width, resolution.height), FullScreenMode);
                SaveResolutionToOptions();

                DebugManager.Log<PanelResolutionOptions>($"Resolução atualizada: {resolution.width} x {resolution.height}");
            }
            else
            {
                DebugManager.LogError<PanelResolutionOptions>("Índice de resolução fora dos limites.");
            }
        }

        private static void SetResolution(Vector2Int dimension, FullScreenMode fullScreenMode)
        {
            Screen.SetResolution(dimension.x, dimension.y, fullScreenMode);
        }

        private static Resolution[] GetUniqueResolutions(Resolution[] resolutionsList, int targetFps)
        {
            var uniqueResolutions = resolutionsList
                .Where(resolution => Mathf.Approximately(targetFps, (float)resolution.refreshRateRatio.value))
                .Distinct()
                .ToArray();

            return uniqueResolutions.Length > 0 ? uniqueResolutions : resolutionsList.Distinct().ToArray();
        }

        private Resolution GetResolutionFromOptionsSave()
        {
            if (GameOptionsSave.Instance == null)
            {
                DebugManager.LogWarning<PanelResolutionOptions>("GameOptionsSave.Instance é nulo. Usando resolução atual da tela.");
                return Screen.currentResolution;
            }

            var savedResolution = GameOptionsSave.Instance.actualResolution;

            // Se a resolução salva for (0, 0), define a resolução atual
            if (savedResolution == Vector2Int.zero)
            {
                var currentResolution = new Vector2Int(Screen.currentResolution.width, Screen.currentResolution.height);
                GameOptionsSave.Instance.actualResolution = currentResolution;
                DebugManager.Log<PanelResolutionOptions>($"Resolução salva como {currentResolution.x} x {currentResolution.y}");
                return Screen.currentResolution;
            }

            // Procura uma resolução que corresponda ao valor salvo
            var matchingResolution = _uniqueResolutions.FirstOrDefault(resolution =>
                resolution.width == savedResolution.x && resolution.height == savedResolution.y);

            if (!matchingResolution.Equals(default(Resolution)))
            {
                return matchingResolution;
            }

            DebugManager.LogWarning<PanelResolutionOptions>("Resolução salva não encontrada. Usando a primeira resolução disponível.");
            return _uniqueResolutions[0];
        }

        private static void SaveResolutionToOptions()
        {
            if (GameOptionsSave.Instance != null)
            {
                // Sempre salva a resolução atual em GameOptionsSave.Instance.actualResolution
                GameOptionsSave.Instance.actualResolution = new Vector2Int(_actualResolution.width, _actualResolution.height);
                DebugManager.Log<PanelResolutionOptions>($"Resolução salva: {_actualResolution.width} x {_actualResolution.height}");
            }
        }

        public Action<int> OnValueChanged { get; set; }
    }
}
