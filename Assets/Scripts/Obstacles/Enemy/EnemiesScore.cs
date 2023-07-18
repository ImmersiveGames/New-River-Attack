using UnityEngine;

namespace RiverAttack
{
    [RequireComponent(typeof(EnemiesMaster))]
    public sealed class EnemyScore : MonoBehaviour
    {

        EnemiesMaster m_EnemyMaster;
        EnemiesDifficulty m_EnemyDifficulties;
        void OnEnable()
        {
            SetInitialReferences();
            m_EnemyMaster.EventPlayerDestroyEnemy += SetScore;
        }
        void SetInitialReferences()
        {
            m_EnemyMaster = GetComponent<EnemiesMaster>();
            if (GetComponent<EnemiesDifficulty>())
            {
                m_EnemyDifficulties = GetComponent<EnemiesDifficulty>();
            }
        }
        void OnDisable()
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
            playerMaster.GetPlayersSettings().score += (int)(score);
        }

        private void Log(int score)
        {
            if (score > GamePlaySettings.instance.totalScore)
                GamePlaySettings.instance.totalScore = score;
        }
    }
}
