using RiverAttack;
namespace Utils
{
    public interface IShoot
    {
        void EnterState(ObstacleMasterOld enemiesMasterOld);
        void UpdateState();
        void ExitState();
    }
}
