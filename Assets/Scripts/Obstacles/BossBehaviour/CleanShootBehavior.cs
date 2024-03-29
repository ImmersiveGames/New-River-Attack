﻿using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class CleanShootBehavior : IBossBehavior
    {
        const float SHOOT_CYCLES = 4;
        const float SHOOT_SPEED = 40.0f;
        const float START_CADENCE = 1f;
        const float BULLET_LIFE_TIME = 5f;

        //Shoot Variaveis
        float m_Cadence;
        float m_ShootCycles;
        
        Transform m_SpawnPoint;
        readonly IHasPool m_MyPool;
        
        Transform m_Target;

        //IBossBehavior
        bool m_Finished;
        readonly BossMaster m_BossMaster;
        readonly BossMissileShoot m_BossMissileShoot;
        internal CleanShootBehavior(BossMaster bossMaster)
        {
            m_BossMaster = bossMaster;
            m_MyPool = m_BossMissileShoot = m_BossMaster.GetBossMissileShoot();
        }
        public void Enter()
        {
            //Debug.Log("Entrando no comportamento CleanShootBehavior");
            m_Cadence = START_CADENCE;
            m_ShootCycles = SHOOT_CYCLES;
            
            m_SpawnPoint = m_BossMissileShoot.spawnPoint;
            m_Target = m_BossMaster.targetPlayer;
        }
        public void Update()
        {
            //Debug.Log("Atualizando comportamento CleanShootBehavior");
            if(!m_BossMaster.shouldBeBossBattle) return;
            m_Cadence -= Time.deltaTime;
            if (!(m_Cadence <= 0.01f))
                return;
            m_Cadence = START_CADENCE;
            if (m_ShootCycles <= 0)
            {
                m_Finished = true;
                return;
            }
            m_ShootCycles--;
            Fire();
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
            var targetTransform = m_Target.transform.position;
            
            var myShoot = PoolObjectManager.GetObject(m_MyPool);
            //setting bullet entity
            var myBullet = myShoot.GetComponent<BulletBoss>();
            myBullet.SetMyPool(PoolObjectManager.GetPool(m_MyPool));
            myBullet.ownerShoot = m_BossMaster;
            myBullet.Init(SHOOT_SPEED, BULLET_LIFE_TIME);
            myBullet.moveDirection = targetTransform;
            //Deattached bullet
            var transformPosition = m_SpawnPoint.position;
            var transformRotation = m_SpawnPoint.rotation;

            myShoot.transform.position = new Vector3(transformPosition.x, transformPosition.y, transformPosition.z);
            myShoot.transform.rotation = new Quaternion(transformRotation.x, transformRotation.y, transformRotation.z, transformRotation.w);
        }
    }
}
