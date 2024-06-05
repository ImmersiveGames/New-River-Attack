using ImmersiveGames.BulletsManagers;
using ImmersiveGames.DebugManagers;
using UnityEngine;
using ImmersiveGames.PoolManagers.Interface;

namespace ImmersiveGames.PoolManagers.Test
{
    public class PoolTest : MonoBehaviour
    {
        public GameObject prefab;
        public int initialPoolSize = 10;
        public Transform poolRoot;
        public bool persistent;

        public string poolName = "MyPool";

        private IPoolManager _poolManager;
        private bool _testingInactiveObjects;

        private void Start()
        {
            _poolManager = GetComponent<IPoolManager>();
            _poolManager.CreatePool("Teste Pool",prefab, initialPoolSize, poolRoot, persistent);

            _testingInactiveObjects = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                var newObj = _poolManager.GetObjectFromPool<IPoolable>(poolName, poolRoot, new BulletData());
                if (newObj != null)
                {
                    var xPos = Random.Range(-5f, 5f);
                    var yPos = Random.Range(-5f, 5f);
                    newObj.transform.position = new Vector3(xPos, yPos, 0f);
                    newObj.SetActive(true);
                    DebugManager.Log<PoolTest>("New object retrieved from the pool and activated.");
                }
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                _testingInactiveObjects = true;
                DebugManager.Log<PoolTest>("Starting inactive objects test.");
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                _testingInactiveObjects = false;
                DebugManager.Log<PoolTest>("Ending inactive objects test.");
            }

            if (_testingInactiveObjects)
            {
                TestInactiveObjectsList();
            }
        }
        

        private void TestInactiveObjectsList()
        {
            _poolManager.ResizePool(poolName, initialPoolSize + 5); // Aumenta o tamanho do pool
            DebugManager.Log<PoolTest>("Increased pool size for testing inactive objects.");
            _poolManager.ResizePool(poolName, initialPoolSize); // Reduz o tamanho do pool de volta ao tamanho inicial
            DebugManager.Log<PoolTest>("Decreased pool size for testing inactive objects.");
        }
    }
}
