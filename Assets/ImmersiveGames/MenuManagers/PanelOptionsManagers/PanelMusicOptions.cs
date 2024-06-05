using ImmersiveGames.Utils;
using NewRiverAttack.SaveManagers;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace ImmersiveGames.MenuManagers.PanelOptionsManagers
{
    public class PanelMusicOptions : MonoBehaviour
    {
        [SerializeField] private EnumAudioMixGroup audioTypeMixGroup;
        [SerializeField] private AudioMixer mixerGroup;

        private Slider _sliderControl;
        private GameOptionsSave _gameOptionsSave;

        private void Awake()
        {
            // Obtém a referência do Slider na hierarquia de filhos.
            _sliderControl = GetComponentInChildren<Slider>();
        }
        

        private void OnEnable()
        {
            // Carrega as opções do jogo ao ativar o objeto.
            LoadGameOptions();
            _sliderControl.onValueChanged.AddListener(SetVolume);
        }

        private void OnDisable()
        {
            // Salva as opções do jogo ao desativar o objeto.
            SaveGameOptions();
            _sliderControl.onValueChanged.RemoveAllListeners();
        }

        public void SetVolume(float slideValue)
        {
            // Calcula o volume em decibéis com base no valor do Slider.
            var volume = AudioUtils.SoundBase10(slideValue);

            // Ajusta o volume no AudioMixer.
            mixerGroup.SetFloat(audioTypeMixGroup.ToString(), volume);

            // Salva as opções do jogo com base no tipo de volume.
            SaveGameOptions();
        }

        private void LoadGameOptions()
        {
            // Obtém a instância de GameOptionsSave.
            _gameOptionsSave = GameOptionsSave.instance;

            // Define o valor do Slider com base nas opções salvas.
            _sliderControl.value = _gameOptionsSave.GetVolume(audioTypeMixGroup);
        }

        private void SaveGameOptions()
        {
            // Verifica se a instância de GameOptionsSave não é nula.
            if (_gameOptionsSave != null)
            {
                // Salva o volume com base no tipo de volume.
                _gameOptionsSave.SetVolume(audioTypeMixGroup, _sliderControl.value);
            }
        }
    }
}
