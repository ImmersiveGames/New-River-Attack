using System;
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
            if (other.GetComponentInParent<PlayerMaster>() == null) return;
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
                        case LevelTypes.Menu:
                            break;
                        case LevelTypes.Hub:
                            break;
                        case LevelTypes.GameOver:
                            break;
                        case LevelTypes.Complete:
                            break;
                        case LevelTypes.HUD:
                            break;
                        case LevelTypes.Boss:
                            break;
                        case LevelTypes.Tutorial:
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
