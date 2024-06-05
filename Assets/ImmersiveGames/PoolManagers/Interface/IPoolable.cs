using ImmersiveGames.BulletsManagers;
using UnityEngine;

namespace ImmersiveGames.PoolManagers.Interface
{
    public interface IPoolable
    {
        bool IsPooled { get; }
        PoolObject Pool { get; set; }
        void OnSpawned(Transform spawnPosition, IBulletsData bulletData);
        void OnDespaired();
    }
}