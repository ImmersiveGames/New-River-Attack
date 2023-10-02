using UnityEngine;
using Utils;

namespace RiverAttack
{
    public class StateMove : IMove
    {
        readonly EnemiesMovement m_EnemiesMovement;
        readonly EnemiesMaster m_EnemiesMaster;
        float m_ElapsedTime;
        float m_MoveVelocity;
        float m_MultiplyEnemiesSpeedy;
        Vector3 m_VectorDirection;
        EnemiesSetDifficulty m_EnemiesSetDifficulty;

        public StateMove(EnemiesMovement enemiesMovement, EnemiesMaster enemiesMaster)
        {
            m_EnemiesMovement = enemiesMovement;
            m_MultiplyEnemiesSpeedy = 1;
            m_EnemiesMaster = enemiesMaster;
            m_EnemiesMaster.OnEventObstacleMovement(true);
        }
        public void EnterState()
        {
           //Debug.Log($"{m_EnemiesMaster.gameObject.name} Estado: MOVE - Entrando ");
           m_MoveVelocity = m_EnemiesMovement.moveVelocity * m_MultiplyEnemiesSpeedy;
           if (!m_EnemiesMaster.enemy || !m_EnemiesMaster.enemy.enemiesSetDifficultyListSo)
               return;
           m_EnemiesSetDifficulty = m_EnemiesMaster.enemy.enemiesSetDifficultyListSo.GetDifficultByEnemyDifficult(m_EnemiesMaster.actualDifficultName);
           m_MultiplyEnemiesSpeedy = m_EnemiesSetDifficulty.multiplyEnemiesSpeedy;
        }
        public void UpdateState(Transform transform, Vector3 direction)
        {
            //Debug.Log($"{transform.gameObject.name} Estado: MOVE - UPDATE" + m_MoveVelocity);
            Move(transform, direction, m_MoveVelocity);
        }
        public void ExitState()
        {
           // Debug.Log("Estado: MOVE - Exit");
           m_EnemiesMaster.OnEventObstacleMovement(false);
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
