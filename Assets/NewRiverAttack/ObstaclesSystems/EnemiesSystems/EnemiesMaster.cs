using ImmersiveGames.PoolManagers.Interface;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;
using NewRiverAttack.PlayerManagers.PlayerSystems;

namespace NewRiverAttack.ObstaclesSystems.EnemiesSystems
{
    public class EnemiesMaster : ObstacleMaster, IHasPool
    {
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

        protected override void ReadyObject()
        {
            IsDisable = false;
        }
        
        public EnemiesScriptables GetEnemySettings => objectDefault as EnemiesScriptables;
    }
}