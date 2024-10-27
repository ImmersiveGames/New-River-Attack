using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;
using NewRiverAttack.PlayerManagers.PlayerSystems;

namespace NewRiverAttack.ObstaclesSystems.EnemiesSystems
{
    public class EnemiesMaster : ObstacleMaster
    {
        private GameLevelManager _gameLevelManager;
        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            _gameLevelManager = GameLevelManager.Instance;
        }

        protected override void AttemptKillObstacle(PlayerMaster playerMaster)
        {
            IsDisable = true;
            if(!objectDefault.canKilled) return;
            IsDead = true;
        }

        protected override void TryReSpawn()
        {
            IsDisable = !_gameLevelManager.IsBossFight;
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
            if (_gameLevelManager.IsBossFight)
            {
                RepositionObject();
                return;
            }
            IsDisable = true;
        }
        public EnemiesScriptable GetEnemySettings => objectDefault as EnemiesScriptable;
    }
}