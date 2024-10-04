using System;
using ImmersiveGames.BehaviorTreeSystem.Interface;

namespace ImmersiveGames.BehaviorTreeSystem.Nodes
{
    public class GlobalConditionNode : INode
    {
        private readonly Func<bool> _condition;
        private readonly INode _overrideNode;

        public GlobalConditionNode(Func<bool> condition, INode overrideNode)
        {
            _condition = condition;
            _overrideNode = overrideNode;
        }

        public NodeState Tick()
        {
            if (!_condition()) return NodeState.Running;
            _overrideNode.OnEnter();
            var result = _overrideNode.Tick();
            if (result is NodeState.Success or NodeState.Failure)
            {
                _overrideNode.OnExit();
            }
            return result;

        }

        public void OnEnter() { }  // Sem necessidade de setup específico
        public void OnExit() { }   // Sem necessidade de cleanup específico
    }
}