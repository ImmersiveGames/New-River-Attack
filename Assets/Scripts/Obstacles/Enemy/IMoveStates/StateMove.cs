using UnityEngine;
using Utils;

namespace RiverAttack
{
    public class StateMove : IMove
    {
        private readonly EnemiesMovement m_EnemiesMovement;
        private readonly ObstacleMasterOld _mObstacleMasterOld;
        private float m_ElapsedTime;
        private float m_MoveVelocity;
        private float m_MultiplyEnemiesSpeedy;
        private Vector3 m_VectorDirection;
        private EnemiesSetDifficulty m_EnemiesSetDifficulty;

        public StateMove(EnemiesMovement enemiesMovement, ObstacleMasterOld obstacleMasterOld)
        {
            m_EnemiesMovement = enemiesMovement;
            m_MultiplyEnemiesSpeedy = 1;
            _mObstacleMasterOld = obstacleMasterOld;
            _mObstacleMasterOld.OnEventObstacleMovement(true);
        }
        public void EnterState()
        {
           //Debug.Log($"{m_EnemiesMaster.gameObject.name} Estado: MOVE - Entrando ");
           m_MoveVelocity = m_EnemiesMovement.moveVelocity * m_MultiplyEnemiesSpeedy;
           if (!_mObstacleMasterOld.enemy || !_mObstacleMasterOld.enemy.enemiesSetDifficultyListSo)
               return;
           m_EnemiesSetDifficulty = _mObstacleMasterOld.enemy.enemiesSetDifficultyListSo.GetDifficultByEnemyDifficult(_mObstacleMasterOld.actualDifficultName);
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
           _mObstacleMasterOld.OnEventObstacleMovement(false);
        }


        private void Move(Transform objMove, Vector3 direction, float velocity)
        {
            float curveValue = 1f;
            if (m_EnemiesMovement.animationCurve != null && m_EnemiesMovement.animationDuration != 0)
            {
                curveValue = MoveCurveAnimation(m_EnemiesMovement.animationDuration, m_EnemiesMovement.animationCurve);
            }
            //Debug.Log("MOVE:" + curveValue);
            objMove.Translate(direction * (curveValue * (velocity * Time.deltaTime)));
        }

        private float MoveCurveAnimation(float duration, AnimationCurve curve)
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
