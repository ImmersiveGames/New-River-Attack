using System.Collections.Generic;
using System.Linq;
using ImmersiveGames.BulletsManagers;
using ImmersiveGames.DebugManagers;
using UnityEngine;
using ImmersiveGames.PoolManagers.Interface;

namespace ImmersiveGames.PoolManagers
{
    public class PoolObject
    {
        private readonly MonoBehaviour _mono;
        private readonly List<GameObject> _listPooledObjects = new List<GameObject>();
        private readonly List<GameObject> _listInactiveObjects = new List<GameObject>();
        private readonly GameObject _pooledObj;
        private readonly Transform _myRoot;

        public PoolObject(string poolName, GameObject prefab, int initialPoolSize, Transform myRoot, bool persistent = false)
        {
            _myRoot = new GameObject(poolName).transform;
            _myRoot.SetParent(myRoot);
            _myRoot.SetAsLastSibling();

            for (var i = 0; i < initialPoolSize; i++)
            {
                CreateObject(prefab);
            }

            if (persistent)
                Object.DontDestroyOnLoad(_myRoot.gameObject);

            _pooledObj = prefab;
        }

        private GameObject CreateObject(GameObject prefab)
        {
            var newObj = Object.Instantiate(prefab, _myRoot);
            newObj.SetActive(false);
            _listPooledObjects.Add(newObj);

            // Verifica se o objeto possui um componente IPoolable e atribui a referência do pool
            var poolable = newObj.GetComponent<IPoolable>();
            if (poolable != null) poolable.Pool = this;

            return newObj;
        }

        internal GameObject GetObject(Transform spawnPosition, BulletData bulletData)
        {
            foreach (var obj in _listInactiveObjects.Where(obj => obj != null))
            {
                _listInactiveObjects.Remove(obj);
                _listPooledObjects.Add(obj);
                return SpawnObject(obj, spawnPosition, bulletData);
            }

            foreach (var obj in _listPooledObjects.Where(obj => !obj.activeSelf))
            {
                return SpawnObject(obj, spawnPosition, bulletData);
            }
            return CreateObject(_pooledObj);
        }

        private GameObject SpawnObject(GameObject obj, Transform spawnPosition, BulletData bulletData)
        {
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            obj.SetActive(true);
            var poolable = obj.GetComponent<IPoolable>();
            if (poolable != null)
            {
                poolable.Pool = this;
                poolable.OnSpawned(spawnPosition, bulletData);
            }
            obj.transform.parent = null;
            return obj;
        }

        public Transform GetRoot()
        {
            return _myRoot;
        }

        public void ClearUnusedObjects()
        {
            for (var i = _listInactiveObjects.Count - 1; i >= 0; i--)
            {
                if (_listInactiveObjects[i] == null)
                {
                    _listInactiveObjects.RemoveAt(i);
                }
                else if (_listInactiveObjects[i].gameObject == null)
                {
                    Object.DestroyImmediate(_listInactiveObjects[i]);
                    _listInactiveObjects.RemoveAt(i);
                }
            }
        }

        public void ResizePool(int newSize)
        {
            if (newSize < _listPooledObjects.Count)
            {
                var inactiveCount = _listInactiveObjects.Count;
                for (var i = inactiveCount - 1; i >= 0; i--)
                {
                    if (_listInactiveObjects[i] == null) continue;
                    Object.Destroy(_listInactiveObjects[i]);
                    _listInactiveObjects.RemoveAt(i);
                }

                for (var i = _listPooledObjects.Count - 1; i >= newSize; i--)
                {
                    if (!_listPooledObjects[i].activeSelf)
                    {
                        _listInactiveObjects.Add(_listPooledObjects[i]);
                        _listPooledObjects.RemoveAt(i);
                    }
                    else
                    {
                        DebugManager.LogWarning<PoolObject>("Trying to resize pool while active objects exist. Consider clearing unused objects first.");
                        break;
                    }
                }
            }
            else if (newSize > _listPooledObjects.Count)
            {
                for (var i = _listPooledObjects.Count; i < newSize; i++)
                {
                    CreateObject(_pooledObj);
                }
            }
        }

        public void ReturnObject(GameObject obj)
        {
            if (obj == null) return;
            obj.SetActive(false);
            obj.transform.SetParent(_myRoot);
            _listInactiveObjects.Add(obj);
        }
    }
}
