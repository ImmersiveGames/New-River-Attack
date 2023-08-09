using UnityEngine;
namespace RiverAttack
{
    public class EnemiesAnimator : MonoBehaviour
    {
        public string explosionTrigger;
        public string onMove;
        public string onFlip;

        ObstacleMaster m_ObstacleMaster;
        protected Animator animator;
        GamePlayManager m_GamePlayManager;

        #region UNITY METHODS
        protected virtual void OnEnable()
        {
            SetInitialReferences();
            m_ObstacleMaster.EventDestroyObject += ExplodeAnimation;
            m_ObstacleMaster.EventObjectMasterMovement += MovementAnimation;
            m_ObstacleMaster.EventObjectMasterFlipEnemies += ObjectMasterFlipObstaclesMasterFlipAnimation;
            m_GamePlayManager.EventResetEnemies += ResetAnimation;
        }
        protected virtual void OnDisable()
        {
            m_ObstacleMaster.EventDestroyObject -= ExplodeAnimation;
            m_ObstacleMaster.EventObjectMasterMovement -= MovementAnimation;
            m_ObstacleMaster.EventObjectMasterFlipEnemies -= ObjectMasterFlipObstaclesMasterFlipAnimation;
            m_GamePlayManager.EventResetEnemies -= ResetAnimation;
        }
  #endregion

        protected virtual void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_ObstacleMaster = GetComponent<ObstacleMaster>();
            animator = GetComponentInChildren<Animator>();
        }

        void MovementAnimation(Vector3 pos)
        {
            if (animator == null)
            {
                animator = GetComponentInChildren<Animator>();
            }
            if (animator == null || string.IsNullOrEmpty(onMove) || !animator.gameObject.activeSelf)
                return;
            animator.SetBool(onMove, pos != Vector3.zero);
        }

        void ObjectMasterFlipObstaclesMasterFlipAnimation(Vector3 face)
        {
            if (animator == null)
            {
                animator = GetComponentInChildren<Animator>();
            }
            if (animator != null && !string.IsNullOrEmpty(onFlip))
                animator.SetBool(onFlip, !animator.GetBool(onFlip));
        }

        internal virtual void ResetAnimation()
        {
            if (animator == null) return;
            if(!string.IsNullOrEmpty(onMove))
                animator.SetBool(onMove, false);
            if(!string.IsNullOrEmpty(onFlip))
                animator.SetBool(onFlip, false);
        }

        void ExplodeAnimation()
        {
            if (animator != null && !string.IsNullOrEmpty(explosionTrigger))
            {
                animator.SetBool(explosionTrigger, true);
            }
        }
        protected void RemoveAnimation()
        {
            if (animator != null && GetComponent<SpriteRenderer>())
                GetComponent<SpriteRenderer>().enabled = false;
            if (animator != null && GetComponent<MeshRenderer>())
                GetComponent<MeshRenderer>().enabled = false;
            if (transform.childCount <= 0) return;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
