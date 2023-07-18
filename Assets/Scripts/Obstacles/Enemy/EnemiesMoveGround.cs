using UnityEngine;
namespace RiverAttack
{
    public class EnemiesMoveGround : ObstacleMove
    {
        EnemiesMaster m_EnemyMaster;
        EnemiesShoot m_EnemiesShoot;
        GamePlayManager m_GamePlayManager;
        bool m_AlreadyCol;

        void OnEnable()
        {
            SetInitialReferences();
            direction = (direction == Vector3.zero) ? Vector3.forward : direction;
            canMove = true;
            m_GamePlayManager.EventResetEnemies -= ResetMovement;
        }

        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            m_EnemyMaster = GetComponent<EnemiesMaster>();
            m_GamePlayManager = GamePlayManager.instance;
            m_EnemiesShoot = GetComponent<EnemiesShoot>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (m_AlreadyCol || m_EnemyMaster.ignoreWall || !other.GetComponentInParent<WallsMaster>()) return;
            m_AlreadyCol = true;
            MoveStop();
            if (m_EnemiesShoot != null && m_EnemiesShoot.holdShoot) m_EnemiesShoot.holdShoot = false;
        }

        void Update()
        {
            Move(direction);
        }

        public override bool ShouldMove()
        {
            bool should = base.ShouldMove();
            if (!should || !GamePlayManager.instance.shouldBePlayingGame || m_EnemyMaster.isDestroyed)
                should = false;
            return should;
        }

        void ResetMovement()
        {
            m_AlreadyCol = false;
            if (m_EnemiesShoot)
            {
                m_EnemiesShoot.holdShoot = true;
            }
        }
        void LateUpdate()
        {
            m_AlreadyCol = false;
        }
        void OnDisable()
        {
            m_GamePlayManager.EventResetEnemies -= ResetMovement;
        }
    }
}
