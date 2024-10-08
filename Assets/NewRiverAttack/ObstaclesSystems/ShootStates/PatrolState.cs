using ImmersiveGames.DebugManagers;
using ImmersiveGames.FiniteStateMachine;
using ImmersiveGames.ObjectManagers.DetectManagers;
using NewRiverAttack.ObstaclesSystems.EnemiesSystems;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.ShootStates
{
    public class PatrolState : IState
    {
        private readonly EnemiesShoot _enemiesShoot;
        private readonly EnemiesMaster _enemiesMaster;
        private readonly DetectPlayerApproach _detectPlayerApproach;
        private Transform _target;

        public PatrolState(EnemiesShoot enemiesShoot)
        {
            _enemiesShoot = enemiesShoot;
            _enemiesMaster = enemiesShoot.GetComponent<EnemiesMaster>();
            _detectPlayerApproach = new DetectPlayerApproach(
                _enemiesShoot.transform.position,
                _enemiesMaster.GetEnemySettings.GetShootApproach
            );
        }

        public void Tick()
        {
            _target = _detectPlayerApproach.TargetApproach<PlayerMaster>(_enemiesMaster.layerPlayer);
            if (_target != null)
            {
                _enemiesShoot.SetTarget(_target);
            }
        }

        public void OnEnter()
        {
            DebugManager.Log<PatrolState>("Inimigo iniciou a patrulha.");
        }

        public void OnExit()
        {
            DebugManager.Log<PatrolState>("Inimigo parou de patrulhar.");
            _enemiesShoot.SetTarget(null); // Reseta o alvo quando sai do estado de patrulha
        }
    }
}