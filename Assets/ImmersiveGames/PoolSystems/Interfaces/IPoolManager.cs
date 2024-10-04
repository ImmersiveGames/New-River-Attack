using UnityEngine;

namespace ImmersiveGames.PoolSystems.Interfaces
{
    public interface IPoolManager
    {
        // Método para criar um novo pool
        bool CreatePool(string poolName, GameObject prefab, int initialPoolSize, Transform poolRoot, bool persistent = false);

        // Método para pegar um objeto de um pool específico
        GameObject GetObjectFromPool<T>(string poolName, Transform spawnPosition, T data) where T : ISpawnData;

        // Método para devolver um objeto ao pool
        void ReturnObjectToPool(GameObject obj, string poolName);

        // Retorna a raiz do pool
        Transform GetPool(string poolName);

        // Limpa os objetos inativos do pool
        void ClearUnusedObjects(string poolName);

        // Redimensiona um pool
        void ResizePool(string poolName, int newSize);
    }
}