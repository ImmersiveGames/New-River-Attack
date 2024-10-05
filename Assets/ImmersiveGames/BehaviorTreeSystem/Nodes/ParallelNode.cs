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
            bool isAnyRunning = false;
            int successCount = 0;

            foreach (var node in _nodes)
            {
                var result = node.Tick();

                switch (result)
                {
                    case NodeState.Running:
                        isAnyRunning = true;
                        break;
                    case NodeState.Success:
                        successCount++;
                        if (_interruptOnSuccess) 
                            return NodeState.Success; // Interrompe com sucesso imediato
                        break;
                    case NodeState.Failure:
                        if (_requireAllSuccess) 
                            return NodeState.Failure; // Falha imediata se todos precisarem ter sucesso
                        break;
                }
            }

            if (_requireAllSuccess && successCount == _nodes.Count) 
                return NodeState.Success;
            if (!_requireAllSuccess && successCount > 0) 
                return NodeState.Success;

            return isAnyRunning ? NodeState.Running : NodeState.Failure;
        }

        public void OnEnter()
        {
            foreach (var node in _nodes)
            {
                node.OnEnter(); // Inicializa todos os nós
            }
        }

        public void OnExit()
        {
            foreach (var node in _nodes)
            {
                node.OnExit(); // Finaliza todos os nós
            }
        }
    }
}
