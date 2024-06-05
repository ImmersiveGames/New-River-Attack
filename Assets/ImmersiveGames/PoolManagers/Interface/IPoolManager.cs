using ImmersiveGames.BulletsManagers;
using UnityEngine;

namespace ImmersiveGames.PoolManagers.Interface
{
    public interface IPoolManager
    {
        bool CreatePool(string poolName, GameObject prefab, int initialPoolSize, Transform poolRoot, bool persistent = false);

        GameObject GetObjectFromPool<T>(string poolName, Transform spawnPosition, BulletData bulletData) where T : IPoolable;

        Transform GetPool(string poolName);

        void ClearUnusedObjects(string poolName);

        void ResizePool(string poolName, int newSize);

        void ReturnObjectToPool(GameObject obj, string poolName);
    }
}