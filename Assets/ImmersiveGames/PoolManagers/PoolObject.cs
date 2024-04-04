using System.Collections.Generic;
using System.Linq;
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

        public PoolObject(GameObject prefab, int initialPoolSize, Transform myRoot, bool persistent = false)
        {
            _myRoot = new GameObject($"Pool({myRoot.root.name})").transform;
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
            if (poolable != null)
            {
                poolable.AssignPool(this);
            }

            return newObj;
        }

        internal GameObject GetObject()
        {
            foreach (var obj in _listInactiveObjects.Where(obj => obj != null))
            {
                _listInactiveObjects.Remove(obj);
                _listPooledObjects.Add(obj);
                ResetTransform(obj);
                obj.SetActive(true);
                obj.transform.parent = null;
                var poolable = obj.GetComponent<IPoolable>();
                poolable?.OnSpawned();
                return obj;
            }

            foreach (var obj in _listPooledObjects.Where(obj => !obj.activeSelf))
            {
                ResetTransform(obj);
                obj.SetActive(true);
                obj.transform.parent = null;
                var poolable = obj.GetComponent<IPoolable>();
                poolable?.OnSpawned();
                return obj;
            }
            return CreateObject(_pooledObj);
        }

        private void ResetTransform(GameObject gameObject)
        {
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;
        }

        public Transform GetRoot()
        {
            return _myRoot;
        }

        public void ClearUnusedObjects()
        {
            for (int i = _listInactiveObjects.Count - 1; i >= 0; i--)
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
