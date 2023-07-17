using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RiverAttack
{
    
    public class EnemyShootApproach : ObstacleDetectApproach
    {
        [SerializeField]
        private float startTime;
        [SerializeField, Range(.1f, 5)]
        public float timeToCheck;
        private EnemyShoot enemyShoot;
        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            enemyShoot = GetComponent<EnemyShoot>();
        }
        private void Start()
        {
            InvokeRepeating("DetectPlayer", startTime, timeToCheck);
        }

        private void DetectPlayer()
        {
            Collider[] col = ApproachPlayer(LayerMask.GetMask("Player"));
            if (col.Length > 0)
            {
                if (enemyShoot.playerTarget)
                    enemyShoot.SetTarget(col[0].transform);
                enemyShoot.Fire();
            }
        }
    }
}
