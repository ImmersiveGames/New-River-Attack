using UnityEngine;
namespace RiverAttack
{
    public class EnemiesAnimator : MonoBehaviour
    {
        public string onMove;
        public string onFlip;

        private EnemiesMaster m_EnemiesMaster;
        private Animator m_Animator;
        private GamePlayManager m_GamePlayManager;

        #region UNITY METHODS

        private void OnEnable()
        {
            SetInitialReferences();
            if (m_EnemiesMaster)
            {
                m_EnemiesMaster.EventObstacleMovement += AnimationMove; 
                m_EnemiesMaster.EventObjectMasterFlipEnemies += AnimationFlip;
            }
            m_GamePlayManager.EventReSpawnEnemiesMaster += ResetAnimation;

        }

        private void OnDisable()
        {
            if (!m_EnemiesMaster) return;
            m_EnemiesMaster.EventObstacleMovement -= AnimationMove;
            m_EnemiesMaster.EventObjectMasterFlipEnemies -= AnimationFlip;
        }

        private void OnDestroy()
        {
            m_GamePlayManager.EventReSpawnEnemiesMaster -= ResetAnimation;
        }
  #endregion

  private void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_EnemiesMaster = GetComponent<EnemiesMaster>();
            m_Animator = GetComponentInChildren<Animator>();
        }

        private void AnimationMove(bool active)
        {
            if (m_Animator == null)
                m_Animator = GetComponentInChildren<Animator>();
            if (string.IsNullOrEmpty(onMove)) 
                return;
            m_Animator.SetBool(onMove, active);
        }

        private void AnimationFlip(bool active)
        {
            if (m_Animator == null)
                m_Animator = GetComponentInChildren<Animator>();
            if (string.IsNullOrEmpty(onFlip)) 
                return;
            m_Animator.SetBool(onFlip, !m_Animator.GetBool(onFlip));
        }

        private void ResetAnimation()
        {
            if (m_Animator == null) return;
            if (!string.IsNullOrEmpty(onMove))
                m_Animator.SetBool(onMove, false);
            if (!string.IsNullOrEmpty(onFlip))
                m_Animator.SetBool(onFlip, false);
        }
    }
}
