using ImmersiveGames.DebugManagers;
using ImmersiveGames.ShopManagers.ShopProducts;
using NewRiverAttack.ObstaclesSystems;
using NewRiverAttack.PlayerManagers.ScriptableObjects;
using UnityEngine;

namespace NewRiverAttack.PlayerManagers.PlayerSystems
{
    public class PlayerFuel : MonoBehaviour
    {
        private float _fuelMax;
        private float _fuel;
        private float _spendFuel;
        [SerializeField] private float reduceFuelCadence = 1f; //Quanto maior mais rápido
        [Header("Power Up Gas Station")]
        [SerializeField] private float fillUpFuelCadence = 6f;
        private bool _fuelPause;
        private bool _inPowerUp;
        
        private PlayerMaster _playerMaster;
        private PlayerAchievements _playerAchievements;

        #region Unity Methods

        private void OnEnable()
        {
            // 1 - ORDEM DE INICIO
            SetInitialReferences();
            _playerMaster.EventPlayerMasterInitialize += InitializeFuel;
            _playerMaster.EventPlayerMasterChangeSkin += InitializeSkinFuel;
            _playerMaster.EventPlayerMasterRespawn += RestoreFuel;
            _playerMaster.EventPlayerMasterAreaEffectStart += StartFillUp;
            _playerMaster.EventPlayerMasterAreaEffectEnd += EndFillEnd;
        }
        void Update()
        {
            if (_inPowerUp)
            {
                var lastFillFuel = fillUpFuelCadence * Time.deltaTime;
                _fuel += lastFillFuel;
                _fuel = Mathf.Clamp(_fuel, 0f, _fuelMax);
            }
            if (_fuelPause || !_playerMaster.ObjectIsReady || _playerMaster.godMode || _playerMaster.inFinishPath) return;
            // Decrementa o combustível com base na taxa de decaimento por segundo
            var lastFuel = reduceFuelCadence * Time.deltaTime;
            _fuel -= lastFuel;
            _spendFuel += lastFuel;
            if (_spendFuel % _fuelMax == 0)
            {
                _playerAchievements.LogSpendFuel(_spendFuel);
                _spendFuel = 0;
            }

            // Garante que o combustível não ultrapasse o máximo
            _fuel = Mathf.Clamp(_fuel, 0f, _fuelMax);
            
            if (!(_fuel <= 0)) return;
            _playerMaster.OnEventPlayerMasterGetHit();
            _playerAchievements.LogOutFuel();
        }
        private void OnDisable()
        {
            _playerAchievements.LogSpendFuel(_spendFuel);
            _playerMaster.EventPlayerMasterInitialize -= InitializeFuel;
            _playerMaster.EventPlayerMasterChangeSkin -= InitializeSkinFuel;
            _playerMaster.EventPlayerMasterRespawn -= RestoreFuel;
            _playerMaster.EventPlayerMasterAreaEffectStart -= StartFillUp;
            _playerMaster.EventPlayerMasterAreaEffectEnd -= EndFillEnd;
        }

        #endregion

        private void SetInitialReferences()
        {
            _playerMaster = GetComponent<PlayerMaster>();
            _playerAchievements = GetComponent<PlayerAchievements>();
        }
        private void FuelUp(float quantidade)
        {
            // Adiciona a quantidade especificada ao combustível atual
            _fuel += quantidade;

            // Garante que o combustível não ultrapasse o máximo
            _fuel = Mathf.Clamp(_fuel, 0f, _fuelMax);
        }
        private void PauseDecoy(bool pausar)
        {
            // Define o estado de pausa do decaimento
            _fuelPause = pausar;
        }
        private void UpdateDecoyParam(float newFuelMax, float newCadenceFuel)
        {
            // Atualiza os valores máximos e decaimento com base nos parâmetros fornecidos
            _fuelMax = newFuelMax;
            reduceFuelCadence = newCadenceFuel;

            // Garante que o combustível não ultrapasse o novo máximo
            reduceFuelCadence = Mathf.Clamp(reduceFuelCadence, 0f, newFuelMax);
        }

        private void RestoreFuel()
        {
            _fuel = _fuelMax;
            _playerAchievements.LogSpendFuel(_spendFuel);
            _spendFuel = 0;
        }

        public float GetFuel => _fuel;
        public float GetMaxFuel => _fuelMax;
        private void InitializeFuel(int indexPlayer, PlayersDefaultSettings defaultSettings)
        {
            // 2- ORDEM DE INICIO
            if (_playerMaster.PlayerIndex != indexPlayer) return;
            var skin = _playerMaster.ActualSkin;
            _fuelMax = skin.maxFuel != 0 ? skin.maxFuel : defaultSettings.maxFuel;
            _fuel = _fuelMax;
            DebugManager.Log<PlayerFuel>($"No Initialize {_playerMaster.ActualSkin.name}");
        }
        private void InitializeSkinFuel(ShopProductSkin shopProductSkin)
        {
            // 3 - ORDEM DE INICIO
            if(shopProductSkin.maxFuel > 0)
                _fuelMax = shopProductSkin.maxFuel;
            if(shopProductSkin.cadenceFuel > 0)
                reduceFuelCadence = shopProductSkin.cadenceFuel;
            DebugManager.Log<PlayerFuel>($"Na Mudança de skin");
        }

        #region PowerUp GasStatioin

        private void EndFillEnd(ObstacleTypes areaEffect)
        {
            if (areaEffect != ObstacleTypes.GasStation) return;
            _inPowerUp = false;
            PauseDecoy(_inPowerUp);
        }

        private void StartFillUp(ObstacleTypes areaEffect)
        {
            if (areaEffect != ObstacleTypes.GasStation) return;
            _inPowerUp = true;
            PauseDecoy(_inPowerUp);
        }

        #endregion
    }
}