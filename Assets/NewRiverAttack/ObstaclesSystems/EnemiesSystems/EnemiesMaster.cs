using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;
using NewRiverAttack.PlayerManagers.PlayerSystems;

namespace NewRiverAttack.ObstaclesSystems.EnemiesSystems
{
    public class EnemiesMaster : ObstacleMaster
    {
        protected override void AttemptKillObstacle(PlayerMaster playerMaster)
        {
            IsDisable = true;
            if(!objectDefault.canKilled) return;
            IsDead = true;
        }

        protected override void TryReSpawn()
        {
            IsDisable = !GamePlayManagerRef.IsBossFight;
            if(!objectDefault.canRespawn) return;
            IsDead = false;
            RepositionObject();
        }

        protected override void ReadyObject()
        {
            IsDisable = false;
        }

        protected override void ReloadObject()
        {
            if (GamePlayManagerRef.IsBossFight)
            {
                RepositionObject();
                return;
            }
            IsDisable = true;
        }
        public EnemiesScriptable GetEnemySettings => objectDefault as EnemiesScriptable;
    }
}