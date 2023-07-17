using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class EnemyShoot : ObstacleShoot, IHasPool
    {
        public int startpool;
        public bool playerTarget;
    
        //private EnemySkinParts skinPart;
        private Transform spawnPosition;
        private PoolObjectManager myPool;
    
        public Transform Target { get; private set; }
    
        protected override void OnEnable()
        {
            base.OnEnable();
           // skinPart = GetComponentInChildren<EnemySkinParts>();
            myPool = PoolObjectManager.instance;
            if (!GetComponent<ObstacleDetectApproach>().enabled)
                InvokeRepeating("StartFire", 1, cadencyShoot);
        }
        private void Start()
        {
            SetTarget(GamePlayManager.instance.GetPlayer(0).transform);
            spawnPosition = GetComponentInChildren<EnemyShootSpawn>().transform;
            StartMyPool();
        }
        public new void Fire()
        {
            GameObject bullet = myPool.GetObject(this);
            bullet.transform.position = spawnPosition.position;
            bullet.transform.rotation = spawnPosition.rotation;
            BulletEnemy enemyBullet = bullet.GetComponent<BulletEnemy>();
            enemyBullet.shootDirection = Vector3.forward;
            enemyBullet.shootVelocity = bulletSpeedy;
            if (playerTarget)
                enemyBullet.transform.LookAt(Target);
        }
    
        public new void SetTarget(Transform toTarget)
        {
            Target = (playerTarget) ? toTarget : null;
        }
    
        private void StartFire()
        {
            if (ShouldFire())
            {
                Fire();
            }
        }
    
        private void FixedUpdate()
        {
            //if (skinPart != null && playerTarget)
                //skinPart.transform.rotation = Quaternion.Lerp(skinPart.transform.rotation, Quaternion.LookRotation(Target.position - skinPart.transform.position), Time.deltaTime);
        }
    
        public void StartMyPool(bool isPersistent = false)
        {
            myPool.CreatePool(this, prefab, startpool, spawnPosition, isPersistent);
        }
    }

}

