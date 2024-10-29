
using System;

namespace NewRiverAttack.ObstaclesSystems.EnemiesSystems.Mines
{
    public class MineMaster : EnemiesMaster
    {
        #region Delagates & Events
        public event Action EventAlertApproach;
        public event Action EventAlertStop;
        public event Action EventDetonate;
        public event Action EventShoot;
        
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