using NewRiverAttack.ObstaclesSystems.ObjectsScriptables;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.Abstracts
{
    public abstract class ObstacleMaster: ObjectMaster
    {
        [SerializeField]
        protected internal ObjectsScriptable objectDefault;
        public LayerMask layerPlayer;
        
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
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            GamePlayManagerRef.EventGameRestart -= TryReSpawn;
            GamePlayManagerRef.EventGameReady -= ReadyObject;
        }

        #endregion
        

        #region Object Methods
        protected abstract void ReadyObject();
        protected abstract void AttemptKillObstacle(PlayerMaster playerMaster);
        protected abstract void TryReSpawn();

        #endregion

        internal virtual void OnEventObstacleHit(PlayerMaster playerMaster)
        {
            Debug.Log("Triggou no evento de Acerto");
            EventObstacleHit?.Invoke(playerMaster);
        }
        internal virtual void OnEventObstacleDeath(PlayerMaster playerMaster)
        {
            Debug.Log("Triggou no evento de Morte");
            AttemptKillObstacle(playerMaster);
            EventObstacleDeath?.Invoke(playerMaster);
        }

        internal void OnObstacleChangeSkin()
        {
            EventObstacleChangeSkin?.Invoke();
        }
    }
}