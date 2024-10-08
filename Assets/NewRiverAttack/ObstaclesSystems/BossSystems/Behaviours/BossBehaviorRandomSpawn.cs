using System;
using System.Collections;
using UnityEngine;
using ImmersiveGames.BehaviorTreeSystem.Interface;
using NewRiverAttack.BulletsManagers.Interface;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.BossSystems.Helpers;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class BossBehaviorRandomSpawn : ObjectShoot, INodeFunctionProvider
    {
        private BossMaster _bossMaster;
        private RandomSpawnHelper _spawnHelper;

        [Header("Node Reference")]
        public int idNode;

        [Header("Spawn Settings")]
        [SerializeField] private int itemCount = 5;
        [SerializeField] private float safeDistance = 2f;

        [Header("Item Data Settings")]
        [SerializeField] private int itemDamage = 1;
        [SerializeField] private float itemSpeed = 10f;
        [SerializeField] private float itemLifetime = 5f;

        [Header("Cooldown Settings")]
        [SerializeField] private float baseCadence = 1.5f;
        [SerializeField] private float cadenceVariance = 0.5f;

        private Coroutine _spawnRoutine;
        private bool _hasCompleted;

        protected override void Awake()
        {
            base.Awake();
            _bossMaster = GetComponent<BossMaster>();
            _spawnHelper = new RandomSpawnHelper(safeDistance, _bossMaster.transform, Camera.main);
        }

        public string NodeName => $"BossRandomSpawn_{idNode}";
        public int NodeID => idNode;

        public Func<NodeState> GetNodeFunction() => ExecuteSpawnBehavior;

        private NodeState ExecuteSpawnBehavior()
        {
            if (_hasCompleted) return NodeState.Success; 
            if (_spawnRoutine == null) _spawnRoutine = StartCoroutine(SpawnItemsCoroutine());
            return NodeState.Running; 
        }

        private IEnumerator SpawnItemsCoroutine()
        {
            var tempSpawnPoint = new GameObject("TempSpawnPoint").transform;

            for (var i = 0; i < itemCount; i++)
            {
                var randomPosition = _spawnHelper.GetValidSpawnPosition();
                if (randomPosition.HasValue)
                {
                    SpawnItem(randomPosition.Value, tempSpawnPoint);
                }
                yield return new WaitForSeconds(baseCadence + UnityEngine.Random.Range(-cadenceVariance, cadenceVariance));
            }

            Destroy(tempSpawnPoint.gameObject);

            _hasCompleted = true;
            _spawnRoutine = null;
        }

        private void SpawnItem(Vector3 position, Transform tempSpawnPoint)
        {
            tempSpawnPoint.position = position;
            var itemData = CreateBulletData(Vector3.zero, position);
            Fire(itemData, tempSpawnPoint); 
        }

        public override float GetCadenceShoot => baseCadence;

        public override void ResetShoot()
        {
            if (_spawnRoutine != null)
            {
                StopCoroutine(_spawnRoutine);
                _spawnRoutine = null;
            }
            _hasCompleted = false;
        }

        public override BulletSpawnData CreateBulletData(Vector3 direction, Vector3 position)
        {
            return new BulletSpawnData(
                _bossMaster,
                direction,
                position,
                itemDamage,
                itemSpeed,
                itemLifetime,
                false
            );
        }
    }
}
