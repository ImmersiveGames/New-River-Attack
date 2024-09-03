using System.Collections.Generic;
using System.Linq;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.ObjectManagers.Interfaces;
using ImmersiveGames.ObjectManagers.PowerUpManagers;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.CollectibleSystems;
using NewRiverAttack.ObstaclesSystems.CollectibleSystems.PowerUpSystems;
using UnityEngine;

namespace NewRiverAttack.PlayerManagers.PlayerSystems
{
    public class PlayerPowerUp : MonoBehaviour
    {
        private PlayerMaster _playerMaster;
        private List<ActivePowerUp> _activePowerUps;
        private GamePlayManager _gamePlayManager;
        
        #region Unity Methods

        private void OnEnable()
        {
            SetInitialReferences();
            _playerMaster.EventPlayerMasterCollect += SetupPowerUp;
            _playerMaster.EventPlayerMasterGetHit += EndAllPowerUps;
            _gamePlayManager.EventGameFinisher += EndAllPowerUps;
        }

        private void Update()
        {
            ProcessPowerUps();
            if (IsPowerUpActive(PowerUpTypes.RapidFire))
            {
                var timeRapid = GetPowerUpRemainingTime(PowerUpTypes.RapidFire);
                _gamePlayManager.OnEventHudRapidFireUpdate(timeRapid, _playerMaster.PlayerIndex);
            }
        }

        private void OnDisable()
        {
            _playerMaster.EventPlayerMasterCollect -= SetupPowerUp;
            _playerMaster.EventPlayerMasterGetHit -= EndAllPowerUps;
            _gamePlayManager.EventGameFinisher -= EndAllPowerUps;
        }
        #endregion
        
        private void SetInitialReferences()
        {
            _playerMaster = GetComponent<PlayerMaster>();
            _gamePlayManager = GamePlayManager.instance;
            _activePowerUps = new List<ActivePowerUp>();
        }

        private void SetupPowerUp(ICollectable collectable)
        {
            var collected = collectable as CollectibleMaster;
            if (collected == null) return;
            var collectedSettings = collected.GetPowerUpSettings;
            if (collectedSettings == null) return;
            var powerUpData = collectedSettings.powerUpData;
            
            if (powerUpData.uniqueEffect)
            {
                // Se accumulateEffects for falso, interrompa todos os power-ups ativos
                DebugManager.Log<PlayerPowerUp>("Não acumula Efeitos, removendo todos os outros.");
                EndAllPowerUps();
            }

            // Verificar se há um power-up ativo do mesmo tipo
            var activePowerUp = FindActivePowerUp(powerUpData.powerUpType);
            if (activePowerUp != null)
            {
                // Se acumular duração estiver permitido, acumule a duração do power-up do mesmo tipo
                if (powerUpData.accumulateDuration)
                {
                    activePowerUp.EndTime += powerUpData.duration;
                    DebugManager.Log<PlayerPowerUp>("Duração do power-up acumulada.");
                }
                else
                {
                    activePowerUp.EndTime = Time.time + powerUpData.duration;
                    DebugManager.Log<PlayerPowerUp>("Power-up não acumula duração. Ele reinicia o contador");
                }
            }
            else
            {
                // Adicionar novo power-up à fila
                var newPowerUp = new ActivePowerUp
                {
                    PowerUpData = powerUpData,
                    EndTime = Time.time + powerUpData.duration
                };
                _activePowerUps.Add(newPowerUp);
                _playerMaster.OnEventPlayerMasterStartPowerUp(newPowerUp);
            }
        }

        private void ProcessPowerUps()
        {
            if (_activePowerUps.Count == 0) return;

            var currentTime = Time.time;

            for (var i = _activePowerUps.Count - 1; i >= 0; i--)
            {
                if (!(_activePowerUps[i].EndTime <= currentTime)) continue;
                var expiredPowerUp = _activePowerUps[i];
                _activePowerUps.RemoveAt(i);
                _playerMaster.OnEventPlayerMasterEndPowerUp(expiredPowerUp);
            }
        }

        private void EndAllPowerUps()
        {
            while (_activePowerUps.Count > 0)
            {
                var powerUp = _activePowerUps[0];
                _activePowerUps.RemoveAt(0);
                _playerMaster.OnEventPlayerMasterEndPowerUp(powerUp);
                DebugManager.Log<PlayerPowerUp>($"Power-up {powerUp.PowerUpData.powerUpType} interrompido.");
            }
        }

        #region Support Functions

        private ActivePowerUp FindActivePowerUp(PowerUpTypes powerUpType)
        {
            return _activePowerUps.FirstOrDefault(powerUp => powerUp.PowerUpData.powerUpType == powerUpType);
        }

        private bool IsPowerUpActive(PowerUpTypes powerUpType)
        {
            return FindActivePowerUp(powerUpType) != null;
        }

        private float GetPowerUpRemainingTime(PowerUpTypes powerUpType)
        {
            var activePowerUp = FindActivePowerUp(powerUpType);
            if (activePowerUp != null)
            {
                return activePowerUp.EndTime - Time.time;
            }
            return 0f; // Retorna 0 se o power-up não estiver ativo
        }

        public void EndSpecificPowerUp(PowerUpTypes powerUpType)
        {
            var activePowerUp = FindActivePowerUp(powerUpType);
            if (activePowerUp == null) return;
            _activePowerUps.Remove(activePowerUp);
            _playerMaster.OnEventPlayerMasterEndPowerUp(activePowerUp);
        }

        #endregion
        
    }
    
    public class ActivePowerUp
    {
        public PowerUpData PowerUpData;
        public float EndTime;
    }
}
