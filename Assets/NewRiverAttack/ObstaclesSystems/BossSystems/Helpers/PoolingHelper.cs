﻿using ImmersiveGames.PoolSystems;
using ImmersiveGames.PoolSystems.Interfaces;
using NewRiverAttack.BulletsManagers.Interface;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Helpers
{
    public class PoolingHelper
    {
        private PoolObject _poolObject;

        public PoolingHelper(GameObject prefab, Transform parent, string poolName, int initialSize, bool persistent)
        {
            IPoolManager poolManager = new PoolObjectManager();
            poolManager.CreatePool(poolName, prefab, initialSize, parent, persistent);
            _poolObject = poolManager.GetPool(poolName);
        }

        /*public void ReturnMarkedObjects()
        {
            _poolObject.ReturnMarkedObjects();
        }

        public void ReturnAllActiveObjects()
        {
            _poolObject.ReturnAllActiveObjects();
        }

        public void MarkForReturn(GameObject obj)
        {
            _poolObject.MarkForReturn(obj);
        }*/

        public void GetObject(Transform spawnPoint, BulletSpawnData data)
        {
            _poolObject.GetObject(spawnPoint, data);
        }
    }
}