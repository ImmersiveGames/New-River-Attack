using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RiverAttack
{

    [RequireComponent(typeof(PlayerMaster))]
    public class PlayerPowerUp : MonoBehaviour
    {
        public Dictionary<PowerUp, float> activePowerups = new Dictionary<PowerUp, float>();
        private List<PowerUp> keys = new List<PowerUp>();

        private PlayerMaster playerMaster;
        private CollectibleScriptable collectibles;
        private GamePlayPowerUps gamePlayPowerUps;

        private void OnEnable()
        {
            SetInitialReferences();
            playerMaster.EventPlayerReload += ResetPowerUp;

        }

        private void SetInitialReferences()
        {
            playerMaster = GetComponent<PlayerMaster>();
        }

        private void HandleGlobalPowerUps()
        {
            bool changed = false;

            if (activePowerups.Count > 0)
            {
                foreach (PowerUp powerup in keys)
                {
                    if (activePowerups[powerup] > 0)
                    {
                        activePowerups[powerup] -= Time.deltaTime;
                    }
                    else
                    {
                        changed = true;
                        activePowerups.Remove(powerup);
                        powerup.PowerUpEnd(GetComponent<PlayerMaster>().PlayersSettings());
                    }
                }
            }

            if (changed)
            {
                keys = new List<PowerUp>(activePowerups.Keys);
            }
        }

        // checks for disabling global powerups
        void Update()
        {
            HandleGlobalPowerUps();
        }

        public void ActivatePowerup(PowerUp powerup) //, Player target)
        {
            //Debug.Log("AQUI: "+ powerup.name);
            ClearActivePowerups(powerup.canAccumulateEffects);
            if (!activePowerups.ContainsKey(powerup))
            {
                //Debug.Log("Não tem esse powerup na lista");
                powerup.PowerUpStart(playerMaster.PlayersSettings());
                activePowerups.Add(powerup, powerup.duration);
            }
            else
            {
                //Debug.Log("Ja tem esse power Up adiciona tempo");
                if (powerup.canAccumulateDuration)
                    activePowerups[powerup] += powerup.duration;
                else
                    activePowerups[powerup] = powerup.duration;
            }
            keys = new List<PowerUp>(activePowerups.Keys);
        }

        public void ResetPowerUp()
        {
            ClearActivePowerups();
        }

        // Calls the end action of each powerup and clears them from the activePowerups
        public void ClearActivePowerups(bool onlyeffect = false) //(Player target, bool onlyeffect = false)
        {
            //Debug.Log("Can Acumulate: "+ onlyeffect);
            foreach (KeyValuePair<PowerUp, float> powerup in activePowerups)
            {
                if (onlyeffect && !powerup.Key.canAccumulateEffects)
                    return;
                //Debug.Log("Termina o Powerup");
                powerup.Key.PowerUpEnd(playerMaster.PlayersSettings());
            }
            if (!onlyeffect)
                activePowerups.Clear();
        }

        private void OnDisable()
        {
            playerMaster.EventPlayerReload -= ResetPowerUp;
        }
    }
}
