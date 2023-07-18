using UnityEngine;

namespace RiverAttack
{
    [RequireComponent(typeof(EnemiesMaster))]
    public class EnemiesScore : MonoBehaviour
    {

        EnemiesMaster m_EnemyMaster;
        EnemiesDifficulty m_EnemyDifficulties;
        protected virtual void OnEnable()
        {
            SetInitialReferences();
            m_EnemyMaster.EventPlayerDestroyEnemy += SetScore;
        }
        protected virtual void SetInitialReferences()
        {
            m_EnemyMaster = GetComponent<EnemiesMaster>();
            if (GetComponent<EnemiesDifficulty>())
            {
                m_EnemyDifficulties = GetComponent<EnemiesDifficulty>();
            }
        }
        protected virtual void OnDisable()
        {
            m_EnemyMaster.EventPlayerDestroyEnemy -= SetScore;
        }

        private void SetScore(PlayerMaster playerMaster)
        {
            float score = m_EnemyMaster.enemy.enemyScore;
            if (m_EnemyDifficulties != null)
            {
                var myDifficulty = m_EnemyDifficulties.myDifficulty;
                if (myDifficulty.scoreMod > 0)
                    score *= myDifficulty.scoreMod;
            }
            playerMaster.PlayersSettings().score += (int)(score);
        }

        private void Log(int score)
        {
            if (score > GamePlaySettings.instance.totalScore)
                GamePlaySettings.instance.totalScore = score;
        }
    }
}
