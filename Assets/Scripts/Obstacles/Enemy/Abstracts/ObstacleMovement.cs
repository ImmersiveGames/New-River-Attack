﻿using UnityEngine;
using Utils;
namespace RiverAttack
{
    public abstract class ObstacleMovement : ObstacleDetectApproach, IMove
    {
        [SerializeField]
        protected internal bool canMove;
        protected enum MovementState { Paused, Moving, Patrolling }
        [SerializeField] protected MovementState obstacleMovementState;
        [SerializeField]
        protected internal bool canMoveByApproach;

        [SerializeField] internal float moveVelocity;
        public enum Directions { None, Up, Right, Down, Left, Forward, Backward, Free }
        [SerializeField]
        protected internal Directions directions;
        protected Directions startDirection;
        [SerializeField]
        protected internal Vector3 moveFreeDirection;
        protected Vector3 facingDirection;
        [Header("Motion with Animation Curve")]
        [SerializeField]
        private protected float animationDuration;
        float m_ElapsedTime = 0f;
        [SerializeField]
        protected internal AnimationCurve animationCurve;

        void SetInitialReferences(){}

        public void Move(Vector3 direction, float velocity)
        {
            transform.Translate(direction * (velocity * Time.deltaTime));
        }
        public void StopMove()
        {
            directions = Directions.None;
            obstacleMovementState = MovementState.Paused;
            facingDirection = SetDirection(directions);
        }

        public void StartMove()
        {
            directions = startDirection;
            obstacleMovementState = MovementState.Moving;
            facingDirection = SetDirection(directions);
        }
        public virtual void ResetMovement()
        {
            canMove = playerApproachRadius == 0 && playerApproachRadiusRandom == Vector2.zero;
            obstacleMovementState = MovementState.Paused;
            directions = startDirection;
            facingDirection = SetDirection(directions);
        }
        protected void MoveCurveAnimation(Vector3 direction, float velocity, float duration, AnimationCurve curve)
        {
            m_ElapsedTime += Time.deltaTime;

            // Verifica se a animação terminou e reinicia se necessário
            if (m_ElapsedTime >= duration)
            {
                m_ElapsedTime = 0.0f;
            }
            float curveFactor = Mathf.Clamp01(m_ElapsedTime / duration);

            // Usa a curva de animação para obter a interpolação de movimento
            float curveValue = curve.Evaluate(curveFactor);

            transform.Translate(direction * (curveValue * (velocity * Time.deltaTime)));
        }
        public bool ShouldMoving()
        {
            return obstacleMovementState != MovementState.Paused && obstacleMovementState != MovementState.Patrolling;
        }
        private protected Vector3 SetDirection(Directions dir)
        {
            return dir switch
            {
                Directions.Up => Vector3.up,
                Directions.Right => Vector3.right,
                Directions.Down => Vector3.down,
                Directions.Left => Vector3.left,
                Directions.Backward => Vector3.back,
                Directions.Forward => Vector3.forward,
                Directions.None => Vector3.zero,
                Directions.Free => moveFreeDirection,
                _ => Vector3.zero
            };
        }
    }
}