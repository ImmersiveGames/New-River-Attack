using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RiverAttack
{
    public class EnemiesAnimator : MonoBehaviour
    {
        public string explosionTrigger;
        public string onMove;
        public string onFlip;

        ObstacleMaster m_ObstacleMaster;
        Animator m_Animator;
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
            m_Animator = GetComponentInChildren<Animator>();
        }

        void MovementAnimation(Vector3 pos)
        {
            if (m_Animator == null)
            {
                m_Animator = GetComponentInChildren<Animator>();
            }
            if (m_Animator == null || string.IsNullOrEmpty(onMove) || !m_Animator.gameObject.activeSelf)
                return;
            m_Animator.SetBool(onMove, pos != Vector3.zero);
        }

        void ObjectMasterFlipObstaclesMasterFlipAnimation(Vector3 face)
        {
            if (m_Animator == null)
            {
                m_Animator = GetComponentInChildren<Animator>();
            }
            if (m_Animator != null && !string.IsNullOrEmpty(onFlip))
                m_Animator.SetBool(onFlip, !m_Animator.GetBool(onFlip));
        }

        void ResetAnimation()
        {
            if (m_Animator == null) return;
            if(!string.IsNullOrEmpty(onMove))
                m_Animator.SetBool(onMove, false);
            if(!string.IsNullOrEmpty(onFlip))
                m_Animator.SetBool(onFlip, false);
        }

        void ExplodeAnimation()
        {
            if (m_Animator != null && !string.IsNullOrEmpty(explosionTrigger))
            {
                m_Animator.SetBool(explosionTrigger, true);
            }
        }
        protected void RemoveAnimation()
        {
            if (m_Animator != null && GetComponent<SpriteRenderer>())
                GetComponent<SpriteRenderer>().enabled = false;
            if (m_Animator != null && GetComponent<MeshRenderer>())
                GetComponent<MeshRenderer>().enabled = false;
            if (transform.childCount <= 0) return;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
