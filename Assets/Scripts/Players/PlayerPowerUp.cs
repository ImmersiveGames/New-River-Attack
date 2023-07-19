using System.Collections.Generic;
using UnityEngine;
namespace RiverAttack
{
    [RequireComponent(typeof(PlayerMaster))]
    public class PlayerPowerUp : MonoBehaviour
    {
        readonly Dictionary<PowerUp, float> m_ActivePowerUps = new Dictionary<PowerUp, float>();
        List<PowerUp> m_Keys = new List<PowerUp>();

        PlayerMaster m_PlayerMaster;
        CollectibleScriptable m_Collectibles;
        GamePlayPowerUps m_GamePlayPowerUps;

        #region UNITY METHODS
        private void OnEnable()
        {
            SetInitialReferences();
            m_PlayerMaster.EventPlayerReload += ResetPowerUp;
        }
        void Update()
        {
            HandleGlobalPowerUps();
        }
        void OnDisable()
        {
            m_PlayerMaster.EventPlayerReload -= ResetPowerUp;
        }
        
  #endregion
        void SetInitialReferences()
        {
            m_PlayerMaster = GetComponent<PlayerMaster>();
        }

        void HandleGlobalPowerUps()
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
                    }
                    else
                    {
                        changed = true;
                        m_ActivePowerUps.Remove(powerUp);
                        powerUp.PowerUpEnd(playerMaster.GetPlayersSettings());
                    }
                }
            }
            if (changed)
            {
                m_Keys = new List<PowerUp>(m_ActivePowerUps.Keys);
            }
        }
        
        public void ActivatePowerUp(PowerUp powerUp) //, Player target)
        {
            //Debug.Log("AQUI: "+ powerup.name);
            ClearActivePowerUps(powerUp.canAccumulateEffects);
            if (!m_ActivePowerUps.ContainsKey(powerUp))
            {
                //Debug.Log("Não tem esse powerup na lista");
                powerUp.PowerUpStart(m_PlayerMaster.GetPlayersSettings());
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
            m_Keys = new List<PowerUp>(m_ActivePowerUps.Keys);
        }

        void ResetPowerUp()
        {
            ClearActivePowerUps();
        }

        // Calls the end action of each powerup and clears them from the activePowerups
        void ClearActivePowerUps(bool onlyEffect = false) //(Player target, bool onlyeffect = false)
        {
            //Debug.Log("Can Acumulate: "+ onlyeffect);
            foreach (var powerUp in m_ActivePowerUps)
            {
                if (onlyEffect && !powerUp.Key.canAccumulateEffects)
                    return;
                //Debug.Log("Termina o Powerup");
                powerUp.Key.PowerUpEnd(m_PlayerMaster.GetPlayersSettings());
            }
            if (!onlyEffect)
                m_ActivePowerUps.Clear();
        }

    }
}
