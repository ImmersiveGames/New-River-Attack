using ImmersiveGames.DebugManagers;
using ImmersiveGames.ObjectManagers.DetectManagers;
using ImmersiveGames.ObjectManagers.Interfaces;
using NewRiverAttack.ObstaclesSystems.EnemiesSystems;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.ShootStates
{
    public class ShootStatePatrol : IShoot
    {
        private Transform _target;
        
        private readonly EnemiesShoot _enemiesShoot;
        private EnemiesMaster _enemiesMaster;
        private EnemiesScriptables _enemies;
        private bool _inTransition;
        
        private float _approachRange;
        private DetectPlayerApproach _detectPlayerApproach;
        public ShootStatePatrol(EnemiesShoot enemiesShoot, bool inTransition)
        {
            _enemiesShoot = enemiesShoot;
            _inTransition = inTransition;
        }
        public void EnterState(EnemiesMaster enemiesMaster)
        {
            DebugManager.Log<EnemiesShoot>($" Entrando no Estado: Patrol");
            _inTransition = false;
            _enemiesMaster = enemiesMaster;
            _enemies = enemiesMaster.GetEnemySettings;
            _approachRange = _enemies.GetShootApproach;
            _detectPlayerApproach = new DetectPlayerApproach(_enemiesShoot.transform.position, _approachRange);
        }

        public void UpdateState(Transform referencePosition)
        {
            
            if (!_enemiesMaster.ObjectIsReady) return;
            DebugManager.Log<EnemiesShoot>($" Atualizando o Estado: Patrol");
            _target = _detectPlayerApproach.TargetApproach<PlayerMaster>(_enemiesMaster.layerPlayer);
            if (_inTransition || !_target) return;
            _enemiesShoot.ChangeState(new ShootStateShoot(_enemiesShoot));
            DebugManager.Log<EnemiesMovement>($" Target: {_target}");
        }

        public void ExitState()
        {
            DebugManager.Log<EnemiesShoot>($" Saindo do Estado: Patrol");
            _inTransition = true;
            _target = null;
        }
    }
}