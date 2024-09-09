using ImmersiveGames.DebugManagers;
using ImmersiveGames.ShopManagers.ShopProducts;
using NewRiverAttack.GameStatisticsSystem;
using NewRiverAttack.ObstaclesSystems;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;
using NewRiverAttack.PlayerManagers.ScriptableObjects;
using UnityEngine;

namespace NewRiverAttack.PlayerManagers.PlayerSystems
{
    public class PlayerFuel : MonoBehaviour
    {

        [SerializeField] private float reduceFuelCadence = 1f; //Quanto maior mais rápido
        
        private bool _fuelPause;
        private bool _inPowerUp;
        
        private PlayerMaster _playerMaster;
        private float _areaEffectCadence;

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
            _playerMaster.EventPlayerMasterStopDecoyFuel += PauseDecoy;
        }

        private void Update()
        {
            if (GetFuel <= 0) return;
            UpdateFuelState();
        }

        private void OnDisable()
        {
            _playerMaster.EventPlayerMasterInitialize -= InitializeFuel;
            _playerMaster.EventPlayerMasterChangeSkin -= InitializeSkinFuel;
            _playerMaster.EventPlayerMasterRespawn -= RestoreFuel;
            _playerMaster.EventPlayerMasterAreaEffectStart -= StartFillUp;
            _playerMaster.EventPlayerMasterAreaEffectEnd -= EndFillEnd;
            _playerMaster.EventPlayerMasterStopDecoyFuel -= PauseDecoy;
        }

        #endregion

        private void SetInitialReferences()
        {
            _playerMaster = GetComponent<PlayerMaster>();
        }

        private void PauseDecoy(bool pausar)
        {
            // Define o estado de pausa do decaimento
            _fuelPause = pausar;
        }

        private void RestoreFuel()
        {
            DebugManager.Log<PlayerFuel>("Restore Fuel");
            GetFuel = GetMaxFuel;
        }

        public float GetFuel { get; private set; }

        public float GetMaxFuel { get; private set; }

        private void InitializeFuel(int indexPlayer, PlayersDefaultSettings defaultSettings)
        {
            // 2- ORDEM DE INICIO
            if (_playerMaster.PlayerIndex != indexPlayer) return;
            var skin = _playerMaster.ActualSkin;
            GetMaxFuel = skin.maxFuel != 0 ? skin.maxFuel : defaultSettings.maxFuel;
            GetFuel = GetMaxFuel;
            DebugManager.Log<PlayerFuel>($"Initialize {_playerMaster.ActualSkin.name}");
        }

        private void InitializeSkinFuel(ShopProductSkin shopProductSkin)
        {
            // 3 - ORDEM DE INICIO
            if (shopProductSkin.maxFuel > 0)
                GetMaxFuel = shopProductSkin.maxFuel;
            if (shopProductSkin.cadenceFuel > 0)
                reduceFuelCadence = shopProductSkin.cadenceFuel;
            DebugManager.Log<PlayerFuel>($"Na Mudança de skin");
        }

        #region PowerUp GasStation

        private void EndFillEnd(AreaEffectScriptable areaEffectScriptable)
        {
            if (areaEffectScriptable.obstacleTypes != ObstacleTypes.GasStation) return;
            _inPowerUp = false;

            PauseDecoy(false); // Retoma o consumo de combustível
        }

        private void StartFillUp(AreaEffectScriptable areaEffectScriptable)
        {
            if (areaEffectScriptable.obstacleTypes != ObstacleTypes.GasStation) return;
            _inPowerUp = true;
            _areaEffectCadence = areaEffectScriptable.areaEffectCadence;

            PauseDecoy(true); // Pausa o consumo de combustível
        }

        #endregion

        private void UpdateFuelState()
        {
            if (_inPowerUp)
            {
                // Recarregar combustível
                var lastFillFuel = _areaEffectCadence * Time.deltaTime;
                GetFuel += lastFillFuel;
                GetFuel = Mathf.Clamp(GetFuel, 0f, GetMaxFuel);
                GameStatisticManager.instance.LogFuelCharge(lastFillFuel);
            }
            else if (!_fuelPause && _playerMaster.ObjectIsReady && !_playerMaster.godMode && !_playerMaster.AutoPilot)
            {
                // Consumir combustível
                var lastFuel = reduceFuelCadence * Time.deltaTime;
                GetFuel -= lastFuel;
                GetFuel = Mathf.Clamp(GetFuel, 0f, GetMaxFuel);
                GameStatisticManager.instance.LogFuelSpend(lastFuel);
                if (GetFuel <= 0)
                {
                    _playerMaster.OnEventPlayerMasterGetHit();
                    GameStatisticManager.instance.LogFuelOut(1);
                }
            }
        }
    }
}
