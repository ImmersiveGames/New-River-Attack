
namespace NewRiverAttack.ObstaclesSystems.EnemiesSystems.Mines
{
    public class MineMaster : EnemiesMaster
    {
        #region Delagates & Events
        public event ObstacleGenericHandler EventAlertApproach;
        public event ObstacleGenericHandler EventAlertStop;
        public event ObstacleGenericHandler EventDetonate;
        public event ObstacleGenericHandler EventShoot;
        
        #endregion
        
        #region Unity Methods

        protected override void OnEnable()
        {
            base.OnEnable();
            IsDisable = false;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            IsDisable = true;
        }

        #endregion
        

        protected internal void OnEventAlertApproach()
        {
            EventAlertApproach?.Invoke();
        }
        
        protected internal void OnEventDetonate()
        {
            EventDetonate?.Invoke();
        }

        protected internal void OnEventAlertStop()
        {
            EventAlertStop?.Invoke();
        }

        protected internal void OnEventShoot()
        {
            EventShoot?.Invoke();
        }
    }
}