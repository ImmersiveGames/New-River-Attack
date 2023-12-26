using System;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;
namespace RiverAttack
{
    public class MinesDetonator : ObstacleDetectApproach
    {
        IShoot m_ActualState;
        readonly StateDetonate m_StateDetonate;
        readonly StateShootHold m_StateShootHold;
        readonly StateShootPatrol m_StateShootPatrol;
        MineMaster m_MineMaster;
        GamePlayManager m_GamePlayManager;
        
        public MinesDetonator()
        {
            m_StateDetonate = new StateDetonate(this);
            m_StateShootHold = new StateShootHold();
            m_StateShootPatrol = new StateShootPatrol(this, null);
        }
        void OnEnable()
        {
            SetInitialReferences();
        }
        void Update()
        {
            if (!m_GamePlayManager.shouldBePlayingGame || !m_MineMaster.shouldObstacleBeReady || m_MineMaster.isDestroyed || !meshRenderer.isVisible)
                return;
            switch (shouldBeExplode)
            {
                case true when shouldBeApproach && !target:
                    target = null;
                    ChangeState(m_StateShootPatrol);
                    target = m_StateShootPatrol.target;
                    break;
                case true:
                    ChangeState(m_StateDetonate);
                    break;
                case false:
                    target = null;
                    ChangeState(m_StateShootHold);
                    break;
            }
            m_ActualState.UpdateState();
        }
        

        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            m_GamePlayManager = GamePlayManager.instance;
            m_MineMaster = GetComponent<MineMaster>();
        }
        
        void ChangeState(IShoot newState)
        {
            if (m_ActualState == newState) return;
            m_ActualState?.ExitState();

            m_ActualState = newState;
            m_ActualState?.EnterState(m_MineMaster);
        }


        bool shouldBeExplode { get { return m_MineMaster.isActive; } }
        
         #region Gizmos
        void OnDrawGizmosSelected()
        {
        #if UNITY_EDITOR
            
            if (playerApproachRadius <= 0 && playerApproachRadiusRandom.y <= 0) return;
            float realApproachRadius  = playerApproachRadiusRandom != Vector2.zero ? Random.Range(playerApproachRadiusRandom.x, playerApproachRadiusRandom.y) : playerApproachRadius;
            var mineMaster = GetComponent<MineMaster>();
            var position = transform.position;

            // Código que será executado apenas no Editor
            if (playerApproachRadiusRandom == Vector2.zero)
            {
                Gizmos.color = gizmoColor;
                Gizmos.DrawWireSphere(center: position, realApproachRadius);
            }
            if(playerApproachRadiusRandom == Vector2.zero) return;
            Gizmos.color = gizmoColor + new Color(0.2f, 0.2f, 0.2f);
            Gizmos.DrawWireSphere(center: position, playerApproachRadiusRandom.x);
            Gizmos.color = gizmoColor - new Color(0.2f, 0.2f, 0.2f);
            Gizmos.DrawWireSphere(center: position, playerApproachRadiusRandom.y);
        #endif
        }
  #endregion
    }
}
