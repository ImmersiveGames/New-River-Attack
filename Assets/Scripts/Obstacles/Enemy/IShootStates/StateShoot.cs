using UnityEngine;
using Utils;

namespace RiverAttack
{
    public class StateShoot : IShoot
    {
        private IShoot m_ShootImplementation;
        private float m_Cadence;
        private float m_BulletSpeed;
        private float m_StartCadence;
        private float m_StartBulletSpeed;
        private float m_BulletLifeTime;
        private Transform m_SpawnPoint;
        private readonly EnemiesShoot m_EnemiesShoot;
        private EnemiesMaster m_EnemiesMaster;
        private EnemiesSetDifficulty m_EnemiesSetDifficulty;
        private readonly IHasPool m_MyPool;

        public StateShoot(EnemiesShoot enemiesShoot)
        {
            m_EnemiesShoot = enemiesShoot;
            m_MyPool = enemiesShoot;
        }

        public void EnterState(ObstacleMaster enemyMaster)
        {
            m_StartCadence = m_Cadence = m_EnemiesShoot.shootCadence;
            m_StartBulletSpeed = m_EnemiesShoot.bulletSpeed;
            m_BulletLifeTime = m_EnemiesShoot.bulletLifeTime;
            m_SpawnPoint = m_EnemiesShoot.spawnPoint;

            //Debug.Log("Estado: Shoot - Entrando" + m_EnemiesShoot.shootCadence);
            m_EnemiesMaster = enemyMaster as EnemiesMaster;
            if (m_EnemiesMaster != null && !m_EnemiesMaster.enemy && !m_EnemiesMaster.enemy.enemiesSetDifficultyListSo) return;
            if (m_EnemiesMaster != null)
                m_EnemiesSetDifficulty = m_EnemiesMaster.enemy.enemiesSetDifficultyListSo.GetDifficultByEnemyDifficult(m_EnemiesMaster.actualDifficultName);
            m_Cadence = m_StartCadence * m_EnemiesSetDifficulty.multiplyEnemiesShootCadence;
            m_BulletSpeed = m_StartBulletSpeed * m_EnemiesSetDifficulty.multiplyEnemiesShootSpeedy;
        }
        public void UpdateState()
        {
            // Debug.Log("Attempt Shoot!" + m_Cadence);
            if (m_SpawnPoint == null)
            {
                
            }
            m_Cadence -= Time.deltaTime;
            if (!(m_Cadence <= 0.01f))
                return;
            m_Cadence = m_StartCadence;
            Fire();
        }
        public void ExitState()
        {
            //Debug.Log("Estado: Shoot - Saindo");
            // Coloque aqui as ações a serem executadas ao sair do estado atirar
        }

        private void Fire()
        {
            if (GamePlayManager.instance.playerDead) return;
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
