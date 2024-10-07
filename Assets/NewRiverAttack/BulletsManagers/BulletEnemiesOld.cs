using ImmersiveGames;
using ImmersiveGames.AudioEvents;
using ImmersiveGames.BulletsManagers;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.PoolManagers;
using ImmersiveGames.PoolManagers.Interface;
using NewRiverAttack.AudioManagers;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.EnemiesSystems;
using NewRiverAttack.WallsManagers;
using UnityEngine;

namespace NewRiverAttack.BulletsManagers
{
    public class BulletEnemiesOld : Bullets, IPoolable
    {
        private float _startTime;
        private const float LimitTime = 0.1f;

        private AudioEvent _audioEvent;
        private EnemiesMaster _enemiesMaster;
        private AudioSource _audioSource;
        

        #region Unity Methods

        private void OnEnable()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioEvent = AudioManager.instance.GetAudioSfxEvent(EnumSfxSound.SfxEnemyShoot);
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision == null) return;
            if (collision.GetComponentInParent<WallMaster>() || collision.GetComponentInParent<ObstacleMaster>() || collision.GetComponent<BulletEnemiesOld>()) return;
            DestroyMe();
        }
        private void FixedUpdate()
        {
            MoveShoot();
            AutoDestroy(_startTime);
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
            _startTime = Time.time + bulletData.BulletTimer;
            _enemiesMaster = bulletData.BulletOwner as EnemiesMaster;
            
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
            if (_enemiesMaster != null && _enemiesMaster.ObjectIsReady)
            {
                var speedy = BulletData.BulletSpeed * Time.deltaTime;
                var direction = BulletData.BulletDirection;
                transform.Translate(direction * speedy);
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