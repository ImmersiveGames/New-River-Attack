using ImmersiveGames.DebugManagers;
using ImmersiveGames.ObjectManagers.DetectManagers;
using ImmersiveGames.ObjectManagers.Interfaces;
using NewRiverAttack.ObstaclesSystems.EnemiesSystems;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.ShootStates
{
    public class EnemyShootStatePatrol : IShoot
    {
        private Transform _target;
        
        private readonly EnemiesShootOld _enemiesShootOld;
        private EnemiesMaster _enemiesMaster;
        private EnemiesScriptable _enemies;
        private bool _inTransition;
        
        private float _approachRange;
        private DetectPlayerApproach _detectPlayerApproach;
        public EnemyShootStatePatrol(EnemiesShootOld enemiesShootOld, bool inTransition)
        {
            _enemiesShootOld = enemiesShootOld;
            _inTransition = inTransition;
        }
        public void EnterState(EnemiesMaster enemiesMaster)
        {
            DebugManager.Log<EnemiesShootOld>($" Entrando no Estado: Patrol");
            _inTransition = false;
            _enemiesMaster = enemiesMaster;
            _enemies = enemiesMaster.GetEnemySettings;
            _approachRange = _enemies.GetShootApproach;
            _detectPlayerApproach = new DetectPlayerApproach(_enemiesShootOld.transform.position, _approachRange);
        }

        public void UpdateState(Transform referencePosition)
        {
            
            if (!_enemiesMaster.ObjectIsReady) return;
            DebugManager.Log<EnemiesShootOld>($" Atualizando o Estado: Patrol");
            _target = _detectPlayerApproach.TargetApproach<PlayerMaster>(_enemiesMaster.layerPlayer);
            if (_inTransition || !_target) return;
            _enemiesShootOld.ChangeState(new EnemyShootSimpleState(_enemiesShootOld));
            DebugManager.Log<EnemiesMovement>($" Target: {_target}");
        }

        public void ExitState()
        {
            DebugManager.Log<EnemiesShootOld>($" Saindo do Estado: Patrol");
            _inTransition = true;
            _target = null;
        }
    }
}