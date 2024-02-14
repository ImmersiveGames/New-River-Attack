using System;
using UnityEngine;
namespace RiverAttack
{
    public class PowerUpMaster : CollectiblesMaster
    {

        #region UNITYMETHODS
        internal override void OnEnable()
        {
            base.OnEnable();
            gamePlayManager.EventReSpawnEnemiesMaster += DestroyPowerUp;
            gamePlayManager.EventEnemiesMasterForceRespawn += DestroyPowerUp;
        }
        internal void OnDisable()
        {
            gamePlayManager.EventReSpawnEnemiesMaster -= DestroyPowerUp;
            gamePlayManager.EventEnemiesMasterForceRespawn -= DestroyPowerUp;
        }
  #endregion
        internal override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            var playerPowerUp = other.GetComponentInParent<PlayerPowerUp>();
            if (other == null) return;
            ComponentToPowerUp(playerPowerUp);
        }
        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            if (collectibleScriptable != null && collectibleScriptable.powerUp)
            {
                name += "(" + collectibleScriptable.powerUp.name + ")";
            }
        }

        private void DestroyPowerUp()
        {
            DestroyImmediate(gameObject);
        }

        private void ComponentToPowerUp(PlayerPowerUp playerPowerUp)
        {
            if (playerPowerUp == null || collectibleScriptable.powerUp == null) return;
            playerPowerUp.ActivatePowerUp(collectibleScriptable.powerUp);
            AchievementHandler(collectibleScriptable.powerUp);
        }

        private void AchievementHandler(PowerUp powerUp)
        {
            switch (powerUp.powerUpType)
            {
                case PowerUpTypes.RapidFire:
                    GameSteamManager.UnlockAchievement("ACH_COLLECT_RAPID_FIRE");
                    break;
                case PowerUpTypes.Lives:
                    GameSteamManager.UnlockAchievement("ACH_COLLECT_LIFE");
                    break;
                case PowerUpTypes.Bomb:
                    GameSteamManager.UnlockAchievement("ACH_COLLECT_BOMB");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        protected override void OnEventCollectItem(PlayerSettings playerSettings)
        {
            base.OnEventCollectItem(playerSettings);
            ToggleChildren(false);
        }
    }
}
