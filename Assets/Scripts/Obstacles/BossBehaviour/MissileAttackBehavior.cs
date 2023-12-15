﻿using System.Collections.Generic;
using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class MissileAttackBehavior: IBossBehavior
    {
        // Shoot in Cone
        static int numBullets = 5; // Número de projéteis no leque
        float coneAngle = 90.0f;   // Ângulo do leque em graus (mínimo 15, máximo 360)
        float shootSpeed = 80.0f;  // Velocidade dos projéteis
        float shootCycles = 4;
        float startCadence = 1f;
        float bulletLifeTime = 5f;
        
        //Shoot Variaveis
        float m_Cadence;
        float m_ShootCycles;

        readonly IHasPool m_IHasPool;
        Transform m_MyPool;
        
        Transform m_Target;

        //IBossBehavior
        bool m_Finished;
        bool m_OnBehavior;
        readonly BossMaster m_BossMaster;
        readonly BossShoot m_BossShoot;
        internal MissileAttackBehavior(BossMaster bossMaster)
        {
            m_BossMaster = bossMaster;
            m_IHasPool = m_BossShoot = m_BossMaster.GetBossShoot();
        }
        public void Enter()
        {
            //Debug.Log("Entrando no comportamento CleanShootBehavior");
            m_Cadence = startCadence;
            m_ShootCycles = shootCycles;
            m_OnBehavior = false;
            m_Target = m_BossMaster.targetPlayer;
            m_MyPool = PoolObjectManager.GetPool(m_IHasPool);
        }
        public void Update()
        {
            //Debug.Log("Atualizando comportamento CleanShootBehavior");
            /*if (m_OnBehavior) return;
            m_OnBehavior = true;*/
            m_Cadence -= Time.deltaTime;
            if (!(m_Cadence <= 0.01f))
                return;
            m_Cadence = startCadence;
            if (m_BossShoot.getBullets == null || m_Target == null || coneAngle is < 15.0f or > 360.0f)
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
            //Debug.Log("Saindo do comportamento CleanShootBehavior");
        }
        public void FinishBehavior()
        {
            m_Finished = true;
        }
        public bool IsFinished()
        {
            return m_Finished;
        }

        void Fire()
        {
            if (GamePlayManager.instance.playerDead) return;
            //Debug.Log("Shoot!");
            var bossPosition = m_BossMaster.transform.position;
            var targetTransform = m_Target.transform.position;
            var targetDirection = (targetTransform - bossPosition).normalized;
            var directions = ConeDirections(targetDirection, numBullets, coneAngle);
            
            foreach (var t in directions)
            {
                var myShoot = PoolObjectManager.GetObject(m_IHasPool);
                var myBullet = myShoot.GetComponent<BulletBoss>();
                
                if (myBullet == null) continue;
                myBullet.SetMyPool(m_MyPool);
                myBullet.ownerShoot = m_BossMaster;
                myBullet.Init(shootSpeed, bulletLifeTime);
                myBullet.moveDirection = t;
            }
        }
        static IEnumerable<Vector3> ConeDirections(Vector3 targetDirection, int numMissile, float angleCone)
        {
            var directions = new Vector3[numMissile];

            float centerAngle = Mathf.Atan2(targetDirection.z, targetDirection.x) * Mathf.Rad2Deg;
            float initialAngle = centerAngle - angleCone / 2;
            float angleIncrease = angleCone / (numMissile - 1);

            for (int i = 0; i < numMissile; i++)
            {
                float angle = initialAngle + angleIncrease * i;
                float deg2Rad = angle * Mathf.Deg2Rad;
                var finalDirection = new Vector3(Mathf.Cos(deg2Rad), 0, Mathf.Sin(deg2Rad));
                directions[i] = finalDirection;
            }
            return directions;
        }
       
    }
}
