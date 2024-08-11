using System;
using ImmersiveGames;
using ImmersiveGames.AudioEvents;
using ImmersiveGames.BulletsManagers;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.PoolManagers;
using ImmersiveGames.PoolManagers.Interface;
using NewRiverAttack.AudioManagers;
using NewRiverAttack.LevelBuilder.Abstracts;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.BulletsManagers
{
    public sealed class BulletPlayer : Bullets, IPoolable
    {
        public Color powerUpColor;
        public Color originalColor;
        private Material _originalMaterial;
        private float _startTime;
        private const float LimitTime = 0.01f;

        private AudioEvent _audioEvent;
        private PlayerMaster _playerMaster;
        private AudioSource _audioSource;
        

        #region Unity Methods

        private void OnEnable()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioEvent = AudioManager.instance.GetAudioSfxEvent(EnumSfxSound.SfxPlayerShoot);
        }

        private void Awake()
        {
            _originalMaterial = GetComponent<Renderer>().material;
            originalColor = _originalMaterial.color;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision == null) return;
            if (collision.GetComponentInParent<PlayerMaster>() || collision.GetComponent<BulletPlayer>() || collision.GetComponent<LevelFinishers>()) return;
            var obstacleMaster = collision.GetComponentInParent<ObstacleMaster>();
            if (obstacleMaster != null)
            {
                if (obstacleMaster.objectDefault.ignoreBullets ) return;
            }
            Invoke(nameof(DestroyMe), 0.1f);
            //DestroyMe();
        }
        private void FixedUpdate()
        {
            MoveShoot();
            AutoDestroy(_startTime);
        }
        private void OnBecameInvisible()
        {
            Invoke(nameof(DestroyMe), LimitTime);
        }

        private void OnDisable()
        {
            Invoke(nameof(DestroyMe), LimitTime);
        }

        #endregion

        #region Bullets Herance
        public bool IsPooled { get; private set; } = true;
        public PoolObject Pool { get; set; }

        public override void OnSpawned(Transform spawnPosition, IBulletsData bulletData)
        {
            IsPooled = false;
            _audioEvent.PlayOnShot(_audioSource);
            base.OnSpawned(spawnPosition, bulletData);
            _originalMaterial.color = bulletData.BulletPowerUp ? powerUpColor : originalColor;
            _startTime = Time.time + bulletData.BulletTimer;
            _playerMaster = bulletData.BulletOwner as PlayerMaster;
            
        }
        public void OnDespaired()
        {
            DebugManager.Log<Bullets>("Bullet object onDespaired.");
            var myRoot = Pool.GetRoot();
            if (!myRoot)
                return;
            gameObject.transform.SetParent(myRoot);
            gameObject.transform.SetAsLastSibling();
            IsPooled = true;
        }
        #endregion

        private void MoveShoot()
        {
            if (_playerMaster != null && _playerMaster.ObjectIsReady)
            {
                var speedy = BulletData.BulletSpeed * Time.deltaTime;
                transform.Translate(Vector3.forward * speedy);
            }
            else
            {
                DestroyMe();
            }
        }

        protected override void DestroyMe()
        {
            base.DestroyMe();
            OnDespaired();
        }
    }
}