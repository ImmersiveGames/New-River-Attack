using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class BossGasStationDrop : MonoBehaviour, IHasPool
    {
        [Header("Gas Station Settings")]
        [SerializeField] GameObject gasStation;
        [SerializeField] int startPool;
        [SerializeField] AudioEventSample dropGasStationSound;
        
        internal Transform spawnPoint;
        AudioSource m_AudioSource;
        void Start()
        {
            m_AudioSource = GetComponent<AudioSource>();
            // setup inicial do status
            StartMyPool(gasStation,startPool);
            spawnPoint = GetComponentInChildren<EnemiesShootSpawn>().transform ? GetComponentInChildren<EnemiesShootSpawn>().transform : transform;
        }
        internal void DropGasStation()
        {
            
        }

        public void StartMyPool(GameObject gameObjectPool, int quantity, bool isPersistent = false)
        {
            PoolObjectManager.CreatePool(this, gameObjectPool, quantity, transform, isPersistent);
        }
    }
}
