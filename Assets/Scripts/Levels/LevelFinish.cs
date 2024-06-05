using System;
using NewRiverAttack.SteamGameManagers;
using UnityEngine;

namespace RiverAttack
{
    public class LevelFinish : LevelCheck
    {
        private GamePlayManager m_GamePlayManager;
        #region UNITY METHODS

        private void OnEnable()
        {
            m_GamePlayManager = GamePlayManager.instance;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<PlayerMasterOld>() == null) return;
            if (m_GamePlayManager.readyToFinish != true) return;
            if (!GamePlayingLog.instance.GetLevelsResult().Contains(m_GamePlayManager.actualLevels))
            {
                m_GamePlayManager.actualLevels.levelsStates = LevelsStates.Complete;
                GamePlayingLog.instance.AddLevel(m_GamePlayManager.actualLevels);
                if (GameManager.instance.gameModes == GameManager.GameModes.Mission)
                {
                    switch (m_GamePlayManager.actualLevels.pathType)
                    {
                        case LevelTypes.Grass:
                            SteamGameManager.UnlockAchievement("ACH_FINISH_M_GRASS");
                            break;
                        case LevelTypes.Forest:
                            SteamGameManager.UnlockAchievement("ACH_FINISH_M_FOREST");
                            break;
                        case LevelTypes.Swamp:
                            SteamGameManager.UnlockAchievement("ACH_FINISH_M_SWAMP");
                            break;
                        case LevelTypes.Antique:
                            SteamGameManager.UnlockAchievement("ACH_FINISH_M_ANCIENT");
                            break;
                        case LevelTypes.Desert:
                            SteamGameManager.UnlockAchievement("ACH_FINISH_M_DESERT");
                            break;
                        case LevelTypes.Ice:
                            SteamGameManager.UnlockAchievement("ACH_FINISH_M_ICE");
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
                
            GameTimelineManager.instance.CompletePathEndCutScene();
        }
  #endregion
        
    }
}
