using ImmersiveGames.DebugManagers;
using ImmersiveGames.ObjectManagers.DetectManagers;
using ImmersiveGames.ObjectManagers.Interfaces;
using NewRiverAttack.ObstaclesSystems.EnemiesSystems;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.MovementStates
{
    public class MoveStatePatrol : IMove
    {
        private Transform _target;
        private readonly EnemiesMovement _enemiesMovement;
        private EnemiesAnimation _enemiesAnimation;
        private bool _inTransition;
        private readonly float _approachRange;
        private readonly DetectPlayerApproach _detectPlayerApproach;
        private readonly EnemiesMaster _enemiesMaster;

        public MoveStatePatrol(EnemiesMovement enemiesMovement)
        {
            _enemiesMovement = enemiesMovement;
            _enemiesMaster = _enemiesMovement.GetComponent<EnemiesMaster>();
            var enemiesScriptable = _enemiesMovement.GetEnemySettings;
            _approachRange = enemiesScriptable.GetMoveApproach;
            _detectPlayerApproach = new DetectPlayerApproach(_enemiesMovement.transform.position, _approachRange);
        }
        public void EnterState()
        {
            DebugManager.Log<EnemiesMovement>($" Entrando no Estado: Patrol");
            _enemiesAnimation = _enemiesMovement.GetComponent<EnemiesAnimation>();
            _enemiesAnimation.AnimationMove(false);
            _inTransition = false;
        }

        public void UpdateState(Transform transform, Vector3 direction)
        {
            if (!_enemiesMaster.ObjectIsReady) return;
            DebugManager.Log<EnemiesMovement>($" Atualizando o Estado: Patrol");
            if (!_inTransition && _approachRange == 0)
            {
                _enemiesMovement.ChangeState(new MoveStateHold(_enemiesMovement));
                return;
            }
            _target = _detectPlayerApproach.TargetApproach<PlayerMaster>(_enemiesMaster.layerPlayer);
            if (_inTransition || !_target) return;
            _enemiesMovement.ChangeState(new MoveStateMove(_enemiesMovement));
            DebugManager.Log<EnemiesMovement>($" Target: {_target}");
        }

        public void ExitState()
        {
            DebugManager.Log<EnemiesMovement>($" Saindo do Estado: Patrol");
            _target = null;
            _inTransition = true;
        }
    }
}