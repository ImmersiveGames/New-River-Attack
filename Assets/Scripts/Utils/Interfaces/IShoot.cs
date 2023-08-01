using RiverAttack;
namespace Utils
{
    public interface IShoot
    {
        void EnterState(EnemiesMaster enemiesMaster);
        void UpdateState();
        void ExitState();
        void Fire();
        //void SetTarget(Transform toTarget);
    }
}
