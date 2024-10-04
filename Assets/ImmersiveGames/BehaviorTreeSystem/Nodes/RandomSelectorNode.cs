using System.Collections.Generic;
using ImmersiveGames.BehaviorTreeSystem.Interface;
using UnityEngine;

namespace ImmersiveGames.BehaviorTreeSystem.Nodes
{
    public class RandomSelectorNode : INode
    {
        private readonly List<INode> _nodes;
        private readonly int _maxTimes;
        private int _currentTimes;
        private INode _currentNode;

        public RandomSelectorNode(List<INode> nodes, int maxTimes = 1)
        {
            _nodes = nodes;
            _maxTimes = maxTimes;
            _currentTimes = 0;
        }

        public void OnEnter()
        {
            if (_currentTimes < _maxTimes)
            {
                _currentNode = _nodes[Random.Range(0, _nodes.Count)];
                _currentNode.OnEnter();
            }
        }

        public NodeState Tick()
        {
            if (_currentTimes >= _maxTimes) return NodeState.Success;

            var result = _currentNode.Tick();
            if (result == NodeState.Success || result == NodeState.Failure)
            {
                _currentNode.OnExit();
                _currentTimes++;
                if (_currentTimes < _maxTimes) OnEnter();  // Escolhe outro nó aleatório
            }

            return NodeState.Running;
        }

        public void OnExit()
        {
            _currentNode?.OnExit();
        }
    }
}