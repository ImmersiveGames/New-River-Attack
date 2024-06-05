using System;
using NewRiverAttack.SteamGameManagers;
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
        private PlayerMasterOld _mPlayerMasterOld;
        private PlayerSettings m_PlayerSettings;

        #region UNITYMETHODS

        private void OnEnable()
        {
            SetInitialReferences();
        }

        private void Update()
        {
            if (!m_GamePlayManager.shouldBePlayingGame || !_mPlayerMasterOld.ShouldPlayerBeReady) return;
            if (m_GamePlayManager.getGodMode || _mPlayerMasterOld.inEffectArea) return;
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
            if(SteamGameManager.ConnectedToSteam)
                SteamGameManager.UnlockAchievement("ACH_DIE_PLAYER_GAS");
            _mPlayerMasterOld.OnEventPlayerMasterHit();
        }

        private void OnDisable()
        {
            if(SteamGameManager.ConnectedToSteam)
                SteamGameManager.StoreStats();
        }
  #endregion

  private void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
            _mPlayerMasterOld = GetComponent<PlayerMasterOld>();
            m_PlayerSettings = _mPlayerMasterOld.getPlayerSettings;
            m_GamePlayingLog = m_GamePlayManager.gamePlayingLog;
        }

        private void LogGamePlay(int reduce)
        {
            m_GamePlayingLog.fuelSpent += reduce;
            SteamGameManager.AddStat("stat_SpendGas", reduce, false);
        }
    }
}
