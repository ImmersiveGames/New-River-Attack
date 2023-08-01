using UnityEngine;
using Utils;

namespace RiverAttack
{
    public class StateShoot : IShoot
    {
        float m_Cadence;
        float m_StartCadence;
        float m_BulletSpeed;
        float m_BulletLifeTime;
        Transform m_SpawnPoint;
   
        public void SetBullet(float cadence, float bulletSpeed, float bulletLifeTime)
        {
            m_StartCadence = m_Cadence = cadence;
            m_BulletSpeed = bulletSpeed;
            m_BulletLifeTime = bulletLifeTime;
        }
        public void SetSpawnPoint(Transform spawnPoint)
        {
            m_SpawnPoint = spawnPoint;
        }
    
        public void EnterState()
        {
            Debug.Log("Estado: Shoot - Entrando");
            // Coloque aqui as ações a serem executadas ao entrar no estado atirar
        }
        public void UpdateState(IHasPool myPool, EnemiesMaster master)
        {
            //Debug.Log("Attempt Shoot!");
            m_Cadence -= Time.deltaTime;
            if (!(m_Cadence <= 0))
                return;
            m_Cadence = m_StartCadence;
            Fire(myPool, master);

        }
        public void ExitState()
        {
            Debug.Log("Estado: Shoot - Saindo");
            // Coloque aqui as ações a serem executadas ao sair do estado atirar
        }
        public void Fire(IHasPool myPool, EnemiesMaster master)
        {
            Debug.Log("Shoot!");
            var myShoot = PoolObjectManager.GetObject(myPool);
            //setting bullet entity
            var myBullet = myShoot.GetComponent<BulletEnemy>();
            myBullet.SetMyPool(PoolObjectManager.GetPool(myPool));
            myBullet.ownerShoot = master;
            myBullet.Init(m_BulletSpeed, m_BulletLifeTime);
            //Deattached bullet
            var myShootTransform = m_SpawnPoint;
            var transformPosition = myShootTransform.position;
            var transformRotation = myShootTransform.rotation;
   
            myShoot.transform.position = new Vector3(transformPosition.x, transformPosition.y, transformPosition.z);
            myShoot.transform.rotation = new Quaternion(transformRotation.x, transformRotation.y, transformRotation.z, transformRotation.w);
        }
    }
}
