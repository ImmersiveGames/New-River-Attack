using RiverAttack;
namespace Utils
{
    public interface IShoot
    {
        void EnterState();
        void UpdateState(IHasPool myPool, EnemiesMaster enemyMaster);
        void ExitState();
        void Fire(IHasPool myPool, EnemiesMaster enemyMaster);
        //void SetTarget(Transform toTarget);
    }
}
