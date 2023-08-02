using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RiverAttack
{
    [RequireComponent(typeof(EnemiesMaster))]
    public class EnemiesAnimator : MonoBehaviour
    {
        public string explosionTrigger;
        public string onMove;
        public string onFlip;

        EnemiesMaster m_EnemyMaster;
        Animator m_Animator;
        GamePlayManager m_GamePlayManager;

        #region UNITY METHODS
        protected virtual void OnEnable()
        {
            SetInitialReferences();
            m_EnemyMaster.EventDestroyEnemy += ExplodeAnimation;
            m_EnemyMaster.EventEnemiesMasterMovement += MovementAnimation;
            m_EnemyMaster.EventEnemiesMasterFlipEnemies += EnemiesMasterFlipAnimation;
            m_GamePlayManager.EventResetEnemies += ResetAnimation;
        }
        protected virtual void OnDisable()
        {
            m_EnemyMaster.EventDestroyEnemy -= ExplodeAnimation;
            m_EnemyMaster.EventEnemiesMasterMovement -= MovementAnimation;
            m_EnemyMaster.EventEnemiesMasterFlipEnemies -= EnemiesMasterFlipAnimation;
            m_GamePlayManager.EventResetEnemies -= ResetAnimation;
        }
  #endregion

        protected virtual void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_EnemyMaster = GetComponent<EnemiesMaster>();
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

        void EnemiesMasterFlipAnimation(Vector3 face)
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
            if(!string.IsNullOrEmpty(onMove))
                m_Animator.SetBool(onMove, false);
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
