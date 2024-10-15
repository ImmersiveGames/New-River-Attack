using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;
using UnityEngine.Serialization;

namespace NewRiverAttack.ObstaclesSystems.Abstracts
{
    public abstract class ObstacleMaster: ObjectMaster
    {
        [SerializeField]
        protected internal ObjectsScriptable.ObjectsScriptable objectDefault;
        public LayerMask layerPlayer;
        public LayerMask myLayerMask;
        public T GetObjectScriptable<T>() where T : class
        {
            return objectDefault as T;
        }
        
        #region Delagates & Events
        public delegate void ObstacleGenericHandler();
        public event ObstacleGenericHandler EventObstacleChangeSkin;
        public delegate void ObstacleMasterHandler(PlayerMaster playerMaster);
        public event ObstacleMasterHandler EventObstacleDeath;
        public event ObstacleMasterHandler EventObstacleHit;

        #endregion

        #region Unity Methods

        protected override void OnEnable()
        {
            base.OnEnable();
            GamePlayManagerRef.EventGameRestart += TryReSpawn;
            GamePlayManagerRef.EventGameReady += ReadyObject;
            GamePlayManagerRef.EventGameReload += ReloadObject;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            GamePlayManagerRef.EventGameRestart -= TryReSpawn;
            GamePlayManagerRef.EventGameReady -= ReadyObject;
            GamePlayManagerRef.EventGameReload -= ReloadObject;
        }

        public bool ShouldBeReady => GamePlayManagerRef.ShouldBePlayingGame && !IsDead && !IsDisable;

        #endregion
        
        #region Object Methods
        protected abstract void ReadyObject();
        protected abstract void ReloadObject();
        protected abstract void AttemptKillObstacle(PlayerMaster playerMaster);
        protected abstract void TryReSpawn();

        #endregion

        internal void OnEventObstacleHit(PlayerMaster playerMaster)
        {
            EventObstacleHit?.Invoke(playerMaster);
        }
        internal void OnEventObstacleDeath(PlayerMaster playerMaster)
        {
            AttemptKillObstacle(playerMaster);
            EventObstacleDeath?.Invoke(playerMaster);
        }

        internal void OnObstacleChangeSkin()
        {
            EventObstacleChangeSkin?.Invoke();
        }
    }
}