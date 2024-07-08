using ImmersiveGames;
using ImmersiveGames.AudioEvents;
using ImmersiveGames.BulletsManagers;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.PoolManagers;
using ImmersiveGames.PoolManagers.Interface;
using NewRiverAttack.AudioManagers;
using NewRiverAttack.ObstaclesSystems.EnemiesSystems;
using UnityEngine;

namespace NewRiverAttack.BulletsManagers
{
    public class BulletMine : Bullets, IPoolable
    {
        private float _startTime;
        private const float LimitTime = 0.1f;

        private AudioEvent _audioEvent;
        private EnemiesMaster _enemiesMaster;
        private AudioSource _audioSource;
        
        public bool IsPooled { get; private set; } = true;
        
        #region Unity Methods
        private void OnEnable()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioEvent = AudioManager.GetAudioSfxEvent(EnumSfxSound.SfxEnemyShoot);
        }
        private void OnDisable()
        {
            Invoke(nameof(DestroyMe), LimitTime);
        }
        #endregion
        
        public override void OnSpawned(Transform spawnPosition, IBulletsData bulletData)
        {
            IsPooled = false;
            _audioEvent.PlayOnShot(_audioSource);
            base.OnSpawned(spawnPosition, bulletData);
            _startTime = Time.time + bulletData.BulletTimer;
            _enemiesMaster = bulletData.BulletOwner as EnemiesMaster;
            
        }
        public PoolObject Pool { get; set; }
        
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
        protected override void DestroyMe()
        {
            base.DestroyMe();
            OnDespaired();
        }
    }
}
