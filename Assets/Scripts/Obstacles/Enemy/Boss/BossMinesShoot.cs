using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class BossMinesShoot: MonoBehaviour, IHasPool
    {
        public bool debugGrade = false;
        
        public const float OffsetZ = 25.0f;
        public const float OffsetX = 0.0f;
        public const float OffsetY = 1.0f;
        [Header("Mines Settings")]
        [SerializeField] GameObject bulletMines;
        [SerializeField] int minesStartPool;
        [SerializeField] AudioEventSample minesShoots;
        public int minesQuantity = 10;
        public int numColumns = 15; 
        public int numLines = 10;
        public List<Vector2Int> quadrantsBlocked;

        internal Transform spawnPoint;
        AudioSource m_AudioSource;
        void Start()
        {
            m_AudioSource = GetComponent<AudioSource>();
            // setup inicial do status
            StartMyPool(bulletMines,minesStartPool);
            spawnPoint = GetComponentInChildren<EnemiesShootSpawn>().transform ? GetComponentInChildren<EnemiesShootSpawn>().transform : transform;
        }
        internal GameObject getBulletsMines
        {
            get { return bulletMines; }
        }

        public void StartMyPool(GameObject bullets, int quantity, bool isPersistent = false)
        {
            PoolObjectManager.CreatePool(this, bullets, quantity, transform, isPersistent);
        }

        public void PlayMineShoot()
        {
            minesShoots.Play(m_AudioSource);
        }
        internal static bool QuadrantAlreadySort(IEnumerable<Vector2Int> quadrantsFull, int x, int z)
        {
            return quadrantsFull.Any(quadrants => quadrants.x == x && quadrants.y == z);
        }

        static bool QuadrantsBlocked(int x, int z, IEnumerable<Vector2Int> quadrantBlocked)
        {
            return quadrantBlocked.Any(quadrants => quadrants.x == x && quadrants.y == z);
        }
        internal static bool QuadrantAlreadyOccupied(Vector2Int quadrants, Vector2 quadrantSize, Vector2 screenSize, ref List<Vector2Int> usedQuadrant)
        {
            float posX = ((quadrants.x + 0.5f) * quadrantSize.x) - (screenSize.x / 2.0f) + OffsetX;
            float posZ = ((quadrants.y + 0.5f) * quadrantSize.y) - (screenSize.y / 2.0f) + OffsetZ;
            var colliders = new Collider[1];
            var newPosition = new Vector3(posX, 1, posZ);
            int count = Physics.OverlapBoxNonAlloc(newPosition, quadrantSize / 2, colliders, Quaternion.identity, GameManager.instance.layerEnemies);
            if (count <= 0)
                return false;
            var q = new Vector2Int(quadrants.x, quadrants.y);
            if(!usedQuadrant.Contains(q))
                usedQuadrant.Add(new Vector2Int(quadrants.x, quadrants.y));
            return true;
        }
        void OnDrawGizmos()
        {
            if(!debugGrade) return;
            var camera = Camera.main;
            float height = camera!.orthographicSize * 2.0f;
            float weight = height * camera.aspect;

            float sizeQuadrantX = weight / numColumns;
            float sizeQuadrantZ = height / numLines;

            var position = transform.position; //TODO: precisa pegar a referencia do player;
            float startX = position.x - (weight / 2.0f) + OffsetX;
            float startZ = position.z - (height / 2.0f) + OffsetZ;

            for (int x = 0; x < numColumns; x++)
            {
                for (int z = 0; z < numLines; z++)
                {
                    var quadrant = new Vector2Int(x, z);

                    float posX = startX + (x * sizeQuadrantX) + (sizeQuadrantX / 2.0f);
                    float posZ = startZ + (z * sizeQuadrantZ) + (sizeQuadrantZ / 2.0f);

                    var center = new Vector3(posX, 0.0f, posZ);
                    var size = new Vector3(sizeQuadrantX, 0.0f, sizeQuadrantZ);

                    Gizmos.color = QuadrantsBlocked(quadrant.x, quadrant.y,quadrantsBlocked) ? Color.red : Color.green;
                    Gizmos.DrawWireCube(center, size);
                }
            }
        }
    }
}
