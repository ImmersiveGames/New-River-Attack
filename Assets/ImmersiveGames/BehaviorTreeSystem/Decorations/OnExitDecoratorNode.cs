using System;
using ImmersiveGames.BehaviorTreeSystem.Interface;

namespace ImmersiveGames.BehaviorTreeSystem.Decorations
{
    public class OnExitDecoratorNode : INode
    {
        private readonly INode _node;     // O nó original
        private readonly Action _onExit;  // Ação adicional a ser executada no OnExit

        public OnExitDecoratorNode(INode node, Action onExit)
        {
            _node = node;
            _onExit = onExit;
        }

        public void OnEnter()
        {
            _node.OnEnter();  // Apenas repassa o OnEnter
        }

        public NodeState Tick()
        {
            return _node.Tick();  // Apenas repassa o Tick
        }

        public void OnExit()
        {
            _node.OnExit();       // Chama OnExit do nó original
            _onExit?.Invoke();    // Executa a ação decorada após o OnExit do nó
        }
    }
}