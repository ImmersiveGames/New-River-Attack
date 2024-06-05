using System.Collections.Generic;
using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class MissileAttackBehavior: IBossBehavior
    {
        private const float SHOOT_SPEED = 80.0f;
        private const float SHOOT_CYCLES = 4;
        private const float START_CADENCE = 1f;
        private const float BULLET_LIFE_TIME = 10f;

        //Shoot Variaveis
        private float m_Cadence;
        private float m_ShootCycles;
        private readonly int m_NumBullets;
        private readonly float m_ConeAngle;

        private readonly IHasPool m_IHasPool;
        private Transform m_MyPool;

        private Transform m_Target;

        //IBossBehavior
        private bool m_Finished;
        private readonly BossMasterOld _mBossMasterOld;
        private readonly BossMissileShoot m_BossMissileShoot;
        internal MissileAttackBehavior(BossMasterOld bossMasterOld, int numMissile, float angle)
        {
            _mBossMasterOld = bossMasterOld;
            m_NumBullets = (numMissile <= 0)? 3 : numMissile;
            m_ConeAngle = (angle <= 15) ? 15 : angle;
            m_IHasPool = m_BossMissileShoot = _mBossMasterOld.GetBossMissileShoot();
        }
        public void Enter()
        {
           // Debug.Log("Entrando no comportamento MissileAttack");
            m_Cadence = START_CADENCE;
            m_ShootCycles = SHOOT_CYCLES;
            m_Finished = false;
            m_Target = _mBossMasterOld.targetPlayer;
            m_MyPool = PoolObjectManager.GetPool(m_IHasPool);
        }
        public void Update()
        {
            if(!_mBossMasterOld.shouldBeBossBattle) return;
            //Debug.Log("Atualizando comportamento MissileAttack");
            m_Cadence -= Time.deltaTime;
            if (!(m_Cadence <= 0.01f))
                return;
            m_Cadence = START_CADENCE;
            if (m_BossMissileShoot.getBulletsMissile == null || m_Target == null)
                return;
            if (m_ShootCycles <= 0)
            {
                m_Finished = true;
                return;
            }
            //Debug.Log("Shoot!");
            Fire();
            m_ShootCycles--;
        }
        public void Exit()
        {
            //Debug.Log("Saindo do comportamento MissileAttack");
        }
        public void FinishBehavior()
        {
            m_Finished = true;
        }
        public bool IsFinished()
        {
            return m_Finished;
        }

        private void Fire()
        {
            if (GamePlayManager.instance.playerDead) return;
            //Debug.Log("Shoot!");
            var bossPosition = _mBossMasterOld.transform.position;
            var targetTransform = m_Target.transform.position;
            var targetDirection = (targetTransform - bossPosition).normalized;
            var directions = ConeDirections(targetDirection, m_NumBullets, m_ConeAngle);
            
            foreach (var t in directions)
            {
                var myShoot = PoolObjectManager.GetObject(m_IHasPool);
                var myBullet = myShoot.GetComponent<BulletBoss>();
                
                if (myBullet == null) continue;
                myBullet.SetMyPool(m_MyPool);
                myBullet.ownerShoot = _mBossMasterOld;
                myBullet.Init(SHOOT_SPEED, BULLET_LIFE_TIME);
                myBullet.moveDirection = t;
            }
        }

        private static IEnumerable<Vector3> ConeDirections(Vector3 targetDirection, int numMissile, float angleCone)
        {
            var directions = new Vector3[numMissile];

            float centerAngle = Mathf.Atan2(targetDirection.z, targetDirection.x) * Mathf.Rad2Deg;
            float initialAngle = centerAngle - angleCone / 2;
            float angleIncrease = angleCone / (numMissile - 1);

            for (int i = 0; i < numMissile; i++)
            {
                float angle = initialAngle + angleIncrease * i;
                float deg2Rad = angle * Mathf.Deg2Rad;
                var finalDirection = new Vector3(Mathf.Cos(deg2Rad), 0f, Mathf.Sin(deg2Rad));
                directions[i] = finalDirection;
            }
            return directions;
        }
       
    }
}
