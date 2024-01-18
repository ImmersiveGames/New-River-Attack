using System;
using UnityEngine;

namespace RiverAttack
{
    public class LevelFinish : LevelCheck
    {
        GamePlayManager m_GamePlayManager;
        #region UNITY METHODS
        void OnEnable()
        {
            m_GamePlayManager = GamePlayManager.instance;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<PlayerMaster>() == null) return;
            if (m_GamePlayManager.readyToFinish != true) return;
            if (!GamePlayingLog.instance.finishLevels.Contains(m_GamePlayManager.actualLevels))
            {
                m_GamePlayManager.actualLevels.levelsStates = LevelsStates.Complete;
                GamePlayingLog.instance.finishLevels.Add(m_GamePlayManager.actualLevels);
                if (GameManager.instance.gameModes == GameManager.GameModes.Mission)
                {
                    switch (m_GamePlayManager.actualLevels.pathType)
                    {
                        case LevelTypes.Grass:
                            GameSteamManager.UnlockAchievement("ACH_FINISH_M_GRASS");
                            break;
                        case LevelTypes.Forest:
                            GameSteamManager.UnlockAchievement("ACH_FINISH_M_FOREST");
                            break;
                        case LevelTypes.Swamp:
                            GameSteamManager.UnlockAchievement("ACH_FINISH_M_SWAMP");
                            break;
                        case LevelTypes.Antique:
                            GameSteamManager.UnlockAchievement("ACH_FINISH_M_ANCIENT");
                            break;
                        case LevelTypes.Desert:
                            GameSteamManager.UnlockAchievement("ACH_FINISH_M_DESERT");
                            break;
                        case LevelTypes.Ice:
                            GameSteamManager.UnlockAchievement("ACH_FINISH_M_ICE");
                            break;
                    }
                }
            }
                
            GameTimelineManager.instance.CompletePathEndCutScene();
        }
  #endregion
        
    }
}
