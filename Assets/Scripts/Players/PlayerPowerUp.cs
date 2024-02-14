using System.Collections.Generic;
using UnityEngine;
namespace RiverAttack
{
    public class PlayerPowerUp : MonoBehaviour
    {
        private readonly Dictionary<PowerUp, float> m_ActivePowerUps = new Dictionary<PowerUp, float>();

        private List<PowerUp> m_Keys = new List<PowerUp>();
        private PlayerMaster m_PlayerMaster;

        #region UNITYMETHODS

        private void OnEnable()
        {
            SetInitialReferences();
            ClearActivePowerUps();
            m_PlayerMaster.EventPlayerMasterRespawn += ResetPowerUp;
        }

        private void Update()
        {
            HandleGlobalPowerUps();
        }

        private void OnDisable()
        {
            m_PlayerMaster.EventPlayerMasterRespawn -= ResetPowerUp;
        }
  #endregion

  private void SetInitialReferences()
        {
            m_PlayerMaster = GetComponent<PlayerMaster>();
        }

        private void HandleGlobalPowerUps()
        {
            bool changed = false;

            if (m_ActivePowerUps.Count > 0)
            {
                var playerMaster = GetComponent<PlayerMaster>();
                foreach (var powerUp in m_Keys)
                {
                    if (m_ActivePowerUps[powerUp] > 0)
                    {
                        m_ActivePowerUps[powerUp] -= Time.deltaTime;
                        //Debug.Log($"POWERUP - Desc: {m_ActivePowerUps[powerUp]}");
                    }
                    else
                    {
                        changed = true;
                        m_ActivePowerUps.Remove(powerUp);
                        powerUp.PowerUpEnd(playerMaster.getPlayerSettings);
                    }
                }
            }
            if (changed)
            {
                m_Keys = new List<PowerUp>(m_ActivePowerUps.Keys);
            }
        }

        public void ActivatePowerUp(PowerUp powerUp)
        {
            //Debug.Log("AQUI: "+ powerUp.name);
            ClearActivePowerUps(powerUp.canAccumulateEffects);
            if (!m_ActivePowerUps.ContainsKey(powerUp))
            {
               // Debug.Log("Não tem esse power Up na lista");
                powerUp.PowerUpStart(m_PlayerMaster.getPlayerSettings);
                m_ActivePowerUps.Add(powerUp, powerUp.duration);
            }
            else
            {
                //Debug.Log("Ja tem esse power Up adiciona tempo");
                if (powerUp.canAccumulateDuration)
                    m_ActivePowerUps[powerUp] += powerUp.duration;
                else
                    m_ActivePowerUps[powerUp] = powerUp.duration;
            }
            //Aqui parece um bom send para HUD
            GamePlayManager.instance.OnEventUpdatePowerUpDuration(powerUp, m_ActivePowerUps[powerUp]);
            m_Keys = new List<PowerUp>(m_ActivePowerUps.Keys);
        }

        // Calls the end action of each powerup and clears them from the activePowerups
        private void ClearActivePowerUps(bool onlyEffect = false) //(Player target, bool onlyeffect = false)
        {
            //Debug.Log($"Can Accumulate Effect: {onlyEffect}");
            foreach (var powerUp in m_ActivePowerUps)
            {
               // Debug.Log($"Power up =  {powerUp}");
                if (onlyEffect && !powerUp.Key.canAccumulateEffects)
                    return;
                //Debug.Log($"Termina o Power up {powerUp}");
                powerUp.Key.PowerUpEnd(m_PlayerMaster.getPlayerSettings);
            }
            if (!onlyEffect)
                m_ActivePowerUps.Clear();
        }

        private void ResetPowerUp()
        {
            ClearActivePowerUps();
        }

    }
}
