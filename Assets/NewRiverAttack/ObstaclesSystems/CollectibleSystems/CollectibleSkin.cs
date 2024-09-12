using NewRiverAttack.ObstaclesSystems.Abstracts;

namespace NewRiverAttack.ObstaclesSystems.CollectibleSystems
{
    public class CollectibleSkin : ObstacleSkin
    {
        private CollectibleMaster _collectibleMaster;
        private CollectibleAnimator _collectibleAnimator;

        private void Awake()
        {
            _collectibleAnimator = GetComponent<CollectibleAnimator>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            if(_collectibleAnimator != null) return;
            _collectibleMaster.EventMasterCollectCollect += DesativeSkin;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if(_collectibleAnimator != null) return;
            _collectibleMaster.EventMasterCollectCollect -= DesativeSkin;
        }
        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            _collectibleMaster = ObstacleMaster as CollectibleMaster;
        }
    }
}