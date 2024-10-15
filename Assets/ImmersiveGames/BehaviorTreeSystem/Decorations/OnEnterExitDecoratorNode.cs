using System;
using ImmersiveGames.BehaviorTreeSystem.Interface;

namespace ImmersiveGames.BehaviorTreeSystem.Decorations
{
    public class OnEnterExitDecoratorNode : INode
    {
        private readonly INode _node;
        private readonly Action _onEnter;
        private readonly Action _onExit;

        public OnEnterExitDecoratorNode(INode node, Action onEnter, Action onExit)
        {
            _node = node;
            _onEnter = onEnter;
            _onExit = onExit;
        }

        public void OnEnter()
        {
            _onEnter?.Invoke(); // Executa a ação decorada antes de chamar OnEnter do nó
            _node.OnEnter();
        }

        public NodeState Tick()
        {
            return _node.Tick();
        }

        public void OnExit()
        {
            _node.OnExit();
            _onExit?.Invoke(); // Executa a ação decorada após chamar OnExit do nó
        }
    }
}