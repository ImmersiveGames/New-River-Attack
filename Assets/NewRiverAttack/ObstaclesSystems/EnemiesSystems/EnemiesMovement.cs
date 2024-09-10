using GD.MinMaxSlider;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.MovementStates;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;
using NewRiverAttack.WallsManagers;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.EnemiesSystems
{
    public sealed class EnemiesMovement : ObstacleMovement
    {
        
        private EnemiesMaster _enemiesMaster;
        private EnemiesAnimation _enemiesAnimation;
        private EnemiesScriptable GetEnemySettings { get; set; }

        #region Unity Methods

        private void Start()
        {
            StartMovement();
        }
        private void StartMovement()
        {
            StartState = new MoveStateHold(this);
            if (GetMoveApproach != 0) StartState = new MoveStatePatrol(this);
            if (GetMoveApproach == 0 && moveVelocity != 0) StartState = new MoveStateMove(this);

            ChangeState(StartState);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other == null || !ObstacleMaster.ObjectIsReady || (GetEnemySettings.ignoreWalls && GetEnemySettings.ignoreEnemies) ) return;
            var enemies = GetEnemySettings.ignoreEnemies ? null : other.GetComponentInParent<EnemiesMaster>();
            var wall = GetEnemySettings.ignoreWalls ? null : other.GetComponentInParent<WallMaster>();
            if (enemies == null && wall == null) return;
            _enemiesAnimation.AnimationFlip();
            DirectionVector *= -1;
        }
        
        private void OnDrawGizmos()
        {
            GizmoRadius = approachMovement;
        }
        #endregion

        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            GetEnemySettings = GetObjectScriptable<EnemiesScriptable>();
            _enemiesAnimation = GetComponent<EnemiesAnimation>();
            MoveVelocity = moveVelocity;
            _enemiesMaster = ObstacleMaster as EnemiesMaster;
            if (_enemiesMaster != null) GetEnemySettings = _enemiesMaster.GetEnemySettings;
        }

        protected override void ResetMovement()
        {
            SetVelocity(moveVelocity);
            base.ResetMovement();
        }

        protected override void ReloadMovement()
        {
            StartMovement();
            ResetMovement();
        }
    }
}