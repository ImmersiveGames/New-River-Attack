using UnityEngine;
using Utils;

namespace RiverAttack
{
    public class StateShoot : IShoot
    {
        float m_Cadence;
        float m_BulletSpeed;
        readonly float m_StartCadence;
        readonly float m_StartBulletSpeed;
        readonly float m_BulletLifeTime;
        readonly Transform m_SpawnPoint;
        EnemiesMaster m_EnemiesMaster;
        EnemiesSetDifficulty m_EnemiesSetDifficulty;
        readonly IHasPool m_MyPool;

        public StateShoot(EnemiesShoot enemiesShoot)
        {
            m_StartCadence = m_Cadence = enemiesShoot.shootCadence;
            m_StartBulletSpeed = enemiesShoot.bulletSpeed;
            m_BulletLifeTime = enemiesShoot.bulletLifeTime;
            m_MyPool = enemiesShoot;
            m_SpawnPoint = enemiesShoot.spawnPoint;
        }
        
        public void EnterState(EnemiesMaster enemyMaster)
        {
            //Debug.Log("Estado: Shoot - Entrando");
            m_EnemiesMaster = enemyMaster;
            if (!m_EnemiesMaster.enemy && !m_EnemiesMaster.enemy.enemiesSetDifficultyListSo) return;
            m_EnemiesSetDifficulty = m_EnemiesMaster.enemy.enemiesSetDifficultyListSo.GetDifficultByEnemyDifficult(m_EnemiesMaster.getDifficultName);
            m_Cadence = m_StartCadence * m_EnemiesSetDifficulty.multiplyEnemiesShootCadence;
            m_BulletSpeed = m_StartBulletSpeed * m_EnemiesSetDifficulty.multiplyEnemiesShootSpeedy;
        }
        public void UpdateState()
        {
            //Debug.Log("Attempt Shoot!");
            m_Cadence -= Time.deltaTime;
            if (!(m_Cadence <= 0))
                return;
            m_Cadence = m_StartCadence;
            Fire();
        }
        public void ExitState()
        {
            //Debug.Log("Estado: Shoot - Saindo");
            // Coloque aqui as ações a serem executadas ao sair do estado atirar
        }
        public void Fire()
        {
            //Debug.Log("Shoot!");
            var myShoot = PoolObjectManager.GetObject(m_MyPool);
            //setting bullet entity
            var myBullet = myShoot.GetComponent<BulletEnemy>();
            myBullet.SetMyPool(PoolObjectManager.GetPool(m_MyPool));
            myBullet.ownerShoot = m_EnemiesMaster;
            myBullet.Init(m_BulletSpeed, m_BulletLifeTime);
            //Deattached bullet
            var transformPosition = m_SpawnPoint.position;
            var transformRotation = m_SpawnPoint.rotation;
   
            myShoot.transform.position = new Vector3(transformPosition.x, transformPosition.y, transformPosition.z);
            myShoot.transform.rotation = new Quaternion(transformRotation.x, transformRotation.y, transformRotation.z, transformRotation.w);
        }
    }
}
