using System.Collections.Generic;
using UnityEngine;
using ImmersiveGames.PoolManagers.Interface;

namespace ImmersiveGames.PoolManagers
{
    public class PoolObjectManager : MonoBehaviour, IPoolManager
    {
        private static readonly Dictionary<string, PoolObject> ObjectPools = new Dictionary<string, PoolObject>();

        public bool CreatePool(GameObject prefab, int initialPoolSize, Transform poolRoot, bool persistent = false)
        {
            // Use o nome do objeto pai como base para o nome do pool
            var poolName = poolRoot.name + "(Pool)";

            if (ObjectPools.ContainsKey(poolName))
                return false;

            var newPool = new PoolObject(prefab, initialPoolSize, poolRoot, persistent);
            ObjectPools[poolName] = newPool;
            return true;
        }

        public GameObject GetObjectFromPool(string poolName)
        {
            return ObjectPools.TryGetValue(poolName, out var objectPool) ? objectPool.GetObject() : null;
        }

        public GameObject GetObjectFromPool<T>(string poolName, ObjectMaster objMaster) where T : IPoolable
        {
            if (!ObjectPools.TryGetValue(poolName, out var pool)) return null;
            var go = pool.GetObject();
            if (go.GetComponent<T>() is IPoolable { IsPooled: false } component)
                component.OnSpawned();
            if (go.GetComponent<IPoolable>() is { } poolable)
                poolable.Pool = pool; // Atribui a referência do pool ao objeto poolável
            return go;
        }

        public Transform GetPool(string poolName)
        {
            return ObjectPools.TryGetValue(poolName, out var pool) ? pool.GetRoot() : null;
        }

        public void ClearUnusedObjects(string poolName)
        {
            if (ObjectPools.TryGetValue(poolName, out var pool))
                pool.ClearUnusedObjects();
        }

        public void ResizePool(string poolName, int newSize)
        {
            if (ObjectPools.TryGetValue(poolName, out var pool))
                pool.ResizePool(newSize);
        }

        public void ReturnObjectToPool(GameObject obj, string poolName)
        {
            if (!ObjectPools.TryGetValue(poolName, out var pool)) return;
            // Verifica se o objeto ainda está na cena antes de retorná-lo para o pool
            if (obj.scene.IsValid())
            {
                pool.ReturnObject(obj);
            }
            else
            {
                Destroy(obj);
            }
        }
    }
}
