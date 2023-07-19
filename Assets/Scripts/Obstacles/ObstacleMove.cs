using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
namespace RiverAttack
{
    public abstract class ObstacleMove : MonoBehaviour, IMove
    {
        [SerializeField]
        private float moveSpeed;
        protected Vector3 direction;
        [SerializeField]
        protected bool isMove;
        [SerializeField]
        protected bool canMove;
        [SerializeField]
        private string parameterAnimationMove;

        protected Vector3 directionVector3;
        Animator m_Animator;

        protected virtual void SetInitialReferences()
        {
            m_Animator = GetComponent<Animator>();
        }
        public void Move(Vector3 directionV3)
        {
            if (!canMove) return;
            if (!isMove) isMove = true;
            AnimateOnMove(directionV3);
            transform.Translate(directionV3 * (moveSpeed * Time.deltaTime));
        }
        protected void MoveStop()
        {
            isMove = false;
            canMove = false;
            m_Animator.SetBool(parameterAnimationMove, false);
        }

        public void SetCanMove(bool can)
        {
            canMove = can;
        }
        public virtual bool ShouldMove()
        {
            return canMove;
        }

        protected virtual void AnimateOnMove(Vector3 pos)
        {
            if (m_Animator != null && !string.IsNullOrEmpty(parameterAnimationMove))
            {
                m_Animator.SetBool(parameterAnimationMove, pos != Vector3.zero);
            }
        }
    }
}
