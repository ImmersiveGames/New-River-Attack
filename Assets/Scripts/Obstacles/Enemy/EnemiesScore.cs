﻿using UnityEngine;

namespace RiverAttack
{
    public class EnemiesScore : MonoBehaviour
    {
        GamePlayManager m_GamePlayManager;
        ObstacleMaster m_ObstacleMaster;

        #region UNITY METHODS
        protected virtual void OnEnable()
        {
            SetInitialReferences();
            m_ObstacleMaster.EventPlayerDestroyObject += SetScore;
        }
        protected virtual void OnDisable()
        {
            m_ObstacleMaster.EventPlayerDestroyObject -= SetScore;
        }
  #endregion

        protected virtual void SetInitialReferences()
        {
            m_ObstacleMaster = GetComponent<ObstacleMaster>();
            m_GamePlayManager = GamePlayManager.instance;

        }
        void SetScore(PlayerMaster playerMaster)
        {
            float score = m_ObstacleMaster.enemy.enemyScore;
            if (EnemiesMaster.myDifficulty.multiplyScore > 0)
            {
                var myDifficulty = EnemiesMaster.myDifficulty;
                if (myDifficulty.multiplyScore > 0)
                    score *= myDifficulty.multiplyScore;
            }
            if (playerMaster == null) return;
            playerMaster.GetPlayersSettings().score += (int)(score);
            m_GamePlayManager.CallEventUIScore(playerMaster.GetPlayersSettings().score);
        }
        void Log(int score)
        {
            if (score > GamePlaySettings.instance.totalScore)
                GamePlaySettings.instance.totalScore = score;
        }
        
    }
}
