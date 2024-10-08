using System.Collections.Generic;
using System.Linq;
using NewRiverAttack.GamePlayManagers;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Helpers
{
    public class RandomSpawnHelper
    {
        private readonly float _safeDistance;
        private List<Vector3> SpawnedPositions { get; }
        private readonly Transform _bossTransform;
        private readonly Camera _playerCamera;

        public RandomSpawnHelper(float safeDistance, Transform bossTransform, Camera playerCamera)
        {
            _safeDistance = safeDistance;
            _bossTransform = bossTransform;
            _playerCamera = playerCamera;
            SpawnedPositions = new List<Vector3>();
        }

        public RandomSpawnHelper(List<Vector3> spawnedPositions)
        {
            SpawnedPositions = spawnedPositions;
        }

        public Vector3? GetValidSpawnPosition()
        {
            for (var attempts = 0; attempts < 100; attempts++)
            {
                var position = GenerateRandomPosition();
                if (IsPositionValid(position)) return position;
            }

            return null;
        }

        private Vector3 GenerateRandomPosition()
        {
            var bossAreaX = GamePlayBossManager.instance.bossAreaX;
            var bossAreaZ = GamePlayBossManager.instance.bossAreaZ;

            var randomX = Random.Range(Mathf.FloorToInt(bossAreaX.x), Mathf.CeilToInt(bossAreaX.y));
            var randomZ = Random.Range(Mathf.FloorToInt(bossAreaZ.x), Mathf.CeilToInt(bossAreaZ.y));

            return new Vector3(randomX, 0, randomZ);
        }

        private bool IsPositionValid(Vector3 position)
        {
            if (!IsPositionInView(position)) return false;
            if (SpawnedPositions.Any(spawnedPos => Vector3.Distance(position, spawnedPos) < _safeDistance)) return false;
            return Vector3.Distance(position, _bossTransform.position) >= _safeDistance;
        }

        private bool IsPositionInView(Vector3 position)
        {
            var viewportPoint = _playerCamera.WorldToViewportPoint(position);
            return viewportPoint.x is >= 0 and <= 1 &&
                   viewportPoint.y is >= 0 and <= 1 &&
                   viewportPoint.z > 0;
        }
    }
}