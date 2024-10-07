using ImmersiveGames.BehaviorTreeSystem.Interface;
using UnityEngine;

namespace ImmersiveGames.BehaviorTreeSystem.Nodes
{
    public class WaitNode : INode
    {
        private readonly float _waitTime;
        private float _elapsedTime;

        public WaitNode(float waitTime)
        {
            _waitTime = waitTime;
            _elapsedTime = 0f;
        }

        public void OnEnter()
        {
            _elapsedTime = 0f;  // Reinicializa o tempo quando o nó começa);
        }

        public NodeState Tick()
        {
            _elapsedTime += Time.deltaTime;
            return _elapsedTime >= _waitTime ? NodeState.Success : NodeState.Running;
        }

        public void OnExit()
        {
            // Limpeza ou lógica ao sair, caso necessário, pode ser decorada
        }
    }
}