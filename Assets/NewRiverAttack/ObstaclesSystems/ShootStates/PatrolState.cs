using ImmersiveGames.DebugManagers;
using ImmersiveGames.FiniteStateMachine;
using ImmersiveGames.ObjectManagers.DetectManagers;
using NewRiverAttack.ObstaclesSystems.EnemiesSystems;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.ShootStates
{
    public class PatrolState : IState
    {
        private readonly EnemiesShoot _enemiesShoot;
        private readonly EnemiesMaster _enemiesMaster;
        private readonly EnemiesScriptable _enemiesScriptable;
        private DetectPlayerApproach _detectPlayerApproach;
        private Transform _target;

        public PatrolState(EnemiesShoot enemiesShoot, EnemiesScriptable enemiesScriptable)
        {
            _enemiesShoot = enemiesShoot;
            _enemiesScriptable = enemiesScriptable;
            _enemiesMaster = enemiesShoot.GetComponent<EnemiesMaster>();

            // Inicializa o DetectPlayerApproach com o valor calculado por GetShootApproach
            _detectPlayerApproach = new DetectPlayerApproach(_enemiesShoot.transform.position, _enemiesScriptable.GetShootApproach);
        }

        public void Tick()
        {
            if (!_enemiesShoot.ShouldBeShoot) return;
            // Checa se o alvo está ao alcance usando Raycast e a distância gerada por GetShootApproach
            _target = _detectPlayerApproach.TargetApproach<PlayerMaster>(_enemiesMaster.layerPlayer);

            // Se o alvo está dentro do alcance, faz a transição para o estado de atirar
            if (_target != null)
            {
                _enemiesShoot.SetTarget(_target);
            }
        }

        public void OnEnter()
        {
            DebugManager.Log<PatrolState>("Inimigo iniciou a patrulha.");
            // Atualiza o DetectPlayerApproach sempre que o estado de patrulha é ativado
            _detectPlayerApproach = new DetectPlayerApproach(_enemiesShoot.transform.position, _enemiesScriptable.GetShootApproach);
        }

        public void OnExit()
        {
            DebugManager.Log<PatrolState>("Inimigo parou de patrulhar.");
            _target = null; // Reseta o alvo ao sair da patrulha
        }

        // Verifica se o alvo foi encontrado e está ao alcance de disparo
        public bool IsTargetInRange()
        {
            return _target != null;
        }
    }
}