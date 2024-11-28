using System.Collections.Generic;
using ImmersiveGames;
using ImmersiveGames.AudioEvents;
using ImmersiveGames.CameraManagers;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.PoolSystems.Interfaces;
using NewRiverAttack.AudioManagers;
using NewRiverAttack.BulletsManagers.Interface;
using NewRiverAttack.GameStatisticsSystem;
using NewRiverAttack.ObstaclesSystems.EnemiesSystems;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.BulletsManagers
{
    public class BulletBombPlayer : Bullet
    {
        private double _timerParam;
        private float _startRadius;

        private AudioEvent _audioEvent;
        private SphereCollider _collider;
        private List<EnemiesMaster> _enemiesMasters = new List<EnemiesMaster>();

        private AudioSource _audioSource;
        private BombSpawnData _bombData;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            var particleSystems = GetComponentsInChildren<ParticleSystem>();
            _collider = GetComponent<SphereCollider>();
            _startRadius = _collider.radius;
            Lifetime = MaxTimeSystemParticle(particleSystems);
            _bombData = SpawnData as BombSpawnData;
        }
        private void FixedUpdate()
        {
            ExpandCollider();
        }

        private void AutoDestroy()
        {
            GameStatisticManager.instance.LogBombsHit(_enemiesMasters.Count);
            _enemiesMasters = new List<EnemiesMaster>();
            ResetBomb();
            ReturnToPool();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<PlayerMaster>()) return;
            var enemy = other.GetComponentInParent<EnemiesMaster>();
            if (enemy && !_enemiesMasters.Contains(enemy))
            {
                _enemiesMasters.Add(enemy);
            }
            
        }
        private void OnBecameInvisible()
        {
            Invoke(nameof(AutoDestroy), _bombData.Timer);
        }

        private void ResetBomb()
        {
            _collider.radius = _startRadius;
            _timerParam = 0; 
        }

        public BombSpawnData GetData => _bombData;
        public override void OnSpawned(Transform spawnPosition, ISpawnData data)
        {
            base.OnSpawned(spawnPosition, data);
            _bombData = SpawnData as BombSpawnData;
            if (_audioSource == null)
                _audioSource = GetComponent<AudioSource>();
            if (_audioEvent == null)
                _audioEvent = AudioManager.instance.GetAudioSfxEvent(EnumSfxSound.SfxPlayerBomb);
            
            gameObject.SetActive(true);
            _audioEvent.PlayOnShot(_audioSource);
            
            var transform1 = transform;
            var position = spawnPosition.position;
            if (_bombData != null)
                transform1.position = new Vector3(position.x, position.y, position.z + BombSpawnData.BulletOffSet);
            
        }

        #region Particles System

        private float MaxTimeSystemParticle(IReadOnlyCollection<ParticleSystem> particleSystems)
        {
            var maxTime = 0f;
            // Verifica se há sistemas de partículas
            if (particleSystems == null || particleSystems.Count == 0)
            {
                DebugManager.LogError<BulletBombPlayer>("Nenhum sistema de partículas encontrado!");
                return 0f;
            }

            // Percorre todos os sistemas de partículas para encontrar o que tem a maior duração
            foreach (var sistema in particleSystems)
            {
                var mainModule = sistema.main;
                var actualDuration = mainModule.duration;

                if (!(actualDuration > maxTime)) continue;
                maxTime = actualDuration;
            }

            return maxTime;
        }
        private void ExpandCollider()
        {
            if (_bombData.BombRadius == 0) return;
            _timerParam += Time.deltaTime * _bombData.BombRadiusSpeed;
            if (CameraShake.Instance != null)
            {
                CameraShake.Instance.ShakeCamera(_bombData.BombShakeForce, _bombData.BombShakeTime);
            }
            //HardWereVibration(_bombData.BombMillisecondsVibrate);
            if (!_collider && _collider.GetType() != typeof(SphereCollider))
                return;
            _collider.radius = Mathf.Lerp(_startRadius, _bombData.BombRadius, (float)_timerParam);

            if (_timerParam >= _bombData.Timer)
            {
                AutoDestroy();
            }
            
        }
        #endregion
    }
}
