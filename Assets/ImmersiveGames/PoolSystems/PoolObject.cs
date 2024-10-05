using System.Collections.Generic;
using ImmersiveGames.PoolSystems.Interfaces;
using UnityEngine;

namespace ImmersiveGames.PoolSystems
{
    public class PoolObject
    {
        private readonly List<GameObject> _pooledObjects = new List<GameObject>();
        private readonly GameObject _prefab;
        private readonly Transform _root;

        public PoolObject(string poolName, GameObject prefab, int initialPoolSize, Transform parent, bool persistent = false)
        {
            _root = new GameObject(poolName).transform;
            _root.SetParent(parent);
            _root.SetAsLastSibling();

            _prefab = prefab;

            for (var i = 0; i < initialPoolSize; i++)
            {
                var newObj = CreateObject();
                _pooledObjects.Add(newObj);
            }

            if (persistent)
            {
                Object.DontDestroyOnLoad(_root.gameObject);
            }
        }

        private GameObject CreateObject()
        {
            var newObj = Object.Instantiate(_prefab, _root);
            newObj.SetActive(false);
            AssignPoolable(newObj);
            return newObj;
        }

        private void AssignPoolable(GameObject obj)
        {
            var poolable = obj.GetComponent<IPoolable>();
            if (poolable != null)
            {
                poolable.Pool = this;
            }
        }

        private GameObject FindInactiveObject()
        {
            foreach (var obj in _pooledObjects)
            {
                if (!obj.activeSelf)
                {
                    return obj;
                }
            }
            return null;
        }

        // O método usa o tipo genérico T para passar o tipo correto de ISpawnData
        internal GameObject GetObject<T>(Transform spawnPosition, T data) where T : ISpawnData
        {
            var obj = FindInactiveObject() ?? CreateObject();
            return SpawnObject(obj, spawnPosition, data);
        }

        private GameObject SpawnObject<T>(GameObject obj, Transform spawnPosition, T data) where T : ISpawnData
        {
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            obj.SetActive(true);

            var poolable = obj.GetComponent<IPoolable>();
            poolable?.OnSpawned(spawnPosition, data);

            obj.transform.parent = null;
            return obj;
        }

        public Transform GetRoot() => _root;

        public void ClearUnusedObjects()
        {
            _pooledObjects.RemoveAll(obj => obj == null);
        }

        public void ResizePool(int newSize)
        {
            if (newSize < _pooledObjects.Count)
            {
                for (var i = _pooledObjects.Count - 1; i >= newSize; i--)
                {
                    if (_pooledObjects[i].activeSelf) continue;
                    Object.Destroy(_pooledObjects[i]);
                    _pooledObjects.RemoveAt(i);
                }
            }
            else
            {
                for (var i = _pooledObjects.Count; i < newSize; i++)
                {
                    _pooledObjects.Add(CreateObject());
                }
            }
        }

        public void ReturnObject(GameObject obj)
        {
            if (obj == null) return;
            obj.SetActive(false);
            obj.transform.SetParent(_root);
        }
    }
}
