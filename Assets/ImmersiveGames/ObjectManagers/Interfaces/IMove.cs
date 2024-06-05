using UnityEngine;

namespace ImmersiveGames.ObjectManagers.Interfaces
{
    public interface IMove
    {
        void EnterState();
        void UpdateState(Transform transform, Vector3 direction);
        void ExitState();
    }
}