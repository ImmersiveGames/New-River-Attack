using System.Collections.Generic;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.PoolSystems.Interfaces;
using UnityEngine;

namespace ImmersiveGames.PoolSystems
{
    public class PoolObjectManager : IPoolManager
    {
        private readonly Dictionary<string, PoolObject> _objectPools = new Dictionary<string, PoolObject>();

        // Cria um novo pool de objetos
        public bool CreatePool(string poolName, GameObject prefab, int initialPoolSize, Transform poolRoot, bool persistent = false)
        {
            if (_objectPools.ContainsKey(poolName)) return false;

            var newPool = new PoolObject(poolName, prefab, initialPoolSize, poolRoot, persistent);
            _objectPools[poolName] = newPool;
            return true;
        }

        // Obtém um objeto do pool
        public GameObject GetObjectFromPool<T>(string poolName, Transform spawnPosition, T data) where T : ISpawnData
        {
            if (_objectPools.TryGetValue(poolName, out var pool))
            {
                return pool.GetObject(spawnPosition, data);
            }

            DebugManager.LogError<PoolObjectManager>($"Pool {poolName} not found!");
            return null;
        }

        // **Modificado**: Agora retorna o PoolObject em vez de apenas o Transform
        public PoolObject GetPool(string poolName)
        {
            return _objectPools.GetValueOrDefault(poolName);
        }

        // Limpa objetos não utilizados
        public void ClearUnusedObjects(string poolName)
        {
            if (_objectPools.TryGetValue(poolName, out var pool))
            {
                pool.ClearUnusedObjects();
            }
        }

        // Redimensiona o pool
        public void ResizePool(string poolName, int newSize)
        {
            if (_objectPools.TryGetValue(poolName, out var pool))
            {
                pool.ResizePool(newSize);
            }
        }

        // Retorna o objeto ao pool
        public void ReturnObjectToPool(GameObject obj, string poolName)
        {
            if (!_objectPools.TryGetValue(poolName, out var pool)) return;
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
