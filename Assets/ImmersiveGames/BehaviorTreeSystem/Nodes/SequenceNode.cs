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
            _currentNodeIndex = 0;  
            if (_nodes.Count > 0)
            {
                _nodes[0].OnEnter(); // Garante que o primeiro nó sempre inicializa ao entrar na sequência
            }
        }

        public NodeState Tick()
        {
            if (_currentNodeIndex >= _nodes.Count) 
                return NodeState.Success; // Retorna sucesso se todos os nós já foram executados

            var currentNode = _nodes[_currentNodeIndex];
            var result = currentNode.Tick();

            if (result == NodeState.Running)
                return NodeState.Running;

            currentNode.OnExit();

            if (result == NodeState.Failure)
                return NodeState.Failure; // Falha na sequência interrompe a execução

            // Se o nó atual teve sucesso, avança para o próximo
            _currentNodeIndex++;
            if (_currentNodeIndex < _nodes.Count)
            {
                _nodes[_currentNodeIndex].OnEnter(); // Inicializa o próximo nó
                return NodeState.Running; // Continua a execução
            }

            return NodeState.Success; // Todos os nós foram executados com sucesso
        }

        public void OnExit()
        {
            if (_currentNodeIndex < _nodes.Count)
            {
                _nodes[_currentNodeIndex].OnExit(); // Executa o OnExit do nó atual ao sair da sequência
            }
        }
    }
}