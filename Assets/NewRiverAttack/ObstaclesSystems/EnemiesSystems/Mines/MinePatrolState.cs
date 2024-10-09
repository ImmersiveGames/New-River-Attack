using ImmersiveGames.FiniteStateMachine;
using ImmersiveGames.ObjectManagers.DetectManagers;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.EnemiesSystems.Mines
{
    public class MinePatrolState : IState
    {
        private readonly MineFuse _mineFuse;
        private  DetectPlayerApproach _detectPlayerApproach;
        private readonly EnemiesScriptable _settings;

        // Construtor que define a mina e inicializa o sistema de detecção
        public MinePatrolState(MineFuse mineFuse, EnemiesScriptable settings)
        {
            _mineFuse = mineFuse;
            _settings = settings;
        }

        // Método chamado a cada frame enquanto a mina está em patrulha
        public void Tick()
        {
            
            // Verifica se há um alvo em aproximação
            var target = _detectPlayerApproach.TargetApproach<PlayerMaster>(_mineFuse.transform.position,_mineFuse.PlayerLayer);
            if (target != null)
            {
                _mineFuse.SetTarget(target); // Define o alvo na mina
            }
        }

        // Método para ações ao entrar no estado de patrulha
        public void OnEnter()
        {
            _mineFuse.SetTarget(null);  // Certifica-se de que o alvo seja resetado ao entrar em patrulha
            if (_detectPlayerApproach != null) return;
            _detectPlayerApproach = new DetectPlayerApproach(
                _settings.GetShootApproach
            );
            
        }

        // Método para resetar o alvo ao sair do estado de patrulha
        public void OnExit()
        {
            _mineFuse.SetTarget(null); // Reseta o alvo
        }
    }
}