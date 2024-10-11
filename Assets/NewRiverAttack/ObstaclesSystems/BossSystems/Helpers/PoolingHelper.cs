using ImmersiveGames.PoolSystems;
using ImmersiveGames.PoolSystems.Interfaces;
using NewRiverAttack.BulletsManagers.Interface;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Helpers
{
    public class PoolingHelper
    {
        private readonly PoolObject _poolObject;

        public PoolingHelper(GameObject prefab, Transform parent, string poolName, int initialSize, bool persistent)
        {
            IPoolManager poolManager = new PoolObjectManager();
            poolManager.CreatePool(poolName, prefab, initialSize, parent, persistent);
            _poolObject = poolManager.GetPool(poolName);
        }
        public void GetObject<T>(Transform spawnPosition, T data) where T : ISpawnData
        {
            _poolObject.GetObject(spawnPosition, data);
        }
        
        public void MarkForReturn(GameObject obj)
        {
            _poolObject.MarkForReturn(obj);
        }
        public void ReturnMarkedObjects()
        {
            _poolObject.ReturnMarkedObjects();
        }

        public void ReturnObject(GameObject obj)
        {
            _poolObject.ReturnObject(obj);
        }

        public void ReturnAllActiveObjects()
        {
            _poolObject.ReturnAllActiveObjects();
        }
        
    }
}