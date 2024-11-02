using System.Collections;
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
        [SerializeField] private int itemCount = 5;
        [SerializeField] private float safeDistance = 2f;
        [SerializeField] private float safeEnemyDistance = 3f;
        [SerializeField] private float cadenceVariance = 0.5f;
        [SerializeField] private LayerMask myLayerMask;

        private RandomSpawnHelper _spawnHelper;
        private Coroutine _spawnRoutine;

        public override void Execute(Transform spawnPoint, ObjectShoot shooter)
        {
            // Inicializa o SpawnHelper com o transform do próprio shooter
            _spawnHelper ??= new RandomSpawnHelper(safeDistance, shooter.transform, Camera.main, safeEnemyDistance, myLayerMask);

            // Inicia o processo de spawn
            _spawnRoutine = shooter.StartCoroutine(SpawnItemsCoroutine(spawnPoint, shooter));
        }

        private IEnumerator SpawnItemsCoroutine(Transform spawnPoint, ObjectShoot shooter)
        {
            var tempSpawnPoint = new GameObject("TempSpawnPoint").transform;
            for (var i = 0; i < itemCount; i++)
            {
                var randomPosition = _spawnHelper?.GetValidSpawnPosition<EnemiesMaster>();
                if (randomPosition.HasValue)
                {
                    tempSpawnPoint.position = randomPosition.Value;
                    var itemData = shooter.CreateBulletData(Vector3.zero, randomPosition.Value);
                    shooter.PoolingOut(tempSpawnPoint, itemData);
                }

                yield return new WaitForSeconds(cooldown + Random.Range(-cadenceVariance, cadenceVariance));
            }

            Destroy(tempSpawnPoint.gameObject);
            shooter.PoolHelper.ReturnMarkedObjects();
            _spawnRoutine = null;
        }

        public void ResetPattern(ObjectShoot shooter)
        {
            // Reinicia o comportamento, interrompendo a coroutine atual, se existir
            if (_spawnRoutine != null)
            {
                shooter.StopCoroutine(_spawnRoutine);
                _spawnRoutine = null;
            }
            
            shooter.PoolHelper.ReturnMarkedObjects();
        }

        public override void SetParameter(EnumShootParameter parameter, object value)
        {
            base.SetParameter(parameter, value);

            switch (parameter)
            {
                case EnumShootParameter.ItemCount:
                    if (value is int count)
                        itemCount = count;
                    else
                        Debug.LogError("O valor para ItemCount precisa ser do tipo int.");
                    break;
                case EnumShootParameter.SafeDistance:
                    if (value is float distance)
                        safeDistance = distance;
                    else
                        Debug.LogError("O valor para SafeDistance precisa ser do tipo float.");
                    break;
                case EnumShootParameter.SafeEnemyDistance:
                    if (value is float enemyDistance)
                        safeEnemyDistance = enemyDistance;
                    else
                        Debug.LogError("O valor para SafeEnemyDistance precisa ser do tipo float.");
                    break;
                case EnumShootParameter.CadenceVariance:
                    if (value is float variance)
                        cadenceVariance = variance;
                    else
                        Debug.LogError("O valor para CadenceVariance precisa ser do tipo float.");
                    break;
                default:
                    Debug.LogWarning($"Parâmetro '{parameter}' não suportado pelo RandomSpawnPattern.");
                    break;
            }
        }

        protected override object GetParameter(EnumShootParameter parameterName)
        {
            return parameterName switch
            {
                EnumShootParameter.ItemCount => itemCount,
                EnumShootParameter.SafeDistance => safeDistance,
                EnumShootParameter.SafeEnemyDistance => safeEnemyDistance,
                EnumShootParameter.CadenceVariance => cadenceVariance,
                _ => base.GetParameter(parameterName)
            };
        }
    }
}
