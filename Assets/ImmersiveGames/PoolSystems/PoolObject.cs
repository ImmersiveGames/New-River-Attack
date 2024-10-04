using System.Collections.Generic;
using System.Linq;
using ImmersiveGames.DebugManagers;
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

        // Método separado para criar um novo objeto
        private GameObject CreateObject()
        {
            var newObj = Object.Instantiate(_prefab, _root);
            newObj.SetActive(false);
            AssignPoolable(newObj); // Configura o poolable
            return newObj;
        }

        // Método para configurar um IPoolable
        private void AssignPoolable(GameObject obj)
        {
            var poolable = obj.GetComponent<IPoolable>();
            if (poolable != null)
            {
                poolable.Pool = this;
            }
        }

        // Método separado para encontrar um objeto inativo
        private GameObject FindInactiveObject()
        {
            return _pooledObjects.FirstOrDefault(obj => !obj.activeSelf);
        }

        // Obtenção do objeto reutilizado ou criação de novo
        internal GameObject GetObject<T>(Transform spawnPosition, T data) where T : ISpawnData
        {
            var obj = FindInactiveObject() ?? CreateObject(); // Busca ou cria objeto
            return SpawnObject(obj, spawnPosition, data); // Spawn objeto
        }

        // Método modular para spawnar o objeto
        private GameObject SpawnObject<T>(GameObject obj, Transform spawnPosition, T data) where T : ISpawnData
        {
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            obj.SetActive(true);
            
            var poolable = obj.GetComponent<IPoolable>();
            poolable?.OnSpawned(spawnPosition, data); // Chama evento customizado

            obj.transform.parent = null;
            return obj;
        }

        public Transform GetRoot() => _root;

        // Limpeza de objetos inativos
        public void ClearUnusedObjects()
        {
            _pooledObjects.RemoveAll(obj => obj == null);
        }

        // Redimensiona o pool
        public void ResizePool(int newSize)
        {
            if (newSize < _pooledObjects.Count)
            {
                for (var i = _pooledObjects.Count - 1; i >= newSize; i--)
                {
                    if (!_pooledObjects[i].activeSelf)
                    {
                        Object.Destroy(_pooledObjects[i]);
                        _pooledObjects.RemoveAt(i);
                    }
                    else
                    {
                        DebugManager.LogWarning<PoolObject>("Cannot resize while objects are active.");
                    }
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

        // Retorna o objeto ao pool
        public void ReturnObject(GameObject obj)
        {
            if (obj == null) return;
            obj.SetActive(false);
            obj.transform.SetParent(_root);
        }
    }
}
