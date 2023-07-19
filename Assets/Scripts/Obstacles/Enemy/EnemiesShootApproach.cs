using UnityEngine;
namespace RiverAttack
{
    public class EnemiesShootApproach : ObstacleDetectApproach
    {
        [SerializeField]
        float startTime;
        [SerializeField, Range(.1f, 5)]
        public float timeToCheck;
        
        EnemiesShoot m_EnemiesShoot;

        #region UNITY METHODS
        void Start()
        {
            InvokeRepeating(nameof(DetectPlayer), startTime, timeToCheck);
        }
  #endregion
        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            m_EnemiesShoot = GetComponent<EnemiesShoot>();
        }
        void DetectPlayer()
        {
            var colliders = ApproachPlayer(GameSettings.instance.layerPlayer);
            if (colliders.Length <= 0) return;
            if (m_EnemiesShoot.playerTarget)
                m_EnemiesShoot.SetTarget(colliders[0].transform);
            m_EnemiesShoot.Fire();
        }
    }
}
