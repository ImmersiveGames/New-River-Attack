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
        void DestroyPowerUp()
        {
            DestroyImmediate(gameObject);
        }
        void ComponentToPowerUp(PlayerPowerUp playerPowerUp)
        {
            if (playerPowerUp == null || collectibleScriptable.powerUp == null) return;
                playerPowerUp.ActivatePowerUp(collectibleScriptable.powerUp);
        }
    }
}
