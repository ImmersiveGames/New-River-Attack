using NewRiverAttack.ObstaclesSystems.Abstracts;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Helpers.Interfaces
{
    public interface IResettablePattern
    {
        void ResetPattern(ObjectShoot shooter);
        void ExitPattern(ObjectShoot shooter);
    }
}