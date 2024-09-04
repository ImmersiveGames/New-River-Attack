using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.PlayerManagers.Tags;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Abstracts
{
    public abstract class BossShoot : ObjectShoot
    {
        [Header("Shoot Settings")] 
        [Range(5f, 40f)] public float speedShoot;
        [Range(0, 10)] public int damageShoot;
        [Range(0.1f, 5f)] public float cadenceShoot;
        
        private BossMaster _bossMaster;
        private bool _isShoot;
        private void OnEnable()
        {
            SetInitialReferences();
            _bossMaster.EventObstacleChangeSkin += UpdateCadenceShoot;
        }

        private void OnDisable()
        {
            _bossMaster.EventObstacleChangeSkin -= UpdateCadenceShoot;
        }

        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            _bossMaster = GetComponent<BossMaster>();
            ShootSpawnPoint = GetComponentInChildren<ShootSpawnPoint>();
            SpawnPoint = ShootSpawnPoint.transform;
        }

        public override void SetDataBullet(ObjectMaster objectMaster)
        {
            var enemiesMaster = objectMaster as BossMaster;
            MakeBullet(enemiesMaster, speedShoot, damageShoot, bulletLifeTime, Vector3.forward);
        }
        
        internal virtual void UpdateCadenceShoot()
        {
            CadenceShoot = cadenceShoot;
            ShootSpawnPoint = GetComponentInChildren<ShootSpawnPoint>();
            SpawnPoint = ShootSpawnPoint.transform;
        }
        internal override void AttemptShoot(ObjectMaster objectMaster, Transform target = null)
        {
            if(timesRepeat > 0)
                base.AttemptShoot(objectMaster, target);
        }

        public bool ShouldBeShoot => _isShoot && !_bossMaster.IsDead && !_bossMaster.IsDisable && !GamePlayManager.Instance.IsPause;
        public void StartShoot()
        {
            _isShoot = true;
        }

        public void StopShoot()
        {
            _isShoot = false;
        }
    }
}