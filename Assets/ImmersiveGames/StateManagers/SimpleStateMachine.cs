using UnityEngine;

namespace ImmersiveGames.StateManagers
{
    public class SimpleStateMachine<T> where T : ISimpleState
    {
        private T _currentState;
        private readonly Component _component;

        public SimpleStateMachine(Component master)
        {
            _component = master;
        }

        public void ChangeState(T newState)
        {
            if (_currentState != null && _currentState.Equals(newState))
                return;

            _currentState?.ExitState();
            _currentState = newState;
            _currentState?.EnterState(_component);
        }

        public void UpdateState()
        {
            _currentState?.UpdateState();
        }

        public ISimpleState GetActualState => _currentState;
    }

    public interface ISimpleState
    {
        void EnterState(Component master);
        void UpdateState();
        void ExitState();
    }
}