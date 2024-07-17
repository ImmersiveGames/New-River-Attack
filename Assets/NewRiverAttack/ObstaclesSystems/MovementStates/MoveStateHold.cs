using ImmersiveGames.DebugManagers;
using ImmersiveGames.ObjectManagers.Interfaces;
using NewRiverAttack.ObstaclesSystems.EnemiesSystems;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.MovementStates
{
    public class MoveStateHold : IMove
    {
        private readonly EnemiesMovement _enemiesMovement;
        private readonly EnemiesMaster _enemiesMaster;
        private readonly EnemiesScriptable _enemiesScriptable;
        private EnemiesAnimation _enemiesAnimation;
        
        public float SpeedMovement;
        private bool _hasApproach;
        private bool _inTransition;
        
        public MoveStateHold(EnemiesMovement enemiesMovement)
        {
            _enemiesMovement = enemiesMovement;
            _enemiesMaster = _enemiesMovement.GetComponent<EnemiesMaster>();
            _enemiesScriptable = _enemiesMovement.GetEnemySettings;
            SpeedMovement = 0;
        }

        public void EnterState()
        {
            DebugManager.Log<EnemiesMovement>($" Entrando no Estado: HOLD");
            _hasApproach = _enemiesScriptable.approachMovement.x != 0 &&
                           _enemiesScriptable.approachMovement.y != 0;
            _enemiesAnimation = _enemiesMovement.GetComponent<EnemiesAnimation>();
            _enemiesAnimation.AnimationMove(false);
            _inTransition = false;
        }

        public void UpdateState(Transform transform, Vector3 direction)
        {
            if (!_enemiesMaster.ObjectIsReady) return;
            if (!_inTransition && _hasApproach)
            {
                _enemiesMovement.ChangeState(new MoveStatePatrol(_enemiesMovement));
            }
            if (!_inTransition && _enemiesMovement.GetVelocity != 0 )
            {
                _enemiesMovement.ChangeState(new MoveStateMove(_enemiesMovement));
            }
            if (_enemiesMovement.ShouldBeMove || _inTransition) return;
            DebugManager.Log<EnemiesMovement>($" Atualizando o Estado: HOLD");
        }

        public void ExitState()
        {
            _inTransition = true;
            DebugManager.Log<EnemiesMovement>($" Saindo do Estado: HOLD");
        }
    }
}