using System;
using ImmersiveGames.BehaviorTreeSystem.Interface;

namespace ImmersiveGames.BehaviorTreeSystem.Decorations
{
    public class ParallelConditionDecorator : INode
    {
        private readonly INode _decoratedNode;      // O nó encapsulado
        private readonly Func<bool> _condition;     // Condição a ser verificada paralelamente
        private readonly INode _parallelActionNode; // Ação a ser executada se a condição for verdadeira

        public ParallelConditionDecorator(INode decoratedNode, Func<bool> condition, INode parallelActionNode)
        {
            _decoratedNode = decoratedNode;
            _condition = condition;
            _parallelActionNode = parallelActionNode;
        }

        public NodeState Tick()
        {
            // Verifica a condição paralela
            if (_condition())
            {
                // Executa a ação paralela se a condição for verdadeira
                var result = _parallelActionNode.Tick();

                // Se a ação paralela terminar, retorna o resultado
                if (result == NodeState.Success || result == NodeState.Failure)
                {
                    _parallelActionNode.OnExit();
                    return result;
                }
            }

            // Continua com o comportamento do nó decorado
            return _decoratedNode.Tick();
        }

        public void OnEnter()
        {
            _decoratedNode.OnEnter();
            _parallelActionNode.OnEnter();  // Prepara o nó paralelo para ser executado se necessário
        }

        public void OnExit()
        {
            _decoratedNode.OnExit();
            _parallelActionNode.OnExit();   // Limpa o nó paralelo após sua execução
        }
    }
}