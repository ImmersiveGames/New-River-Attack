using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RiverAttack
{
    [RequireComponent(typeof(EnemiesMaster))]
    public sealed class EnemiesAnimator : MonoBehaviour
    {

        public string explosionTrigger;
        public string onMove;
        public string onFlip;

        EnemiesMaster m_EnemyMaster;
        Animator m_Animator;
        GamePlayManager m_GamePlayManager;

        void OnEnable()
        {
            SetInitialReferences();
            m_EnemyMaster.EventDestroyEnemy += ExplodeAnimation;
            m_EnemyMaster.EventMovementEnemy += MovementAnimation;
            m_EnemyMaster.EventFlipEnemy += FlipAnimation;
            m_EnemyMaster.EventChangeSkin += SetInitialReferences;
            m_GamePlayManager.EventResetEnemies += ResetAnimation;
        }


        void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_EnemyMaster = GetComponent<EnemiesMaster>();
            m_Animator = GetComponentInChildren<Animator>();
        }

        private void MovementAnimation(Vector3 pos)
        {
            if (m_Animator == null || string.IsNullOrEmpty(onMove) || !m_Animator.gameObject.activeSelf)
                return;
            m_Animator.SetBool(onMove, pos.x != 0);
            //animator.SetFloat(MovimentFloat, pos.x * 10);
        }

        private void FlipAnimation(Vector3 face)
        {
            if (m_Animator != null && !string.IsNullOrEmpty(onFlip))
                m_Animator.SetBool(onFlip, !m_Animator.GetBool(onFlip));
        }

        private void ResetAnimation()
        {
            if (m_Animator != null && !string.IsNullOrEmpty(onMove))
                m_Animator.SetBool(onMove, false);
            if (m_Animator != null && !string.IsNullOrEmpty(onFlip))
                m_Animator.SetBool(onFlip, false);
        }

        void ExplodeAnimation()
        {
            if (m_Animator != null && !string.IsNullOrEmpty(explosionTrigger))
            {
                m_Animator.SetBool(explosionTrigger, true);
            }
        }
        public void RemoveAnimation()
        {
            if (m_Animator != null && GetComponent<SpriteRenderer>())
                GetComponent<SpriteRenderer>().enabled = false;
            if (m_Animator != null && GetComponent<MeshRenderer>())
                GetComponent<MeshRenderer>().enabled = false;
            if (transform.childCount > 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }

        void OnDisable()
        {
            m_EnemyMaster.EventDestroyEnemy -= ExplodeAnimation;
            m_EnemyMaster.EventMovementEnemy -= MovementAnimation;
            m_EnemyMaster.EventFlipEnemy -= FlipAnimation;
            m_EnemyMaster.EventChangeSkin -= SetInitialReferences;
            m_GamePlayManager.EventResetEnemies -= ResetAnimation;

        }
    }

}
