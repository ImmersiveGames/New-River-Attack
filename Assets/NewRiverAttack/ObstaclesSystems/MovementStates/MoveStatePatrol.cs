using ImmersiveGames.DebugManagers;
using ImmersiveGames.ObjectManagers.DetectManagers;
using ImmersiveGames.ObjectManagers.Interfaces;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.EnemiesSystems;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.MovementStates
{
    public class MoveStatePatrol : IMove
    {
        private Transform _target;
        private bool _inTransition;
        private readonly float _approachRange;
        private readonly DetectPlayerApproach _detectPlayerApproach;
        private readonly ObstacleMaster _obstacleMaster;
        private readonly ObstacleMovement _obstacleMovement;
        
        private EnemiesAnimation _enemiesAnimation;

        public MoveStatePatrol(ObstacleMovement obstacleMovement)
        {
            _obstacleMovement = obstacleMovement;
            _obstacleMaster = _obstacleMovement.GetComponent<ObstacleMaster>();
            _approachRange = _obstacleMovement.GetMoveApproach;
            _detectPlayerApproach = new DetectPlayerApproach(_obstacleMovement.transform.position, _approachRange);
        }
        public void EnterState()
        {
            DebugManager.Log<ObstacleMovement>($" Entrando no Estado: Patrol");
            _enemiesAnimation = _obstacleMovement.GetComponent<EnemiesAnimation>();
            _enemiesAnimation.AnimationMove(false);
            _inTransition = false;
        }

        public void UpdateState(Transform transform, Vector3 direction)
        {
            if (!_obstacleMaster.ObjectIsReady) return;
            DebugManager.Log<ObstacleMovement>($" Atualizando o Estado: Patrol");
            if (!_inTransition && _approachRange == 0)
            {
                _obstacleMovement.ChangeState(new MoveStateHold(_obstacleMovement));
                return;
            }
            _target = _detectPlayerApproach.TargetApproach<PlayerMaster>(_obstacleMaster.layerPlayer);
            if (_inTransition || !_target) return;
            _obstacleMovement.ChangeState(new MoveStateMove(_obstacleMovement));
            DebugManager.Log<ObstacleMovement>($" Target: {_target}");
        }

        public void ExitState()
        {
            DebugManager.Log<ObstacleMovement>($" Saindo do Estado: Patrol");
            _target = null;
            _inTransition = true;
        }
    }
}