using RiverAttack;
using UnityEngine;
namespace  Utils
{
    public interface IMove
    {
        void EnterState(EnemiesMaster enemiesMaster);
        void UpdateState(Transform transform, Vector3 direction);
        void ExitState();
    }
}

