using System;
using ImmersiveGames.BehaviorTreeSystem.Interface;

namespace ImmersiveGames.BehaviorTreeSystem.Decorations
{
    public class GlobalConditionDecorator : INode
    {
        private readonly INode _decoratedNode; // O nó principal que está sendo decorado
        private readonly Func<bool> _condition; // A condição global a ser verificada
        private readonly INode _overrideNode;   // O nó a ser executado se a condição for verdadeira

        public GlobalConditionDecorator(INode decoratedNode, Func<bool> condition, INode overrideNode)
        {
            _decoratedNode = decoratedNode;
            _condition = condition;
            _overrideNode = overrideNode;
        }

        public NodeState Tick()
        {
            // Verifica a condição global
            if (_condition())
            {
                // Se a condição for verdadeira, executa o nó de substituição (override)
                var result = _overrideNode.Tick();

                // Finaliza a execução do overrideNode quando ele termina
                if (result is NodeState.Success or NodeState.Failure)
                {
                    _overrideNode.OnExit();
                }

                return result;
            }

            // Caso a condição não seja verdadeira, continua com o comportamento original
            return _decoratedNode.Tick();
        }

        public void OnEnter()
        {
            _decoratedNode.OnEnter();
            _overrideNode.OnEnter();  // Prepara o nó de substituição, caso seja necessário
        }

        public void OnExit()
        {
            _decoratedNode.OnExit();
            _overrideNode.OnExit();  // Limpa o nó de substituição
        }
    }
}