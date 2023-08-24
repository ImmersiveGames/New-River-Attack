using UnityEngine;
namespace RiverAttack
{
    public class PowerUpMaster : CollectiblesMaster
    {
        internal override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            var playerPowerUp = other.GetComponentInParent<PlayerPowerUp>();
            if(other == null) return;
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

        void ComponentToPowerUp(PlayerPowerUp playerPowerUp)
        {
            if (playerPowerUp != null && collectibleScriptable.powerUp != null)
                playerPowerUp.ActivatePowerUp(collectibleScriptable.powerUp);
        }
    }
}
