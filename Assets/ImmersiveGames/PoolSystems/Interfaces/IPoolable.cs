using UnityEngine;

namespace ImmersiveGames.PoolSystems.Interfaces
{
    public interface IPoolable
    {
        // Propriedade que referencia o pool ao qual o objeto pertence
        PoolObject Pool { get; set; }

        // Método chamado quando o objeto é ativado pelo pool
        void OnSpawned(Transform spawnPosition, ISpawnData data);
    }
}