using UnityEngine;
using Utils;

namespace RiverAttack
{
    public class StateMove : IMove
    {
        readonly EnemiesMovement m_EnemiesMovement;
        float m_ElapsedTime;
        float m_MoveVelocity;
        float m_MultiplyEnemiesSpeedy;
        Vector3 m_VectorDirection;
        EnemiesSetDifficulty m_EnemiesSetDifficulty;

        public StateMove(EnemiesMovement enemiesMovement)
        {
            m_EnemiesMovement = enemiesMovement;
            m_MultiplyEnemiesSpeedy = 1;
        }
        public void EnterState(EnemiesMaster enemiesMaster)
        {
           // Debug.Log("Estado: MOVE - Entrando: ");
            m_MoveVelocity = m_EnemiesMovement.moveVelocity * m_MultiplyEnemiesSpeedy;
            m_VectorDirection = m_EnemiesMovement.SetDirection(m_EnemiesMovement.startDirection);

            if (enemiesMaster.enemy && enemiesMaster.enemy.enemiesSetDifficultyListSo)
            {
                m_EnemiesSetDifficulty = enemiesMaster.enemy.enemiesSetDifficultyListSo.GetDifficultByEnemyDifficult(enemiesMaster.actualDifficultName);
                m_MultiplyEnemiesSpeedy = m_EnemiesSetDifficulty.multiplyEnemiesSpeedy;
            }

            enemiesMaster.OnEventObstacleMovement(m_VectorDirection);
        }
        public void UpdateState(Transform transform, Vector3 direction)
        {
            //Debug.Log("Estado: MOVE - UPDATE" + m_MoveVelocity);
            Move(transform, direction, m_MoveVelocity);
        }
        public void ExitState()
        {
           // Debug.Log("Estado: MOVE - Exit");
        }
        void Move(Transform objMove, Vector3 direction, float velocity)
        {
            float curveValue = 1f;
            if (m_EnemiesMovement.animationCurve != null && m_EnemiesMovement.animationDuration != 0)
            {
                curveValue = MoveCurveAnimation(m_EnemiesMovement.animationDuration, m_EnemiesMovement.animationCurve);
            }
            //Debug.Log("MOVE:" + curveValue);
            objMove.Translate(direction * (curveValue * (velocity * Time.deltaTime)));
        }
        float MoveCurveAnimation(float duration, AnimationCurve curve)
        {
            m_ElapsedTime += Time.deltaTime;

            // Verifica se a animação terminou e reinicia se necessário
            if (m_ElapsedTime >= duration) { m_ElapsedTime = 0.0f; }
            float curveFactor = Mathf.Clamp01(m_ElapsedTime / duration);

            // Usa a curva de animação para obter a interpolação de movimento
            return curve.Evaluate(curveFactor);
        }
    }
}
