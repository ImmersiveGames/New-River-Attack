using UnityEngine;

namespace ImmersiveGames.PoolManagers.Interface
{
    public interface IPoolManager
    {
        bool CreatePool(GameObject prefab, int initialPoolSize, Transform poolRoot, bool persistent = false);

        GameObject GetObjectFromPool(string poolName);

        GameObject GetObjectFromPool<T>(string poolName, ObjectMaster objMaster) where T : IPoolable;

        Transform GetPool(string poolName);

        void ClearUnusedObjects(string poolName);

        void ResizePool(string poolName, int newSize);

        void ReturnObjectToPool(GameObject obj, string poolName);
    }
}