using ImmersiveGames.BehaviorTreeSystem.Interface;
using UnityEngine;

namespace ImmersiveGames.BehaviorTreeSystem.Decorations
{
    public class DelayDecoratorNode : INode
    {
        private readonly INode _node;   // Nó a ser decorado
        private readonly float _delay;  // Tempo de delay em segundos
        private float _elapsedTime;     // Tempo decorrido

        public DelayDecoratorNode(INode node, float delay)
        {
            _node = node;
            _delay = delay;
            _elapsedTime = 0f;
        }

        public void OnEnter()
        {
            _elapsedTime = 0f;  // Reinicializa o tempo decorrido ao entrar no decorator
        }

        public NodeState Tick()
        {
            // Incrementa o tempo decorrido
            _elapsedTime += Time.deltaTime;

            // Se o tempo de delay não tiver passado, retorna "Running"
            return _elapsedTime < _delay ? NodeState.Running :
                // Quando o delay termina, executa o nó encapsulado
                _node.Tick();
        }

        public void OnExit()
        {
            _node.OnExit();  // Chama o OnExit do nó encapsulado quando o decorator termina
        }
    }
}