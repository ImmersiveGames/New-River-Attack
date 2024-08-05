using ImmersiveGames.PoolManagers.Interface;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.EnemiesSystems
{
    public class EnemiesMaster : ObstacleMaster, IHasPool
    {
        public delegate void ObstaclePositionHandler(Vector2 position);
        public event ObstaclePositionHandler EventSpawnObject;
        protected override void AttemptKillObstacle(PlayerMaster playerMaster)
        {
            IsDisable = true;
            if(!objectDefault.canKilled) return;
            IsDead = true;
        }

        protected override void TryReSpawn()
        {
            //GamePlayManagerRef.IsBossFight
            IsDisable = !GamePlayManagerRef.IsBossFight;
            if(!objectDefault.canRespawn) return;
            IsDead = false;
            RepositionObject();
        }

        protected override void ReadyObject()
        {
            IsDisable = false;
        }
        
        public EnemiesScriptable GetEnemySettings => objectDefault as EnemiesScriptable;

        public void OnEventSpawnObject(Vector2 position)
        {
            EventSpawnObject?.Invoke(position);
        }
    }
}