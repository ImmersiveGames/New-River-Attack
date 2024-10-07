using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ImmersiveGames.BehaviorTreeSystem.Interface;
using ImmersiveGames.DebugManagers;
using NewRiverAttack.BulletsManagers.Interface;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.Abstracts;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class BossBehaviorRandomSpawn : ObjectShoot, INodeFunctionProvider
    {
        private BossMaster _bossMaster;
        private Camera _playerCamera;

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

        private readonly List<Vector3> _spawnedPositions = new();
        private Coroutine _spawnRoutine;
        private NodeState _currentNodeState = NodeState.Running;
        private int _spawnedCount;
        private bool _hasCompleted;

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();
            _bossMaster = GetComponent<BossMaster>();
            _playerCamera = Camera.main;
        }

        protected override void OnDisable()
        {
            StopSpawnRoutine();
            base.OnDisable();
        }

        #endregion

        #region INodeFunctionProvider Implementation

        public string NodeName => $"BossRandomSpawn_{idNode}";
        public int NodeID => idNode;

        public Func<NodeState> GetNodeFunction() => SpawnItems;

        #endregion

        #region Spawn Logic

        private NodeState SpawnItems()
        {
            if (_hasCompleted) return NodeState.Success;

            if (_spawnRoutine != null) return _currentNodeState;
            _spawnedCount = 0;
            _spawnRoutine = StartCoroutine(SpawnItemsCoroutine());
            _currentNodeState = NodeState.Running;

            return _currentNodeState;
        }

        private IEnumerator SpawnItemsCoroutine()
        {
            _spawnedPositions.Clear();
            var tempSpawnPoint = new GameObject("TempSpawnPoint").transform; // Cria um GameObject temporário

            while (_spawnedCount < itemCount)
            {
                var randomPosition = GetValidSpawnPosition();
                if (randomPosition == null) continue;

                SpawnItem(randomPosition.Value, tempSpawnPoint);
                _spawnedCount++;

                yield return new WaitForSeconds(baseCadence + UnityEngine.Random.Range(-cadenceVariance, cadenceVariance));
            }

            Destroy(tempSpawnPoint.gameObject); // Destrói o GameObject temporário após o uso
            _currentNodeState = NodeState.Success;
            _hasCompleted = true;
            _spawnRoutine = null;
            ReturnMarkedObjects();
        }


        private void SpawnItem(Vector3 position, Transform tempSpawnPoint)
        {
            tempSpawnPoint.position = position;
            var itemData = CreateItemData(position);
            base.Fire(itemData, tempSpawnPoint);
            _spawnedPositions.Add(position);
            DebugManager.Log<BossBehaviorRandomSpawn>($"Objeto spawnado na posição: {position}");
        }

        private Vector3? GetValidSpawnPosition()
        {
            for (var attempts = 0; attempts < 100; attempts++)
            {
                var position = GenerateRandomPosition();
                if (IsPositionValid(position)) return position;
            }

            DebugManager.LogWarning<BossBehaviorRandomSpawn>("Não foi possível encontrar uma posição válida para o objeto após várias tentativas.");
            return null;
        }

        private Vector3 GenerateRandomPosition()
        {
            var bossAreaX = GamePlayBossManager.instance.bossAreaX;
            var bossAreaZ = GamePlayBossManager.instance.bossAreaZ;

            var randomX = UnityEngine.Random.Range(Mathf.FloorToInt(bossAreaX.x), Mathf.CeilToInt(bossAreaX.y));
            var randomZ = UnityEngine.Random.Range(Mathf.FloorToInt(bossAreaZ.x), Mathf.CeilToInt(bossAreaZ.y));

            return new Vector3(randomX, 0, randomZ);
        }

        private bool IsPositionValid(Vector3 position)
        {
            if (!IsPositionInView(position)) return false;

            if (_spawnedPositions.Any(spawnedPos => Vector3.Distance(position, spawnedPos) < safeDistance))
            {
                return false;
            }

            return Vector3.Distance(position, _bossMaster.transform.position) >= safeDistance;
        }

        private bool IsPositionInView(Vector3 position)
        {
            var viewportPoint = _playerCamera.WorldToViewportPoint(position);
            return viewportPoint.x is >= 0 and <= 1 &&
                   viewportPoint.y is >= 0 and <= 1 &&
                   viewportPoint.z > 0;
        }

        private BulletSpawnData CreateItemData(Vector3 position)
        {
            return new BulletSpawnData(
                _bossMaster,
                Vector3.zero, 
                position,
                itemDamage,
                itemSpeed,
                itemLifetime,
                false
            );
        }

        #endregion

        #region Override de Métodos Herdados de ObjectShoot

        protected override float GetCadenceShoot() => baseCadence;

        protected override BulletSpawnData CreateBulletData(Vector3 direction, Vector3 position) => null;

        public void OnEnter()
        {
            ResetShoot();
        }
        public override void ResetShoot()
        {
            StopSpawnRoutine();
            ResetSpawnedObjects();
            _spawnedPositions.Clear();
            _hasCompleted = false;
        }

        private void StopSpawnRoutine()
        {
            if (_spawnRoutine != null)
            {
                StopCoroutine(_spawnRoutine);
                _spawnRoutine = null;
            }
            ReturnMarkedObjects();
        }

        #endregion
    }
}
