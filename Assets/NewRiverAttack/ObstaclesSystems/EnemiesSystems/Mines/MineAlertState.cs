using ImmersiveGames.FiniteStateMachine;
using NewRiverAttack.VfxSystems;
using UnityEngine;
using UnityEngine.VFX;

namespace NewRiverAttack.ObstaclesSystems.EnemiesSystems.Mines
{
    public class MineAlertState : IState
    {
        private readonly MineFuse _mineFuse;
        private float _remainingTime;
        private readonly float _duration;

        // Construtor que define a mina e a duração do alerta
        public MineAlertState(MineFuse mineFuse, float duration)
        {
            _mineFuse = mineFuse;
            _duration = duration;
        }

        // Método chamado a cada frame enquanto a mina está no estado de alerta
        public void Tick()
        {
            if (!_mineFuse.ShouldBeReady) return;
            _remainingTime -= Time.deltaTime; // Subtrai o tempo a cada tick
        }

        // Propriedade que retorna o tempo restante de alerta
        public float RemainingTime => _remainingTime;

        // Método para configurar o estado inicial ao entrar no alerta
        public void OnEnter()
        {
            _remainingTime = _duration;
            _mineFuse.OnEventAlertApproach(); // Notifica que o alerta foi iniciado
        }

        // Método para resetar as variáveis ao sair do estado de alerta
        public void OnExit()
        {
            _remainingTime = 0;
            _mineFuse.OnEventAlertStop(); // Notifica que o alerta foi interrompido
        }
        
    }
}