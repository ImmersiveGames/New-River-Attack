using System.Collections.Generic;
using ImmersiveGames.BehaviorTreeSystem.Interface;

namespace ImmersiveGames.BehaviorTreeSystem.Nodes
{
    public class ParallelNode : INode
    {
        private readonly List<INode> _nodes;
        private readonly bool _requireAllSuccess;
        private readonly bool _interruptOnSuccess;

        public ParallelNode(List<INode> nodes, bool requireAllSuccess = true, bool interruptOnSuccess = false)
        {
            _nodes = nodes;
            _requireAllSuccess = requireAllSuccess;
            _interruptOnSuccess = interruptOnSuccess;
        }

        public NodeState Tick()
        {
            bool anyRunning = false;
            int successCount = 0;

            foreach (var node in _nodes)
            {
                var result = node.Tick();

                if (result == NodeState.Running) anyRunning = true;
                else if (result == NodeState.Success) successCount++;

                if (result == NodeState.Success && _interruptOnSuccess) return NodeState.Success;
                if (result == NodeState.Failure && _requireAllSuccess) return NodeState.Failure;
            }

            if (_requireAllSuccess && successCount == _nodes.Count) return NodeState.Success;
            if (!_requireAllSuccess && successCount > 0) return NodeState.Success;

            return anyRunning ? NodeState.Running : NodeState.Failure;
        }

        public void OnEnter()
        {
            foreach (var node in _nodes) node.OnEnter();
        }

        public void OnExit()
        {
            foreach (var node in _nodes) node.OnExit();
        }
    }
}