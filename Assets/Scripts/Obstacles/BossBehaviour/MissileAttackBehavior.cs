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
        
        
        //Shoot Variaveis
        float m_StartCadence;
        float m_StartBulletSpeed;
        float m_BulletSpeed;
        float m_Cadence;
        float m_BulletLifeTime;
        Transform m_SpawnPoint;

        readonly IHasPool m_MyPool;
        
        Transform m_Target;
        Vector3 bossPosition;
        
        //IBossBehavior
        bool m_Finished;
        BossMaster m_BossMaster;
        BossShoot m_BossShoot;
        internal MissileAttackBehavior(BossMaster bossMaster)
        {
            m_BossMaster = bossMaster;
            m_MyPool = m_BossShoot = m_BossMaster.GetBossShoot();
        }
        public void Enter()
        {
            Debug.Log("Entrando no comportamento MissileAttack");
            m_StartCadence = m_Cadence = m_BossShoot.shootCadence;
            m_StartBulletSpeed = m_BossShoot.bulletSpeed;
            m_BulletLifeTime = m_BossShoot.bulletLifeTime;
            m_SpawnPoint = m_BossShoot.spawnPoint;
            m_BulletSpeed = m_StartBulletSpeed;
            m_Target = m_BossMaster.targetPlayer;
        }
        public void Update()
        {
            Debug.Log("Atualizando comportamento MissileAttack");
            m_Cadence -= Time.deltaTime;
            if (!(m_Cadence <= 0.01f))
                return;
            m_Cadence = m_StartCadence;
            Fire();
        }
        public void Exit()
        {
            Debug.Log("Saindo do comportamento MissileAttack");
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
            
            bossPosition = m_BossMaster.transform.position;
            var targetTransform = m_Target.transform.position;
            
            var myShoot = PoolObjectManager.GetObject(m_MyPool);
            //setting bullet entity
            var myBullet = myShoot.GetComponent<BulletBoss>();
            myBullet.SetMyPool(PoolObjectManager.GetPool(m_MyPool));
            myBullet.ownerShoot = m_BossMaster;
            myBullet.Init(m_BulletSpeed, m_BulletLifeTime);
            myBullet.moveDirection = targetTransform;
            //Deattached bullet
            var transformPosition = m_SpawnPoint.position;
            var transformRotation = m_SpawnPoint.rotation;

            myShoot.transform.position = new Vector3(transformPosition.x, transformPosition.y, transformPosition.z);
            myShoot.transform.rotation = new Quaternion(transformRotation.x, transformRotation.y, transformRotation.z, transformRotation.w);
        }
        /*
        
        float m_CountTime;
        const float TIME_CADENCE = .5f;
        float m_BulletLifeTime;

        readonly IHasPool m_MyPool;
        int startPool = numBullets * 3;
        Transform m_SpawnPoint;

        Vector3 bossPosition;
        bool m_Finished;
        BossMaster m_BossMaster;
        GameObject m_Bullets;
        Transform m_Target;
        internal MissileAttackBehavior(BossMaster bossMaster)
        {
            m_BossMaster = bossMaster;
            m_Bullets = m_BossMaster.GetBossShoot().GetBullets();
            m_MyPool = m_BossMaster.GetBossShoot();
        }
        public void Enter()
        {
            Debug.Log("Entrando no comportamento MissileAttack");
            // Lógica de entrada para o comportamento MissileAttack
            m_Finished = false;
            m_Target = m_BossMaster.targetPlayer;
            m_BulletLifeTime = m_BossMaster.GetBossShoot().bulletLifeTime;
            StartMyPool();
            spawnPoint = m_BossMaster.GetComponentInChildren<EnemiesShootSpawn>().transform ? m_BossMaster.GetComponentInChildren<EnemiesShootSpawn>().transform : m_BossMaster.transform;

        }
        public void Update()
        {
            Debug.Log("Atualizando comportamento MissileAttack");
            
            // Lógica de atualização para o comportamento MissileAttack
            
            bossPosition = m_BossMaster.transform.position;
            var targetTransform = m_Target.transform.position;
            m_CountTime += Time.deltaTime;

            if (!(m_CountTime >= TIME_CADENCE))
                return;
            m_CountTime = 0f;
            Shoot();
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
            
            
            if (GamePlayManager.instance.playerDead) return;
            //Debug.Log("Shoot!");
            var myShoot = PoolObjectManager.GetObject(m_MyPool);
            //setting bullet entity
            var myBullet = myShoot.GetComponent<BulletBoss>();
            myBullet.SetMyPool(PoolObjectManager.GetPool(m_MyPool));
            myBullet.ownerShoot = m_BossMaster;
            myBullet.Init(shootSpeed, m_BulletLifeTime);
            //Deattached bullet
            var transformPosition = m_SpawnPoint.position;
            var transformRotation = m_SpawnPoint.rotation;

            myShoot.transform.position = new Vector3(transformPosition.x, transformPosition.y, transformPosition.z);
            myShoot.transform.rotation = new Quaternion(transformRotation.x, transformRotation.y, transformRotation.z, transformRotation.w);
            
            
            

            if (m_Bullets != null && m_Target != null && coneAngle >= 15.0f && coneAngle <= 360.0f)
            {
                
                var targetDirection = (m_Target.position - bossPosition).normalized;
                var directions = ConeDirections(targetDirection, numBullets, coneAngle);
                /*Debug.Log($"Target: {targetDirection}");#1#
                Debug.Log($"Target: {directions}");

                for (int i = 0; i < directions.Length; i++)
                {
                    GameObject projetil = Object.Instantiate(m_Bullets, bossPosition, Quaternion.identity);
                    BulletBoss scriptProjetil = projetil.GetComponent<BulletBoss>();
                    
                    if (scriptProjetil != null)
                    {
                        scriptProjetil.Init(shootSpeed, 10f);
                        scriptProjetil.MoveShoot(directions[i]);
                    }
                }
            }
        }
        
        Vector3[] ConeDirections(Vector3 targetDirection, int numMissile, float angleCone)
        {
            Vector3[] directions = new Vector3[numMissile];

            float centerAngle = Mathf.Atan2(targetDirection.z, targetDirection.x) * Mathf.Rad2Deg;
            float initialAngle = centerAngle - angleCone / 2;
            float angleIncrease = angleCone / (numMissile - 1);
            Debug.Log($"centerAngle: {centerAngle}");
            Debug.Log($"initialAngle: {initialAngle}");
            Debug.Log($"angleIncrease: {angleIncrease}");

            for (int i = 0; i < numMissile; i++)
            {
                float angle = initialAngle + angleIncrease * i;
                Debug.Log($"angle: {angle}");
                float deg2Rad = angle * Mathf.Deg2Rad;

                var finalDirection = new Vector3(Mathf.Cos(deg2Rad), 0, Mathf.Sin(deg2Rad));
                Debug.Log($"finalDirection: {finalDirection}");
                directions[i] = finalDirection;
            }
            /*var directions = new Vector3[numMissile];

            float centerAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            float initialAngle = centerAngle - angleCone / 2;
            float angleIncrease = angleCone / (numMissile - 1);
            //Debug.Log($"centerAngle: {centerAngle}");
            //Debug.Log($"initialAngle: {initialAngle}");
            //Debug.Log($"angleIncrease: {angleIncrease}");
            for (int i = 0; i < numMissile; i++)
            {
                float angle = initialAngle + angleIncrease * i;
                //Debug.Log($"angle: {angle}");
                var finalDirection = Quaternion.Euler(0, 0, angle) * Vector3.right;
                //Debug.Log($"finalDirection: {finalDirection}");
                directions[i] = finalDirection;
                Debug.Log($"directions: {directions[i]}");
            }#1#
            
            return directions;
        }
        public void StartMyPool(bool isPersistent = false)
        {
            PoolObjectManager.CreatePool(this, m_Bullets, startPool, m_BossMaster.transform, isPersistent);
        }*/
    }
}
