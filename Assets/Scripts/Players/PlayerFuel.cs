using System;
using UnityEngine;

namespace RiverAttack
{
    public class PlayerFuel : MonoBehaviour
    {
        [SerializeField] private int reduceFuelRate;

        [SerializeField, Range(0, 5)] private float reduceFuelCadence;

        private float m_TimeLoop;

        private GamePlayManager m_GamePlayManager;
        private GamePlayingLog m_GamePlayingLog;
        private PlayerMaster m_PlayerMaster;
        private PlayerSettings m_PlayerSettings;

        #region UNITYMETHODS

        private void OnEnable()
        {
            SetInitialReferences();
        }

        private void Update()
        {
            if (!m_GamePlayManager.shouldBePlayingGame || !m_PlayerMaster.shouldPlayerBeReady) return;
            if (m_GamePlayManager.getGodMode || m_PlayerMaster.inEffectArea) return;
            m_TimeLoop += Time.deltaTime;
            if (m_TimeLoop < reduceFuelCadence) return;
            //Pode reduzir a Gasolina
            m_PlayerSettings.actualFuel -= reduceFuelRate;
            LogGamePlay(reduceFuelRate);

            // Reinicia o contador
            m_TimeLoop = 0;
            // Verifica se ainda tem gasolina
            if (m_PlayerSettings.actualFuel > 0) return;
            m_PlayerSettings.actualFuel = 0;
            if(GameSteamManager.connectedToSteam)
                GameSteamManager.UnlockAchievement("ACH_DIE_PLAYER_GAS");
            m_PlayerMaster.OnEventPlayerMasterHit();
        }

        private void OnDisable()
        {
            if(GameSteamManager.connectedToSteam)
                GameSteamManager.StoreStats();
        }
  #endregion

  private void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_PlayerSettings = m_PlayerMaster.getPlayerSettings;
            m_GamePlayingLog = m_GamePlayManager.gamePlayingLog;
        }

        private void LogGamePlay(int reduce)
        {
            m_GamePlayingLog.fuelSpent += reduce;
            GameSteamManager.AddStat("stat_SpendGas", reduce, false);
        }
    }
}
