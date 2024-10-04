using System;
using ImmersiveGames.BehaviorTreeSystem.Interface;

namespace ImmersiveGames.BehaviorTreeSystem.Decorations
{
    public class OnEnterDecoratorNode : INode
    {
        private readonly INode _node;      // O nó original
        private readonly Action _onEnter;  // Ação adicional a ser executada no OnEnter

        public OnEnterDecoratorNode(INode node, Action onEnter)
        {
            _node = node;
            _onEnter = onEnter;
        }

        public void OnEnter()
        {
            _onEnter?.Invoke();  // Executa a ação decorada antes de chamar OnEnter do nó
            _node.OnEnter();      // Chama OnEnter do nó encapsulado
        }

        public NodeState Tick()
        {
            return _node.Tick();  // Apenas repassa o Tick
        }

        public void OnExit()
        {
            _node.OnExit();  // Apenas repassa o OnExit
        }
    }
}