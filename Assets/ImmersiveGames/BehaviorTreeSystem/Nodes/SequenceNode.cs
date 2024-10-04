using System.Collections.Generic;
using ImmersiveGames.BehaviorTreeSystem.Interface;

namespace ImmersiveGames.BehaviorTreeSystem.Nodes
{
    public class SequenceNode : INode
    {
        private readonly List<INode> _nodes;
        private int _currentNodeIndex;

        public SequenceNode(List<INode> nodes)
        {
            _nodes = nodes;
            _currentNodeIndex = 0;
        }

        public void OnEnter()
        {
            _currentNodeIndex = 0;  // Reinicializa o índice quando o nó começa
            if (_nodes.Count > 0)
            {
                _nodes[_currentNodeIndex].OnEnter();  // Chama OnEnter do primeiro nó
            }
        }

        public NodeState Tick()
        {
            if (_currentNodeIndex >= _nodes.Count) return NodeState.Success;

            var result = _nodes[_currentNodeIndex].Tick();

            if (result != NodeState.Success)
                return _currentNodeIndex >= _nodes.Count ? NodeState.Success : NodeState.Running;
            _nodes[_currentNodeIndex].OnExit();  // Limpeza do nó concluído
            _currentNodeIndex++;

            if (_currentNodeIndex < _nodes.Count)
            {
                _nodes[_currentNodeIndex].OnEnter();  // Chama OnEnter do próximo nó
            }

            return _currentNodeIndex >= _nodes.Count ? NodeState.Success : NodeState.Running;
        }

        public void OnExit()
        {
            if (_currentNodeIndex < _nodes.Count)
            {
                _nodes[_currentNodeIndex].OnExit();
            }
        }
    }
}