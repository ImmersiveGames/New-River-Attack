using ImmersiveGames.ObjectManagers.DetectManagers;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.Abstracts;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Helpers
{
    public class RandomSpawnHelper
    {
        private readonly float _safeDistance;
        private readonly List<Vector3> _spawnedPositions;
        private readonly Transform _bossTransform;
        private readonly Camera _playerCamera;
        private readonly DetectPlayerApproach _detectPlayerApproach;
        private readonly LayerMask _enemyLayerMask; // Layer para detectar inimigos

        public RandomSpawnHelper(float safeDistance, Transform bossTransform, Camera playerCamera, float detectDistance,
            LayerMask enemyLayerMask)
        {
            _safeDistance = safeDistance;
            _bossTransform = bossTransform;
            _playerCamera = playerCamera;
            _enemyLayerMask = enemyLayerMask;
            _spawnedPositions = new List<Vector3>();

            // Instanciando DetectPlayerApproach com a distância de detecção
            _detectPlayerApproach = new DetectPlayerApproach(detectDistance);
        }

        public Vector3? GetValidSpawnPosition<T>() where T : ObstacleMaster
        {
            var resetAttempts = 0; // Limite para evitar loops infinitos
            const int maxResets = 5; // Limite de resets permitidos
            while (resetAttempts < maxResets)
            {
                for (var attempts = 0; attempts < 100; attempts++)
                {
                    var position = GenerateRandomPosition();

                    // Agora testamos todas as condições para essa mesma posição
                    if (!IsPositionValid<T>(position)) continue;
                    _spawnedPositions.Add(position);
                    return position;
                }

                // Se todas as tentativas falharem, resetamos a lista
                Debug.LogWarning("Todas as posições estão muito próximas! Resetando posições spawnadas.");
                _spawnedPositions.Clear();
                resetAttempts++; // Contabiliza o reset
            }

            Debug.LogError("Falha ao encontrar uma posição válida após múltiplos resets.");
            return null; // Retorna null após o máximo de resets
        }

        private bool IsPositionValid<T>(Vector3 position) where T : ObstacleMaster
        {
            // Testar todas as condições para a mesma posição
            if (!IsPositionInView(position)) return false;
            if (IsTooCloseToSpawned(position)) return false;
            if (IsTooCloseToBoss(position)) return false;
            if (IsNearEnemy<T>(position)) return false;

            // Se passar por todas as condições, a posição é válida
            return true;
        }

        private bool IsTooCloseToSpawned(Vector3 position)
        {
            return _spawnedPositions.Any(spawnedPos => Vector3.Distance(position, spawnedPos) < _safeDistance);
        }

        private bool IsTooCloseToBoss(Vector3 position)
        {
            return Vector3.Distance(position, _bossTransform.position) < _safeDistance;
        }

        private bool IsNearEnemy<T>(Vector3 position) where T : ObstacleMaster
        {
            var enemyNearby = _detectPlayerApproach.TargetApproach<T>(position, _enemyLayerMask);
            if (enemyNearby == null) return false;
            Debug.Log($"Posição inválida! EnemiesMaster detectado perto da posição {position}");
            return true;
        }
        
        private bool IsPositionInView(Vector3 position)
        {
            var viewportPoint = _playerCamera.WorldToViewportPoint(position);
            return viewportPoint.x is >= 0 and <= 1 &&
                   viewportPoint.y is >= 0 and <= 1 &&
                   viewportPoint.z > 0;
        }

        private Vector3 GenerateRandomPosition()
        {
            var bossAreaX = GamePlayBossManager.instance.bossAreaX;
            var bossAreaZ = GamePlayBossManager.instance.bossAreaZ;

            var randomX = Random.Range(Mathf.FloorToInt(bossAreaX.x), Mathf.CeilToInt(bossAreaX.y));
            var randomZ = Random.Range(Mathf.FloorToInt(bossAreaZ.x), Mathf.CeilToInt(bossAreaZ.y));

            return new Vector3(randomX, 0, randomZ);
        }
    }
}