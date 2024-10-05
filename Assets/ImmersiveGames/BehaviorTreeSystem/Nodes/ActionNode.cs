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
            Debug.Log("Action Enter");
            // O comportamento de entrada será controlado pelo decorator, se necessário.
        }

        public NodeState Tick()
        {
            return _action.Invoke();
        }

        public void OnExit()
        {
            Debug.Log("Action Exit");
            // O comportamento de saída será controlado pelo decorator, se necessário.
        }
    }
}