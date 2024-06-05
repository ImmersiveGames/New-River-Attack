using System.Linq;
using GD.MinMaxSlider;
using ImmersiveGames.DebugManagers;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NewRiverAttack.ObstaclesSystems.EnemiesSystems
{
    public class EnemiesDropItem : MonoBehaviour
    {
        private EnemiesMaster _enemiesMaster;
        
        private float _realDropChances;

        #region Unity Methods

        private void OnEnable()
        {
            SetInitialReferences();
            _enemiesMaster.EventObstacleHit += DropItem;
        }

        private void OnDisable()
        {
            _enemiesMaster.EventObstacleHit -= DropItem;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                DropItem(null);
            }
        }

        private void DropItem(PlayerMaster playerMaster)
        {
            var sort = Random.Range(0f,1f);
            DebugManager.Log<EnemiesDropItem>($"Sorteou {sort}");
            if (!(sort <= _realDropChances)) return;
            var drop = GetRandomDrop(_enemiesMaster.GetEnemySettings.dropItems);
            var position = transform.position;
            var dropPosition = new Vector3(position.x, 1f, position.z);
            Instantiate(drop, dropPosition, Quaternion.identity);
            DebugManager.Log<EnemiesDropItem>($"Caiu um item {sort} {drop.name}");
        }

        #endregion

        private void SetInitialReferences()
        {
            _enemiesMaster = GetComponent<EnemiesMaster>();
            _realDropChances = _enemiesMaster.GetEnemySettings.dropItemChances;
        }
        public static GameObject GetRandomDrop(EnemyDropData[] items)
        {
            // Calcular a soma total das chances
            var totalChance = items.Aggregate(0, (current, item) => (current + item.dropChance));

            // Gerar um número aleatório entre 0 e a soma total das chances
            var randomValue = Random.Range(0, totalChance);

            // Percorrer os itens e retornar o item cujo intervalo contém o valor aleatório
            var cumulativeChance = 0;
            foreach (var item in items)
            {
                cumulativeChance += item.dropChance;
                if (randomValue < cumulativeChance)
                {
                    return item.prefabPowerUp;
                }
            }

            // Se algo der errado, retornamos null, mas isso não deve acontecer
            DebugManager.LogWarning<EnemiesDropItem>("Failed to select a drop. Check the drop chances.");
            return null;
        }
    }
}