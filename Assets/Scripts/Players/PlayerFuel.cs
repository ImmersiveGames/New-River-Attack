using UnityEngine;

namespace RiverAttack
{
    public class PlayerFuel : MonoBehaviour
    {
        [SerializeField]
        int reduceFuelRate;

        [SerializeField, Range(0, 5)]
        float reduceFuelCadence;

        float m_TimeLoop;

        GamePlayManager m_GamePlayManager;
        GamePlayingLog m_GamePlayingLog;
        PlayerMaster m_PlayerMaster;
        PlayerSettings m_PlayerSettings;

        #region UNITYMETHODS
        void OnEnable()
        {
            SetInitialReferences();
        }
        void Update()
        {
            if (!m_GamePlayManager.shouldBePlayingGame || !m_PlayerMaster.shouldPlayerBeReady) return;
            if (m_GamePlayManager.getGodMode || m_PlayerMaster.inPowerUp) return;
            m_TimeLoop += Time.deltaTime;
            if (m_TimeLoop < reduceFuelCadence) return;
            //Pode reduzir a Gasolina
            m_PlayerSettings.actualFuel -= reduceFuelRate;
            GameSteamManager.AddState("stat_SpendGas", reduceFuelRate, false);
            LogGamePlay(reduceFuelRate);

            // Reinicia o contador
            m_TimeLoop = 0;
            // Verifica se ainda tem gasolina
            if (m_PlayerSettings.actualFuel > 0) return;
            m_PlayerSettings.actualFuel = 0;
            GameSteamManager.UnlockAchievement("ACH_DIE_PLAYER_GAS");
            m_PlayerMaster.OnEventPlayerMasterHit();
        }
  #endregion
        void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_PlayerSettings = m_PlayerMaster.getPlayerSettings;
            m_GamePlayingLog = m_GamePlayManager.gamePlayingLog;
        }
        /*void IncreaseFuel(int fuel)
        {
            m_PlayerSettings.actualFuel = fuel;
            if (m_PlayerSettings.actualFuel > GamePlayManager.getGameSettings.maxFuel)
            {
                m_PlayerSettings.actualFuel = GamePlayManager.getGameSettings.maxFuel;
            }
        }*/

        void LogGamePlay(int reduce)
        {
            m_GamePlayingLog.fuelSpent += reduce;
            GameSteamManager.AddState("stat_SpendGas", reduce, false);
        }
    }
}
