﻿using GD.MinMaxSlider;
using UnityEngine;
using Utils;
namespace RiverAttack
{
    public abstract class ObstacleMovement : MonoBehaviour,IMove
    {
        [SerializeField]
        protected internal bool canMove;
        [SerializeField] bool isMoving;
        
        [SerializeField] internal float moveVelocity;
        [SerializeField]
        internal Vector3 moveDirection;
        public enum Directions { None, Up, Right, Down, Left, Forward, Backward, Free }
        [SerializeField]
        protected internal Directions directions;
        [SerializeField]
        protected internal Vector3 moveFreeDirection;
        protected Vector3 facingDirection;
        [Header("Motion with Animation Curve")]
        [SerializeField]
        private protected float animationDuration;
        float m_ElapsedTime = 0f;
        [SerializeField]
        protected internal AnimationCurve animationCurve;
        [Header("Start Move By Player Approach")]
        [SerializeField] protected internal float playerApproachRadius;
        [SerializeField, Range(.1f, 5)] public float timeToCheck = 2f;
        [SerializeField, MinMaxSlider(0f,10f)] protected internal Vector2 playerApproachRadiusRandom;

        #region GizmoSettings
        [Header("Gizmo Settings")]
        [SerializeField]
        Color gizmoColor = new Color(255, 0, 0, 150);
        #endregion
        
        public void Move(Vector3 direction, float velocity)
        {
            isMoving = true;
            transform.Translate(direction * (velocity * Time.deltaTime));
        }
        public void StopMove()
        {
            isMoving = false;
            directions = Directions.None;
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
            return canMove;
        }

        public void ResetMovement()
        {
            directions = Directions.None;
            facingDirection = SetDirection(directions);
            canMove = playerApproachRadius != 0 || playerApproachRadiusRandom != Vector2.zero;
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
        void OnDrawGizmosSelected()
        {
            if (playerApproachRadius <= 0 && playerApproachRadiusRandom.y <= 0) return;
            float radius = playerApproachRadius;
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(center: transform.position, radius);
        }
    }
}
