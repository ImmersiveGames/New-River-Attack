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
        private float _spendFuel;
        [SerializeField] private float reduceFuelCadence = 1f; //Quanto maior mais rápido
        
        private bool _fuelPause;
        private bool _inPowerUp;
        
        private PlayerMaster _playerMaster;
        private PlayerAchievements _playerAchievements;
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
            if (_inPowerUp)
            {
                var lastFillFuel = _areaEffectCadence * Time.deltaTime;
                GetFuel += lastFillFuel;
                GetFuel = Mathf.Clamp(GetFuel, 0f, GetMaxFuel);
                GameStatisticManager.instance.LogFuelCharge(lastFillFuel);
            }
            if (_fuelPause || !_playerMaster.ObjectIsReady || _playerMaster.godMode || _playerMaster.AutoPilot) return;
            // Decrementa o combustível com base na taxa de decaimento por segundo
            var lastFuel = reduceFuelCadence * Time.deltaTime;
            GetFuel -= lastFuel;

            // Garante que o combustível não ultrapasse o máximo
            GetFuel = Mathf.Clamp(GetFuel, 0f, GetMaxFuel);
            GameStatisticManager.instance.LogFuelSpend(lastFuel);
            if (GetFuel > 0) return;
            _playerMaster.OnEventPlayerMasterGetHit();
            GameStatisticManager.instance.LogFuelOut(1);
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
            _playerMaster.EventPlayerMasterStopDecoyFuel -= PauseDecoy;
        }

        #endregion

        private void SetInitialReferences()
        {
            _playerMaster = GetComponent<PlayerMaster>();
            _playerAchievements = GetComponent<PlayerAchievements>();
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
            _playerAchievements.LogSpendFuel(_spendFuel);
            _spendFuel = 0;
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
            if(shopProductSkin.maxFuel > 0)
                GetMaxFuel = shopProductSkin.maxFuel;
            if(shopProductSkin.cadenceFuel > 0)
                reduceFuelCadence = shopProductSkin.cadenceFuel;
            DebugManager.Log<PlayerFuel>($"Na Mudança de skin");
        }

        #region PowerUp GasStatioin

        private void EndFillEnd(AreaEffectScriptable areaEffectScriptable)
        {
            if (areaEffectScriptable.obstacleTypes != ObstacleTypes.GasStation) return;
            _inPowerUp = false;
            PauseDecoy(_inPowerUp);
        }

        private void StartFillUp(AreaEffectScriptable areaEffectScriptable)
        {
            if (areaEffectScriptable.obstacleTypes != ObstacleTypes.GasStation) return;
            _inPowerUp = true;
            _areaEffectCadence = areaEffectScriptable.areaEffectCadence;

            PauseDecoy(_inPowerUp);
        }

        #endregion
    }
}