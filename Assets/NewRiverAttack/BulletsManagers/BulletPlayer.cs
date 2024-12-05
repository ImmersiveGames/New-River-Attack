using System;
using ImmersiveGames.PoolSystems.Interfaces;
using NewRiverAttack.BulletsManagers.Interface;
using NewRiverAttack.ObstaclesSystems.CollectibleSystems.PowerUpSystems;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using NewRiverAttack.WallsManagers;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace NewRiverAttack.BulletsManagers
{
    public sealed class BulletPlayer : Bullet
    {
        [SerializeField] private GameObject vfxExplode;
        [SerializeField] private Color powerUpColor = Color.red;
        private Color _originalColor;
        private Material _originalMaterial;
        private void Awake()
        {
            _originalMaterial = GetComponent<Renderer>().material;
            _originalColor = _originalMaterial.color;
        }

        private void Update()
        {
            if (!IsInitialize) return;

            // Movimenta o projétil na direção e velocidade fornecidas
            transform.position += GetData.Direction * (GetData.Speed * Time.deltaTime);

            // Reduz o tempo de vida do projétil
            Lifetime -= Time.deltaTime;

            // Se o tempo de vida acabar, retorna ao pool
            if (Lifetime <= 0)
            {
                ReturnToPool();
            }
        }

        public BulletSpawnData GetData { get; private set; }

        public override void OnSpawned(Transform spawnPosition, ISpawnData data)
        {
            GetData = data as BulletSpawnData;
            _originalMaterial.color = GetData is { PowerUp: true } ? powerUpColor : _originalColor;
            base.OnSpawned(spawnPosition, GetData);
        }

        protected override void ReturnToPool()
        {
            var vfx =Instantiate(vfxExplode, transform.position, quaternion.identity);
            base.ReturnToPool();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<BulletPlayer>()) return;
            if (other.GetComponentInParent<PlayerMaster>()) return;
            if (other.GetComponentInParent<PowerUpMaster>()) return;
            Instantiate(vfxExplode, transform.position, quaternion.identity);
            Invoke(nameof(ReturnToPool), 0.02f);
        }
    }
}