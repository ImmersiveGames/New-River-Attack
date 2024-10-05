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
            _currentTimes = 0;
            SelectRandomNode();
        }

        public NodeState Tick()
        {
            if (_currentTimes >= _maxTimes) 
                return NodeState.Success;  // Sucesso se o limite de execuções for alcançado

            var result = _currentNode.Tick();

            if (result is NodeState.Success or NodeState.Failure)
            {
                _currentNode.OnExit();
                _currentTimes++;

                if (_currentTimes < _maxTimes)
                {
                    SelectRandomNode(); // Seleciona um novo nó aleatório se ainda há execuções restantes
                    return NodeState.Running;
                }

                return NodeState.Success; // Sucesso após atingir o limite de execuções
            }

            return NodeState.Running; // Continua enquanto o nó atual está em execução
        }

        public void OnExit()
        {
            _currentNode?.OnExit();
        }

        private void SelectRandomNode()
        {
            if (_nodes.Count == 0) return;
            _currentNode = _nodes[Random.Range(0, _nodes.Count)];
            _currentNode.OnEnter();
        }
    }
}