using ImmersiveGames.AudioEvents;
using ImmersiveGames.DebugManagers;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;
using UnityEngine.UI;

namespace NewRiverAttack.HUDManagers.UI
{
    public class UIFuelDisplay : MonoBehaviour
    {
        public int playerIndex = 0;
        [SerializeField] private AudioEvent playerAlert;
        [SerializeField] private Image gasBarImage;
        [SerializeField] private Color highGasColor;
        [SerializeField] private Color mediumGasColor;
        [SerializeField] private Color lowGasColor;

        private const float MediumGasValue = 0.5f;
        private const float LowGasValue = 0.25f;
        
        public float speedSmooth = 5f; // Quanto maior mais rápido atualiza
        private float _valorMeta;

        private PlayerMaster _playerMaster;
        
        private AudioSource _audioSource;
        private PlayerFuel _playerFuel;
        private GamePlayManager _gamePlayManager;

        #region UNITYMETHODS

        private void OnEnable()
        {
            SetInitialReferences();
        }
        private void Start()
        {
            _playerMaster = _gamePlayManager.GetPlayerMaster(playerIndex);
            _playerFuel = _playerMaster.GetComponent<PlayerFuel>();
            _valorMeta = _playerFuel.GetFuel / _playerFuel.GetMaxFuel;
            _playerMaster.EventPlayerMasterGetHit += StopSound;
        }
        private void Update()
        {
            UpdateDisplay();
        }

        #endregion

        private void SetInitialReferences()
        {
            _gamePlayManager = GamePlayManager.instance;
            _audioSource = GetComponent<AudioSource>();
        }
        private void UpdateFuel(float valueUpdate, int iPlayerIndex)
        {
            if(playerIndex != iPlayerIndex) return;
            _playerMaster = _gamePlayManager.GetPlayerMaster(playerIndex);
            
            //TODO: Aumentar a gasolina ou reduzir
        }
        private void StopSound()
        {
            playerAlert.Stop(_audioSource);
        }

        private void UpdateDisplay()
        {
            if (!_playerFuel) return;
            DebugManager.Log<UIFuelDisplay>($"Fuel: {_playerFuel.GetFuel}");
            
            // Calcula a proporção do combustível restante em relação ao máximo
            var portion = _playerFuel.GetFuel / _playerFuel.GetMaxFuel;

            // Define o valor meta suavizado com base na proporção do combustível
            _valorMeta = Mathf.Lerp(_valorMeta, portion, Time.deltaTime * speedSmooth);

            // Define a escala do medidor com base no valor meta suavizado
            gasBarImage.fillAmount = _valorMeta;
            
            gasBarImage.color = _valorMeta switch
            {
                < MediumGasValue and > LowGasValue => mediumGasColor,
                <= LowGasValue => lowGasColor,
                _ => highGasColor
            };

            if (_valorMeta <= LowGasValue && !_audioSource.isPlaying)
            {
                playerAlert.SimplePlay(_audioSource);
            }
            if (_audioSource.isPlaying && _valorMeta > LowGasValue)
            {
                playerAlert.Stop(_audioSource);
            }
        }
    }
}
