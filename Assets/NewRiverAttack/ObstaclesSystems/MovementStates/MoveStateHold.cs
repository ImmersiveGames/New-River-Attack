using ImmersiveGames.DebugManagers;
using ImmersiveGames.ObjectManagers.Interfaces;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.EnemiesSystems;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.MovementStates
{
    public class MoveStateHold : IMove
    {
        private readonly ObstacleMovement _obstacleMovement;
        private readonly ObstacleMaster _obstacleMaster;
        private readonly EnemiesScriptable _enemiesScriptable;
        private EnemiesAnimation _enemiesAnimation;
        
        public float SpeedMovement;
        private bool _hasApproach;
        private bool _inTransition;
        
        public MoveStateHold(ObstacleMovement obstacleMovement)
        {
            _obstacleMovement = obstacleMovement;
            _obstacleMaster = _obstacleMovement.GetComponent<ObstacleMaster>();
            _enemiesScriptable = _obstacleMovement.GetObjectScriptable<EnemiesScriptable>();
            SpeedMovement = 0;
        }

        public void EnterState()
        {
            DebugManager.Log<ObstacleMovement>($" Entrando no Estado: HOLD");
            _hasApproach = _enemiesScriptable.approachMovement.x != 0 &&
                           _enemiesScriptable.approachMovement.y != 0;
            _enemiesAnimation = _obstacleMovement.GetComponent<EnemiesAnimation>();
            _enemiesAnimation.AnimationMove(false);
            _inTransition = false;
        }

        public void UpdateState(Transform transform, Vector3 direction)
        {
            if (!_obstacleMaster.ObjectIsReady) return;
            if (!_inTransition && _hasApproach)
            {
                _obstacleMovement.ChangeState(new MoveStatePatrol(_obstacleMovement));
            }
            if (!_inTransition && _obstacleMovement.GetVelocity != 0 )
            {
                _obstacleMovement.ChangeState(new MoveStateMove(_obstacleMovement));
            }
            if (_obstacleMovement.ShouldBeMove || _inTransition) return;
            DebugManager.Log<ObstacleMovement>($" Atualizando o Estado: HOLD");
        }

        public void ExitState()
        {
            _inTransition = true;
            DebugManager.Log<ObstacleMovement>($" Saindo do Estado: HOLD");
        }
    }
}