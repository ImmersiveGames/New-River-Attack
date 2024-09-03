using System.Collections.Generic;
using ImmersiveGames.BulletsManagers;
using ImmersiveGames.DebugManagers;
using UnityEngine;
using ImmersiveGames.PoolManagers.Interface;

namespace ImmersiveGames.PoolManagers
{
    public class PoolObjectManager : IPoolManager
    {
        private readonly Dictionary<string, PoolObject> _objectPools = new Dictionary<string, PoolObject>();

        public bool CreatePool(string poolName, GameObject prefab, int initialPoolSize, Transform poolRoot, bool persistent = false)
        {
            if (_objectPools.ContainsKey(poolName))
                return false;

            var newPool = new PoolObject(poolName, prefab, initialPoolSize, poolRoot, persistent);
            _objectPools[poolName] = newPool;
            return true;
        }
        
        public GameObject GetObjectFromPool<T>(string poolName, Transform spawnPosition, BulletData bulletData) where T : IPoolable
        {
            if (_objectPools.TryGetValue(poolName, out var pool)) return pool.GetObject(spawnPosition, bulletData);
            DebugManager.LogError<PoolObjectManager>($"Pool {poolName} não encontrado!");
            return null;
        }


        public Transform GetPool(string poolName)
        {
            return _objectPools.TryGetValue(poolName, out var pool) ? pool.GetRoot() : null;
        }

        public void ClearUnusedObjects(string poolName)
        {
            if (_objectPools.TryGetValue(poolName, out var pool))
                pool.ClearUnusedObjects();
        }

        public void ResizePool(string poolName, int newSize)
        {
            if (_objectPools.TryGetValue(poolName, out var pool))
                pool.ResizePool(newSize);
        }

        public void ReturnObjectToPool(GameObject obj, string poolName)
        {
            if (!_objectPools.TryGetValue(poolName, out var pool)) return;
            // Verifica se o objeto ainda está na cena antes de retorná-lo para o pool
            if (obj.scene.IsValid())
            {
                pool.ReturnObject(obj);
            }
            else
            {
                Object.Destroy(obj);
            }
        }
        
    }
}
