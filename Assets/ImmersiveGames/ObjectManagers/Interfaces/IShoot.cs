using NewRiverAttack.ObstaclesSystems.EnemiesSystems;
using UnityEngine;

namespace ImmersiveGames.ObjectManagers.Interfaces
{
    public interface IShoot
    {
        void EnterState(EnemiesMaster enemiesMaster);
        void UpdateState(Transform referencePosition);
        void ExitState();
    }
}