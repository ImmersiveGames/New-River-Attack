using DG.Tweening;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.EnemiesSystems;
using NewRiverAttack.PlayerManagers.PlayerSystems;

namespace NewRiverAttack.ObstaclesSystems.BossSystems
{
    public sealed class BossMaster : EnemiesMaster
    {
        public Ease enterAnimation;
        
        private PlayerMaster _playerMaster;
        
        public delegate void BossMasterGeneralHandler();

        public event BossMasterGeneralHandler EventBossShowUp;
        protected override void OnEnable()
        {
            base.OnEnable();
            GamePlayBossManager.instance.SetBoss(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        public void InitializeBoss(PlayerMaster getPlayerMaster)
        {
            _playerMaster = getPlayerMaster;
            var vector3 = transform.position;
            vector3.z = _playerMaster.transform.position.z - 5;
            vector3.x = _playerMaster.transform.position.x;
            transform.position = vector3;
            
            Invoke(nameof(OnEventBossShowUp), 3f);
        }

        private void ShowUp()
        {
            var distance = _playerMaster.transform.position.z + 40;
            OnEventBossShowUp();
            DOTween.Sequence()
                .Append(gameObject.transform.DOMoveZ(distance, 2f).SetEase(enterAnimation))
                .Play();
        }

        internal void OnEventBossShowUp()
        {
            EventBossShowUp?.Invoke();
        }
    }
}