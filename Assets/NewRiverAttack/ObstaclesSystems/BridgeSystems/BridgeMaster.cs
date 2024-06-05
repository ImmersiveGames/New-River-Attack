using NewRiverAttack.LevelBuilder;
using NewRiverAttack.ObstaclesSystems.EnemiesSystems;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptables;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BridgeSystems
{
    public class BridgeMaster : EnemiesMaster
    {
        private float _zPosition;
        private BridgeScriptable GetBridgeSettings => objectDefault as BridgeScriptable;

        protected override void AttemptKillObstacle(PlayerMaster playerMaster)
        {
            base.AttemptKillObstacle(playerMaster);
            
            //TODO: Aqui é um bom local para ajustar os saves e atualizar o cenário.
            LevelBuilderManager.instance.OptimizeSegments(transform.position.z);
            
            if (!GetBridgeSettings.isCheckPoint) return;
            var position = transform.position;
            var savePosition = new Vector3(position.x, playerMaster.transform.position.y,
                position.z);
            playerMaster.SavePosition(savePosition);
        }

        
    }
}