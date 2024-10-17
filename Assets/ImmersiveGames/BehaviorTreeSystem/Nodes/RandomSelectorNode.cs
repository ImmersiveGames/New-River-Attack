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
            // Verifica se o RandomSelectorNode atingiu o número máximo de execuções
            if (_currentTimes >= _maxTimes) 
                return NodeState.Success;

            var result = _currentNode.Tick();

            // Se o nó atual retornar Success ou Failure, conclui a execução do RandomSelectorNode
            if (result is NodeState.Success or NodeState.Failure)
            {
                _currentNode.OnExit();
                _currentTimes++;

                // Se houver execuções restantes, seleciona um novo nó aleatório
                if (_currentTimes >= _maxTimes) return NodeState.Success;
                SelectRandomNode();
                return NodeState.Running;

                // Retorna Success após alcançar o número máximo de execuções
            }

            // Continua executando enquanto o nó atual ainda está Running
            return NodeState.Running;
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