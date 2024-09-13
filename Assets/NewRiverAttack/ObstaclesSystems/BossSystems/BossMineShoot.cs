using System.Collections.Generic;
using System.Linq;
using ImmersiveGames.PoolManagers.Interface;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.BossSystems.Abstracts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NewRiverAttack.ObstaclesSystems.BossSystems
{
    public class BossMineShoot : BossShoot
    {
        private const float OffsetZ = -18.0f;
        private const float OffsetX = 0.0f;
        private const float OffsetY = 1.0f;

        [SerializeField] private bool debugGrade;
        
        [SerializeField] private int numColumns;
        [SerializeField] private int numLines;
        [SerializeField] private int numMines;
        [SerializeField] private List<Vector2Int> quadrantsBlocked;
        [SerializeField] private List<Vector2Int> usedQuadrants;
        
        private Camera _camera;
        private Vector2 _viewSize;
        private Vector2 _sizeQuadrant;
        private Transform _spawnPoint;
        
        private int _actualMines;
        private void Awake()
        {
            poolName = $"Pool ({nameof(BossMineShoot)})";
            _camera = Camera.main;
            CreateSpawnPoint("MinesSpawnPoint");
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            GamePlayManager.Instance.EventGameReload += ReloadMine;
            GamePlayManager.Instance.EventGameRestart += ReloadMine;
        }

        private void ReloadMine()
        {
            DestroyImmediate(_spawnPoint);
            CreateSpawnPoint("MinesSpawnPoint");
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            GamePlayManager.Instance.EventGameReload -= ReloadMine;
            GamePlayManager.Instance.EventGameRestart -= ReloadMine;
        }

        private void CreateSpawnPoint(string spawnName)
        {
            _spawnPoint = new GameObject().transform;
            _spawnPoint.name = spawnName;
        }

        internal override void UpdateCadenceShoot()
        {
            base.UpdateCadenceShoot();
            _viewSize = new Vector2(_camera!.orthographicSize * 2.0f * _camera.aspect, _camera!.orthographicSize * 2.0f);
            _sizeQuadrant = new Vector2(_viewSize.x / numColumns, _viewSize.y / numLines);
            usedQuadrants = new List<Vector2Int>();
            usedQuadrants.AddRange(quadrantsBlocked);
        }
        public void SetShoots(int mines, float cadence)
        {
            //numMines = mines <= 0 ? numMines : mines;
            timesRepeat = mines <= 0 ? numMines : mines;
            CadenceShoot = cadence <= 0 ? CadenceShoot : cadence;
        }

        protected override void Fire()
        {
            //sorteia um espaço não ocupado
            var indexQuadrants = new Vector2Int(0,0);
            do
            {
                indexQuadrants.x = Random.Range(0, numColumns);
                indexQuadrants.y = Random.Range(0, numLines);
            }
            while (QuadrantAlreadyOccupied(indexQuadrants, _sizeQuadrant, _viewSize, ref usedQuadrants) ||
                   QuadrantAlreadySort(usedQuadrants, indexQuadrants.x, indexQuadrants.y)
                  );
            usedQuadrants.Add(new Vector2Int(indexQuadrants.x, indexQuadrants.y));
            
            // define a posição do quadrante sorteado
            var posX = ((indexQuadrants.x + 0.5f) * _sizeQuadrant.x) - (_viewSize.x / 2.0f) + OffsetX;
            var posZ = ((indexQuadrants.y + 0.5f) * _sizeQuadrant.y) - (_viewSize.y / 2.0f) + OffsetZ;
            _spawnPoint.position = transform.position;
            var randomPosition = new Vector3(posX, OffsetY, posZ);
            
            _spawnPoint.position += randomPosition;
            
            Bullet = PoolManager.GetObjectFromPool<IPoolable>(poolName, _spawnPoint, BulletData);
        }

        private static bool QuadrantAlreadySort(IEnumerable<Vector2Int> quadrantsFull, int x, int z)
        {
            return quadrantsFull.Any(quadrants => quadrants.x == x && quadrants.y == z);
        }

        private static bool QuadrantsBlocked(IEnumerable<Vector2Int> quadrantBlocked, int x, int z)
        {
            return quadrantBlocked.Any(quadrants => quadrants.x == x && quadrants.y == z);
        }

        private static bool QuadrantAlreadyOccupied(Vector2Int quadrants, Vector2 quadrantSize, Vector2 screenSize, ref List<Vector2Int> usedQuadrant)
        {
            var posX = ((quadrants.x + 0.5f) * quadrantSize.x) - (screenSize.x / 2.0f) + OffsetX;
            var posZ = ((quadrants.y + 0.5f) * quadrantSize.y) - (screenSize.y / 2.0f) + OffsetZ;
            var colliders = new Collider[1];
            var newPosition = new Vector3(posX, 1, posZ);
            var count = Physics.OverlapBoxNonAlloc(newPosition, quadrantSize / 2, colliders, Quaternion.identity, GamePlayManager.Instance.layerEnemies);
            if (count <= 0)
                return false;
            var q = new Vector2Int(quadrants.x, quadrants.y);
            if(!usedQuadrant.Contains(q))
                usedQuadrant.Add(new Vector2Int(quadrants.x, quadrants.y));
            return true;
        }

        #region Gizmos
        private void OnDrawGizmos()
        {
            if(!debugGrade) return;

            var height = _camera!.orthographicSize * 2.0f;
            var weight = height * _camera.aspect;

            var sizeQuadrantX = weight / numColumns;
            var sizeQuadrantZ = height / numLines;

            var position = transform.position; //TODO: precisa pegar a referencia do player;
            var startX = position.x - (weight / 2.0f) + OffsetX;
            var startZ = position.z - (height / 2.0f) + OffsetZ;

            for (var x = 0; x < numColumns; x++)
            {
                for (var z = 0; z < numLines; z++)
                {
                    var quadrant = new Vector2Int(x, z);

                    var posX = startX + (x * sizeQuadrantX) + (sizeQuadrantX / 2.0f);
                    var posZ = startZ + (z * sizeQuadrantZ) + (sizeQuadrantZ / 2.0f);

                    var center = new Vector3(posX, 0.0f, posZ);
                    var size = new Vector3(sizeQuadrantX, 0.0f, sizeQuadrantZ);

                    Gizmos.color = QuadrantsBlocked(quadrantsBlocked, quadrant.x, quadrant.y) ? Color.red : Color.green;
                    Gizmos.DrawWireCube(center, size);
                }
            }
        }
        
        #endregion
    }
}