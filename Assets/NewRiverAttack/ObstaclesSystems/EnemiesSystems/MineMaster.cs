﻿
using Unity.VisualScripting;

namespace NewRiverAttack.ObstaclesSystems.EnemiesSystems
{
    public class MineMaster : EnemiesMaster
    {
        #region Delagates & Events
        public event ObstacleGenericHandler EventAlertApproach;
        public event ObstacleGenericHandler EventAlertStop;
        public event ObstacleGenericHandler EventDetonate;
        #endregion
        
        #region Unity Methods

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        private void Start()
        {
            IsDisable = false;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        #endregion
        
        /*

        protected override void OnEnable()
        {
            base.OnEnable();
            GamePlayManagerRef.EventGameReload += DestroyMine;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            GamePlayManagerRef.EventGameReload -= DestroyMine;
        }
        
        private void DestroyMine()
        {
            DestroyImmediate(gameObject);
        }

        */
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