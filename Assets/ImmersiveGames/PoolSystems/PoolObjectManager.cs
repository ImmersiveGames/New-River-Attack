using System.Collections.Generic;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.PoolSystems.Interfaces;
using UnityEngine;

namespace ImmersiveGames.PoolSystems
{
    public class PoolObjectManager : IPoolManager
    {
        private readonly Dictionary<string, PoolObject> _objectPools = new Dictionary<string, PoolObject>();

        // Cria um pool de objetos
        public bool CreatePool(string poolName, GameObject prefab, int initialPoolSize, Transform poolRoot, bool persistent = false)
        {
            if (_objectPools.ContainsKey(poolName)) return false;

            var newPool = new PoolObject(poolName, prefab, initialPoolSize, poolRoot, persistent);
            _objectPools[poolName] = newPool;
            return true;
        }

        // Obtém objeto de um pool genérico
        public GameObject GetObjectFromPool<T>(string poolName, Transform spawnPosition, T data) where T : ISpawnData
        {
            if (_objectPools.TryGetValue(poolName, out var pool))
            {
                return pool.GetObject(spawnPosition, data);
            }
            DebugManager.LogError<PoolObjectManager>($"Pool {poolName} not found!");
            return null;
        }

        // Obtém a raiz de um pool
        public Transform GetPool(string poolName)
        {
            return _objectPools.TryGetValue(poolName, out var pool) ? pool.GetRoot() : null;
        }

        // Limpa objetos inativos em um pool
        public void ClearUnusedObjects(string poolName)
        {
            if (_objectPools.TryGetValue(poolName, out var pool))
            {
                pool.ClearUnusedObjects();
            }
        }

        // Redimensiona um pool
        public void ResizePool(string poolName, int newSize)
        {
            if (_objectPools.TryGetValue(poolName, out var pool))
            {
                pool.ResizePool(newSize);
            }
        }

        // Retorna um objeto ao pool
        public void ReturnObjectToPool(GameObject obj, string poolName)
        {
            if (!_objectPools.TryGetValue(poolName, out var pool)) return;
            if (obj.scene.IsValid())
            {
                pool.ReturnObject(obj);
            }
            else
            {
                Object.Destroy(obj); // Destrói se não estiver em cena
            }
        }
    }
}
