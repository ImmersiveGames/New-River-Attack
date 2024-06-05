using ImmersiveGames.BulletsManagers;
using ImmersiveGames.DebugManagers;
using UnityEngine;
using ImmersiveGames.PoolManagers.Interface;

namespace ImmersiveGames.PoolManagers.Test
{
    public class SpherePoolable : MonoBehaviour, IPoolable
    {
        public bool IsPooled { get; private set; }

        public PoolObject Pool { get; set; }

        public void AssignPool(PoolObject pool)
        {
            Pool = pool;
        }
        public void OnSpawned(Transform spawnPosition, IBulletsData bulletData)
        {
            DebugManager.Log<SpherePoolable>("SpherePoolable object spawned.");
            IsPooled = false;
        }

        public void OnDespaired()
        {
            DebugManager.Log<SpherePoolable>("SpherePoolable object despawned.");
            IsPooled = true;
        }

        private void OnEnable()
        {
            DebugManager.Log<SpherePoolable>("SpherePoolable object enabled.");
        }

        private void OnDisable()
        {
            DebugManager.Log<SpherePoolable>("SpherePoolable object disabled.");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            DebugManager.Log<SpherePoolable>("SpherePoolable object triggered by player.");
            gameObject.SetActive(false); // Desativa o objeto ao colidir com o jogador
        }
    }
}