using RiverAttack;
using UnityEngine;
namespace Utils
{
    public interface IMove
    {
        void EnterState();
        void UpdateState(Transform transform, Vector3 direction);
        void ExitState();
    }
}
