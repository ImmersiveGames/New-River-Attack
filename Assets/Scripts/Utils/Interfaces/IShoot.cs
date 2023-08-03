using RiverAttack;
namespace Utils
{
    public interface IShoot
    {
        void EnterState(EnemiesMaster enemiesMaster);
        void UpdateState();
        void ExitState();
    }
}
