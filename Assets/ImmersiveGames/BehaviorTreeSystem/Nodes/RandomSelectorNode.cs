using System.Collections.Generic;
using ImmersiveGames.BehaviorTreeSystem.Interface;
using UnityEngine;

namespace ImmersiveGames.BehaviorTreeSystem.Nodes
{
    public class RandomSelectorNode : INode
    {
        private readonly List<INode> _nodes;   // Lista de nós a serem selecionados aleatoriamente
        private readonly int _maxTimes;        // Quantas vezes selecionar um nó
        private int _currentTimes;             // Contador de execuções
        private INode _currentNode;            // Nó atualmente sendo executado

        public RandomSelectorNode(List<INode> nodes, int maxTimes = 1)
        {
            _nodes = nodes;
            _maxTimes = maxTimes;
            _currentTimes = 0;
        }

        public void OnEnter()
        {
            _currentTimes = 0;  // Reseta o contador ao entrar
            SelectRandomNode(); // Seleciona um nó aleatório para começar
        }

        public NodeState Tick()
        {
            // Se já atingiu o número máximo de execuções, retorna Success
            if (_currentTimes >= _maxTimes) return NodeState.Success;

            // Executa o nó atual
            var result = _currentNode.Tick();

            // Se o nó terminou com sucesso ou falha, sai do nó atual
            if (result is NodeState.Success or NodeState.Failure)
            {
                _currentNode.OnExit();
                _currentTimes++;  // Incrementa o contador

                // Se ainda não atingiu o máximo, escolhe outro nó aleatório
                if (_currentTimes < _maxTimes)
                {
                    SelectRandomNode();
                }
            }

            return _currentTimes < _maxTimes ? NodeState.Running : NodeState.Success;
        }

        public void OnExit()
        {
            _currentNode?.OnExit();  // Sai do nó atual se estiver em execução
        }

        // Método auxiliar para selecionar um nó aleatório
        private void SelectRandomNode()
        {
            if (_nodes.Count == 0) return;
            _currentNode = _nodes[Random.Range(0, _nodes.Count)];
            _currentNode.OnEnter();
        }
    }
}
