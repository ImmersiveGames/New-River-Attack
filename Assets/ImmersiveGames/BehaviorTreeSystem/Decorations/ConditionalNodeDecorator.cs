using System;
using ImmersiveGames.BehaviorTreeSystem.Interface;
using UnityEngine;

namespace ImmersiveGames.BehaviorTreeSystem.Decorations
{
    public class ConditionalNodeDecorator : INode
    {
        private readonly INode _node;            // O nó decorado
        private readonly Func<bool> _condition;  // Condição para interromper o comportamento
        private readonly Action _onConditionMet; // Ação a ser executada quando a condição é atendida

        private bool _isConditionMet;    // Flag para verificar se a condição foi atendida

        public ConditionalNodeDecorator(INode node, Func<bool> condition, Action onConditionMet = null)
        {
            _node = node;
            _condition = condition;
            _onConditionMet = () => 
            {
                Debug.Log("Executing onConditionMet action.");
                onConditionMet?.Invoke();
            };
        }


        public void OnEnter()
        {
            _isConditionMet = false;
            _node.OnEnter();
        }

        public NodeState Tick()
        {
            // Verifica a condição de parada
            if (_condition())
            {
                Debug.Log("Condição atendida no ConditionalNodeDecorator.");
                _onConditionMet?.Invoke();
                _isConditionMet = true;
            }

            // Se a condição foi atendida, retorna Success imediatamente
            if (_isConditionMet)
            {
                return NodeState.Success;
            }

            // Continua normalmente caso a condição não tenha sido atendida
            return _node.Tick();
        }


        public void OnExit()
        {
            _isConditionMet = false;
            _node.OnExit();
        }
    }
}