using System;
using ImmersiveGames.BehaviorTreeSystem.Interface;
using UnityEngine;

namespace ImmersiveGames.BehaviorTreeSystem.Nodes
{
    public class ActionNode : INode
    {
        private readonly Func<NodeState> _action;

        public ActionNode(Func<NodeState> action)
        {
            _action = action;
        }

        public void OnEnter()
        {
            // O comportamento de entrada será controlado pelo decorator, se necessário.
        }

        public NodeState Tick()
        {
            return _action.Invoke();
        }

        public void OnExit()
        {
            // O comportamento de saída será controlado pelo decorator, se necessário.
        }
    }
}