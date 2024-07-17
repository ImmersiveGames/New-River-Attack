using System.Linq;
using GD.MinMaxSlider;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.Utils;
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
            _enemiesMaster.EventObstacleDeath += DropItem;
        }

        private void OnDisable()
        {
            _enemiesMaster.EventObstacleDeath -= DropItem;
        }

        private void DropItem(PlayerMaster playerMaster)
        {
            var sort = Random.Range(0f,1f);
            DebugManager.Log<EnemiesDropItem>($"Sorteou {sort}");
            if (!(sort <= _realDropChances)) return;
            var drop = Tools.GetRandomDrop(_enemiesMaster.GetEnemySettings.dropItems);
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
    }
}