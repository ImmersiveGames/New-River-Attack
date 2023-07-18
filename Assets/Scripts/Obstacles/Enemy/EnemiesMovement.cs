using UnityEngine;

namespace RiverAttack
{
    public class EnemiesMovement : ObstacleMovement
    {
        private bool m_AlreadyCol;

        #region UnityMethods
        void OnTriggerEnter(Collider other)
        {
            if (!m_AlreadyCol && !enemyMaster.ignoreWall && other.GetComponentInParent<WallsMaster>())
            {
                FlipMe();
            }
        }
        void LateUpdate()
        {
            m_AlreadyCol = false;
        }
  #endregion
        protected override void MoveEnemy()
        {
            base.MoveEnemy();
            if (enemyMovment != Vector3.zero)
            {
                enemyMaster.CallEventMovementEnemy(enemyMovment);
            }
        }
        private void FlipMe()
        {
            m_AlreadyCol = true;
            if (faceDirection.x != 0)
                faceDirection.x *= -1;
            if (faceDirection.y != 0)
                faceDirection.y *= -1;
            if (faceDirection.z != 0)
                faceDirection.z *= -1;
            if (enemy.canFlip)
                enemyMaster.CallEventFlipEnemy(faceDirection);
        }
    }
}
