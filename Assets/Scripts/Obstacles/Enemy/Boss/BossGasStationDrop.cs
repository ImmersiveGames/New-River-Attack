﻿using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class BossGasStationDrop : MonoBehaviour, IHasPool
    {
        [Header("Gas Station Settings")]
        [SerializeField]
        private GameObject gasStation;
        [SerializeField] private int startPool;
        [SerializeField] internal int[] dropGasStation;
        [SerializeField] private AudioEventSample dropGasStationSound;
        
        internal Transform spawnPoint;
        private AudioSource m_AudioSource;

        private void Start()
        {
            m_AudioSource = GetComponent<AudioSource>();
            dropGasStation ??= new[] { 2 };
            // setup inicial do status
            StartMyPool(gasStation,startPool);
            spawnPoint = GetComponentInChildren<EnemiesShootSpawn>().transform ? GetComponentInChildren<EnemiesShootSpawn>().transform : transform;
        }
        public void StartMyPool(GameObject gameObjectPool, int quantity, bool isPersistent = false)
        {
            PoolObjectManager.CreatePool(this, gameObjectPool, quantity, transform, isPersistent);
        }

        public Transform GetMyPool()
        {
            return transform;
        }

        internal void PlayDropGas()
        {
            dropGasStationSound.Play(m_AudioSource);
        }
    }
}
