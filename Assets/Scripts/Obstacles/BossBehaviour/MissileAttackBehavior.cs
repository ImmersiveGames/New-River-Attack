using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class MissileAttackBehavior: IBossBehavior
    {
        int numBullets = 5;         // Número de projéteis no leque
        float coneAngle = 30.0f;      // Ângulo do leque em graus (mínimo 15, máximo 360)
        float shootSpeed = 5.0f; // Velocidade dos projéteis

        Vector3 bossPosition;
        bool m_Finished;
        BossMaster m_BossMaster;
        GameObject m_Bullets;
        Transform m_Target;
        internal MissileAttackBehavior(BossMaster bossMaster)
        {
            m_BossMaster = bossMaster;
            m_Bullets = m_BossMaster.GetBossShoot().GetBullets();
        }
        public void Enter()
        {
            Debug.Log("Entrando no comportamento MissileAttack");
            // Lógica de entrada para o comportamento MissileAttack
            m_Finished = false;
            m_Target = m_BossMaster.targetPlayer;

        }
        public void Update()
        {
            Debug.Log("Atualizando comportamento MissileAttack");
            
            // Lógica de atualização para o comportamento MissileAttack
            
            bossPosition = m_BossMaster.transform.position;
            var targetTransform = m_Target.transform.position;
        }
        public void Exit()
        {
            Debug.Log("Saindo do comportamento MissileAttack");
            m_Target = null;
            // Lógica de saída para o comportamento MissileAttack
        }
        public void FinishBehavior()
        {
            m_Finished = true;
        }
        public bool IsFinished()
        {
            return m_Finished;
        }
        
        void Shoot()
        {
            if (m_Bullets != null && m_Target != null && coneAngle >= 15.0f && coneAngle <= 360.0f)
            {
                Vector3 direcaoAlvo = (m_Target.position - bossPosition).normalized;
                Vector3[] direcoes = ConeDirections(direcaoAlvo, numBullets, coneAngle);

                for (int i = 0; i < direcoes.Length; i++)
                {
                    GameObject projetil = Object.Instantiate(m_Bullets, bossPosition, Quaternion.identity);
                    BulletBoss scriptProjetil = projetil.GetComponent<BulletBoss>();

                    if (scriptProjetil != null)
                    {
                        scriptProjetil.MoveShoot(direcoes[i], shootSpeed);
                    }
                }
            }
        }
        
        Vector3[] ConeDirections(Vector3 targetDirection, int numMissile, float angleCone)
        {
            var directions = new Vector3[numMissile];

            float centerAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            float initialAngle = centerAngle - angleCone / 2;
            float angleIncrease = angleCone / (numMissile - 1);

            for (int i = 0; i < numMissile; i++)
            {
                float angle = initialAngle + angleIncrease * i;
                var finalDirection = Quaternion.Euler(0, 0, angle) * Vector3.right;
                directions[i] = finalDirection;
            }
            return directions;
        }
    }
}
