using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private string parmMoveAnimation;
    
    protected Vector3 _direction;
    protected Animator animator;
    
    protected virtual void SetInitialReferences()
    {
        animator = GetComponent<Animator>();
    }
    public void Move(Vector3 direction)
    {
        if (ShouldMove())
        {
            if (!isMove) isMove = true;
            AnimateOnMove(direction);
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
    }

    public void MoveStop()
    {
        isMove = false;
        canMove = false;
        animator.SetBool(parmMoveAnimation, false);
    }

    public void CanMove(bool can)
    {
        canMove = can;
    }
    public virtual bool ShouldMove()
    {
        if (!canMove)
        {
            return false;
        }
        return true;
    }

    protected virtual void AnimateOnMove(Vector3 pos)
    {
        if (animator != null && !string.IsNullOrEmpty(parmMoveAnimation))
        {
            if (pos != Vector3.zero)
                animator.SetBool(parmMoveAnimation, true);
            else
                animator.SetBool(parmMoveAnimation, false);
        }
    }
}
}
