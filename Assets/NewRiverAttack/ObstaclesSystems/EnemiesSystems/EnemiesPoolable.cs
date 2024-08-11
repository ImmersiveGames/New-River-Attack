using ImmersiveGames;
using ImmersiveGames.AudioEvents;
using ImmersiveGames.BulletsManagers;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.PoolManagers;
using NewRiverAttack.AudioManagers;
using UnityEngine;
using IPoolable = ImmersiveGames.PoolManagers.Interface.IPoolable;

namespace NewRiverAttack.ObstaclesSystems.EnemiesSystems
{
    public sealed class EnemiesPoolable : MonoBehaviour, IPoolable
    {
        private float _startTime;
        private const float LimitTime = 0.1f;
        private AudioEvent _audioEvent;
        private EnemiesMaster _enemiesMaster;
        private AudioSource _audioSource;

        #region Unity Methods

        private void OnEnable()
        {
            SetInitialReferences();
        }
        
        private void OnDisable()
        {
            Invoke(nameof(DestroyMe), LimitTime);
        }

        #endregion
        

        #region Bullets Herance

        private IBulletsData _bulletData;
        public bool IsPooled { get; private set; }
        public PoolObject Pool { get; set; }
        public IBulletsData GetBulletData => _bulletData;
        public void OnSpawned(Transform spawnPosition, IBulletsData bulletData)
        {
            IsPooled = false;
            _audioEvent.PlayOnShot(_audioSource);
            var transform1 = transform;
            transform1.position = spawnPosition.position;
            transform1.rotation = spawnPosition.rotation;
            _bulletData = bulletData;
            _startTime = Time.time + bulletData.BulletTimer;
            _enemiesMaster = GetComponent<EnemiesMaster>();
            if (_enemiesMaster == null) return;
            _enemiesMaster.IsDisable = false;
            _enemiesMaster.OnEventSpawnObject(transform1.position);
        }
        private void AutoDestroy(float timer)
        {
            if (_bulletData.BulletTimer > 0 && Time.time >= timer)
            {
                DestroyMe();
            }
        }

        public void OnDespaired()
        {
            DebugManager.Log<EnemiesPoolable>("Enemy object onDespaired.");
            var myRoot = Pool.GetRoot();
            if (!myRoot)
                return;
            if (_enemiesMaster != null) _enemiesMaster.IsDisable = true;
            gameObject.transform.SetParent(myRoot);
            gameObject.transform.SetAsLastSibling();
            IsPooled = true;
        }

        private void DestroyMe()
        {
            gameObject.SetActive(false);
            OnDespaired();
        }
        #endregion
        
        private void SetInitialReferences()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioEvent = AudioManager.instance.GetAudioSfxEvent(EnumSfxSound.SfxMineShoot);
        }
        
    }
}