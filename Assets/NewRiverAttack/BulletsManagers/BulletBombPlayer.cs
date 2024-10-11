using System.Collections.Generic;
using ImmersiveGames;
using ImmersiveGames.AudioEvents;
using ImmersiveGames.BulletsManagers;
using ImmersiveGames.CameraManagers;
using ImmersiveGames.DebugManagers;
using NewRiverAttack.AudioManagers;
using NewRiverAttack.GameStatisticsSystem;
using NewRiverAttack.ObstaclesSystems.EnemiesSystems;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.BulletsManagers
{
    public class BulletBombPlayer : Bullets
    {
        private float _timeLife;
        private float _endLife;
        private double _timerParam;
        private float _startRadius;

        private AudioEvent _audioEvent;
        private SphereCollider _collider;
        private readonly List<EnemiesMaster> _enemiesMasters = new List<EnemiesMaster>();

        private AudioSource _audioSource;
        private BombData _bombData;

        #region Unity Methods

        private void OnEnable()
        {
            var particleSystems = GetComponentsInChildren<ParticleSystem>();
            _collider = GetComponent<SphereCollider>();
            _startRadius = _collider.radius;
            _timeLife = MaxTimeSystemParticle(particleSystems);
            DebugManager.Log<BulletBombPlayer>($"{_timeLife}");
        }
        
        private void OnTriggerEnter(Collider other)
        {
            var enemy = other.GetComponentInParent<EnemiesMaster>();
            if (enemy && !_enemiesMasters.Contains(enemy))
            {
                _enemiesMasters.Add(enemy);
            }
        }

        private void FixedUpdate()
        {
            ExpandCollider();
            AutoDestroy(_endLife);
        }
        private void OnBecameInvisible()
        {
            Invoke(nameof(DestroyMe), _bombData.BulletTimer);
        }

        private void OnDisable()
        {
            Invoke(nameof(DestroyMe), _bombData.BulletTimer);
        }

        #endregion

        public override void OnSpawned(Transform spawnPosition, IBulletsData bombData)
        {
            if (_audioSource == null)
                _audioSource = GetComponent<AudioSource>();
            if (_audioEvent == null)
                _audioEvent = AudioManager.instance.GetAudioSfxEvent(EnumSfxSound.SfxPlayerBomb);
            BulletData = _bombData = bombData is BombData bulletsData ? bulletsData : default;
            
            var transform1 = transform;
            var position = spawnPosition.position;
            transform1.position = new Vector3(position.x, position.y, position.z + _bombData.BulletOffSet);
            gameObject.SetActive(true);
            _endLife = Time.time + _timeLife;
            _audioEvent.PlayOnShot(_audioSource);
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
        }

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

        protected override void DestroyMe()
        {
            GameStatisticManager.instance.LogBombsHit(_enemiesMasters.Count);
            GameObject o;
            (o = gameObject).SetActive(false);
            Destroy(o);
        }

        public PlayerMaster GetBombOwner => _bombData.BulletOwner as PlayerMaster;

        /*private void HardWereVibration(long timeVibration)
        {
            if (Application.platform == RuntimePlatform.Android && SystemInfo.supportsVibration)
            {
                #if UNITY_ANDROID && !UNITY_EDITOR
                ToolsAndroid.Vibrate(timeVibration);
                Handheld.Vibrate();
                #endif
            }
        }*/
    }
}
