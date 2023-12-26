using RiverAttack;
namespace Utils
{
    public interface IShoot
    {
        void EnterState(ObstacleMaster enemiesMaster);
        void UpdateState();
        void ExitState();
    }
}
