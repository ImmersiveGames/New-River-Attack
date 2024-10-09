using System.Collections.Generic;
using System.Linq;
using ImmersiveGames.PoolSystems.Interfaces;
using UnityEngine;

namespace ImmersiveGames.PoolSystems
{
    public class PoolObject
    {
        private readonly List<GameObject> _pooledObjects;  // Objetos no pool
        private readonly List<GameObject> _activeObjects;  // Objetos ativos
        private readonly List<GameObject> _markedForReturn;  // Objetos marcados para retorno

        private readonly GameObject _prefab;
        private readonly Transform _root;

        // Construtor para inicializar o PoolObject com os parâmetros necessários
        public PoolObject(string poolName, GameObject prefab, int initialPoolSize, Transform parent, bool persistent = false)
        {
            _root = new GameObject(poolName).transform;
            _root.SetParent(parent);
            _root.SetAsLastSibling();

            _pooledObjects = new List<GameObject>();
            _activeObjects = new List<GameObject>();
            _markedForReturn = new List<GameObject>();

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

        // Cria um novo objeto e o associa ao pool
        private GameObject CreateObject()
        {
            var newObj = Object.Instantiate(_prefab, _root);
            newObj.SetActive(false);
            AssignPoolable(newObj);
            return newObj;
        }

        // Associa o IPoolable ao objeto
        private void AssignPoolable(GameObject obj)
        {
            var poolable = obj.GetComponent<IPoolable>();
            if (poolable != null)
            {
                poolable.Pool = this;
            }
        }

        // Busca por um objeto inativo dentro do pool
        private GameObject FindInactiveObject()
        {
            return _pooledObjects.FirstOrDefault(obj => !obj.activeSelf);
        }

        // Método que obtém um objeto do pool e o ativa
        internal GameObject GetObject<T>(Transform spawnPosition, T data) where T : ISpawnData
        {
            var obj = FindInactiveObject() ?? CreateObject();
            _pooledObjects.Add(obj);  // Certifique-se de que o novo objeto seja adicionado ao pool
            _activeObjects.Add(obj);  // Adiciona à lista de objetos ativos
            return SpawnObject(obj, spawnPosition, data);
        }

        // Ativa o objeto, configura sua posição e dados, e o retira do pool
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

        // Marca um objeto para retornar ao pool após a conclusão do ciclo de spawn
        public void MarkForReturn(GameObject obj)
        {
            if (_markedForReturn.Contains(obj)) return;
            //Debug.Log($"Marcado: {obj}");
            _markedForReturn.Add(obj);  // Marca o objeto para retorno
        }

        // Retorna todos os objetos que foram marcados para retorno ao pool
        public void ReturnMarkedObjects()
        {
            // Use um loop 'for' para evitar erros de modificação de coleção durante a enumeração
            for (var i = _markedForReturn.Count - 1; i >= 0; i--)
            {
                //Debug.Log($"Return Objects {_markedForReturn[i]}");
                ReturnObject(_markedForReturn[i]);
            }
            _markedForReturn.Clear();  // Limpa a lista de marcados
        }

        // Retorna todos os objetos ativos ao pool
        public void ReturnAllActiveObjects()
        {
            // Iteração segura enquanto modifica a lista de ativos
            for (var i = _activeObjects.Count - 1; i >= 0; i--)
            {
                ReturnObject(_activeObjects[i]);
            }
            _activeObjects.Clear();  // Limpa a lista de objetos ativos
        }
        // Retorna um objeto ao pool
        public void ReturnObject(GameObject obj)
        {
            if (obj == null) return;

            var poolable = obj.GetComponent<IPoolable>(); // Obtém o componente que implementa a interface IPoolable
            poolable?.OnReturnedToPool();  // Chama o método para resetar o objeto

            obj.SetActive(false);
            obj.transform.SetParent(_root);
            _activeObjects.Remove(obj);  // Remove da lista de ativos
        }

        // Método que limpa objetos nulos da lista de objetos poolados
        public void ClearUnusedObjects()
        {
            _pooledObjects.RemoveAll(obj => obj == null);
        }

        // Redimensiona o pool de acordo com o tamanho solicitado
        public void ResizePool(int newSize)
        {
            if (newSize < _pooledObjects.Count)
            {
                // Se o novo tamanho for menor, destrua objetos inativos e ajuste o tamanho
                for (var i = _pooledObjects.Count - 1; i >= newSize; i--)
                {
                    if (_pooledObjects[i].activeSelf) continue;  // Pula os objetos ativos
                    Object.Destroy(_pooledObjects[i]);
                    _pooledObjects.RemoveAt(i);
                }
            }
            else
            {
                // Se o novo tamanho for maior, crie novos objetos e adicione ao pool
                for (var i = _pooledObjects.Count; i < newSize; i++)
                {
                    var newObj = CreateObject();
                    _pooledObjects.Add(newObj);
                }
            }
        }
    }
}
