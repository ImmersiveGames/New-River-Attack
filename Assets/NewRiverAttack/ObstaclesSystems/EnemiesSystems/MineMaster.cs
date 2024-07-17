using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.EnemiesSystems
{
    public class MineMaster : EnemiesMaster
    {
        #region Delagates & Events
        public event ObstacleGenericHandler EventAlertApproach;
        public event ObstacleGenericHandler EventAlertStop;
        public event ObstacleGenericHandler EventDetonate;
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
    }
}