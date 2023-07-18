using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RiverAttack
{
    public class EnemiesShootApproach : ObstacleDetectApproach
    {
        [SerializeField]
        private float startTime;
        [SerializeField, Range(.1f, 5)]
        public float timeToCheck;
        private EnemiesShoot m_EnemiesShoot;
        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            m_EnemiesShoot = GetComponent<EnemiesShoot>();
        }
        private void Start()
        {
            InvokeRepeating(nameof(DetectPlayer), startTime, timeToCheck);
        }
        private void DetectPlayer()
        {
            var col = ApproachPlayer(GameSettings.instance.layerPlayer);
            if (col.Length <= 0) return;
            if (m_EnemiesShoot.playerTarget)
                m_EnemiesShoot.SetTarget(col[0].transform);
            m_EnemiesShoot.Fire();
        }
    }
}
