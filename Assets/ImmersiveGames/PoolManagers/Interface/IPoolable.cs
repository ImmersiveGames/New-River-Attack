namespace ImmersiveGames.PoolManagers.Interface
{
    public interface IPoolable
    {
        bool IsPooled { get; }
        PoolObject Pool { get; set; }
        void AssignPool(PoolObject pool);
        void OnSpawned();
        void OnDespawned();
    }
}