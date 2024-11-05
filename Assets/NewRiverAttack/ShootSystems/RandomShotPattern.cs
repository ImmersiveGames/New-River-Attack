using System;
using UnityEngine;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.BossSystems.Helpers;
using NewRiverAttack.ObstaclesSystems.BossSystems.Helpers.Interfaces;
using NewRiverAttack.ObstaclesSystems.EnemiesSystems;

namespace NewRiverAttack.ShootSystems
{
    [CreateAssetMenu(fileName = "RandomSpawnPattern", menuName = "ImmersiveGames/RiverAttack/ShootPatterns/RandomSpawn", order = 405)]
    public class RandomSpawnPattern : ShootPatternBase, IResettablePattern
    {
        [SerializeField] private float safeDistance = 2f;
        [SerializeField] private float safeEnemyDistance = 3f;
        [SerializeField] private LayerMask myLayerMask;

        private RandomSpawnHelper _spawnHelper;
        private GameObject _randomPoint;

        public override void Execute(Transform spawnPoint, ObjectShoot shooter)
        {
            TryShoot(() =>
            {
                // Inicializa o RandomSpawnHelper se necessário
                _spawnHelper ??= new RandomSpawnHelper(safeDistance, shooter.transform, Camera.main, safeEnemyDistance, myLayerMask);

                // Gera uma posição aleatória para o disparo
                var randomPosition = _spawnHelper.GetValidSpawnPosition<EnemiesMaster>();
                if (!randomPosition.HasValue) return;
                _randomPoint.transform.position = (Vector3)randomPosition;
                var bulletData = shooter.CreateBulletData(Vector3.zero, randomPosition.Value);
                shooter.PoolingOut(_randomPoint.transform, bulletData); // Usa o spawnPoint fornecido para o disparo
                shooter.ShootSound();
            });
        }

        public void ResetPattern(ObjectShoot shooter)
        {
            //LastShootTime = -cooldown;  // Reseta o tempo do último disparo
            //shooter.PoolHelper.ReturnMarkedObjects();
            if(_randomPoint == null)
                _randomPoint = new GameObject("randomPoint");
        }

        public void ExitPattern(ObjectShoot shooter)
        {
            if(_randomPoint != null)
                Destroy(_randomPoint);
        }
    }
}