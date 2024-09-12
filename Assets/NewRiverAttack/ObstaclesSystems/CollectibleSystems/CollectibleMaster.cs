using ImmersiveGames.ObjectManagers.Interfaces;
using NewRiverAttack.GameStatisticsSystem;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;
using NewRiverAttack.PlayerManagers.PlayerSystems;

namespace NewRiverAttack.ObstaclesSystems.CollectibleSystems
{
    public class CollectibleMaster : ObstacleMaster, ICollectable
    {
        internal event ObstacleGenericHandler EventMasterCollectCollect;

        public void Collect(PlayerMaster playerMaster)
        {
            OnEventMasterCollectCollect();
            GameStatisticManager.instance.LogCollectables(objectDefault);
        }

        internal CollectibleScriptable GetCollectibleSettings => objectDefault as CollectibleScriptable;
        internal PowerUpScriptable GetPowerUpSettings => objectDefault as PowerUpScriptable;

        protected override void ReadyObject()
        {
            if(!objectDefault.canRespawn) return;
            IsDisable = false;
        }
        protected override void ReloadObject(){}

        protected override void AttemptKillObstacle(PlayerMaster playerMaster)
        {
            IsDisable = true;
            if(!objectDefault.canKilled) return;
            IsDead = true;
        }

        protected override void TryReSpawn()
        {
            IsDisable = true;
            if(!objectDefault.canRespawn) return;
            IsDead = false;
            RepositionObject();
        }
        private void OnEventMasterCollectCollect()
        {
            EventMasterCollectCollect?.Invoke();
        }
    }
}