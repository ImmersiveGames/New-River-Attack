/*
using UnityEngine;
namespace RiverAttack
{
    public class EnemiesMoveGround : ObstacleMove
    {
        EnemiesMaster m_EnemyMaster;
        EnemiesShoot m_EnemiesShoot;
        GamePlayManager m_GamePlayManager;
        bool m_AlreadyCollided;

        #region UNITY METHODS
        void OnEnable()
        {
            SetInitialReferences();
            direction = (direction == Vector3.zero) ? Vector3.forward : direction;
            canMove = true;
            m_GamePlayManager.EventResetEnemies -= ResetMovement;
        }
        void OnTriggerEnter(Collider other)
        {
            if (m_AlreadyCollided || m_EnemyMaster.ignoreWall || !other.GetComponentInParent<WallsMaster>()) return;
            m_AlreadyCollided = true;
            MoveStop();
            if (m_EnemiesShoot != null && m_EnemiesShoot.holdShoot) m_EnemiesShoot.holdShoot = false;
        }

        void Update()
        {
            Move(direction);
        }
        void LateUpdate()
        {
            m_AlreadyCollided = false;
        }
        void OnDisable()
        {
            m_GamePlayManager.EventResetEnemies -= ResetMovement;
        }
  #endregion
        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            m_EnemyMaster = GetComponent<EnemiesMaster>();
            m_GamePlayManager = GamePlayManager.instance;
            m_EnemiesShoot = GetComponent<EnemiesShoot>();
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
            m_AlreadyCollided = false;
            if (m_EnemiesShoot)
            {
                m_EnemiesShoot.holdShoot = true;
            }
        }
    }
}
*/
