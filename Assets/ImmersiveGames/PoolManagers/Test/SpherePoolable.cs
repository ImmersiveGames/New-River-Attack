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
        public void OnSpawned()
        {
            Debug.Log("SpherePoolable object spawned.");
            IsPooled = false;
        }

        public void OnDespawned()
        {
            Debug.Log("SpherePoolable object despawned.");
            IsPooled = true;
        }

        private void OnEnable()
        {
            Debug.Log("SpherePoolable object enabled.");
        }

        private void OnDisable()
        {
            Debug.Log("SpherePoolable object disabled.");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            Debug.Log("SpherePoolable object triggered by player.");
            gameObject.SetActive(false); // Desativa o objeto ao colidir com o jogador
        }
    }
}