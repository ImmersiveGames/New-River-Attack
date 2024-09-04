﻿using NewRiverAttack.GameManagers;
using NewRiverAttack.GameStatisticsSystem;
using NewRiverAttack.LevelBuilder;
using NewRiverAttack.ObstaclesSystems.EnemiesSystems;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;
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
            LevelBuilderManager.Instance.OptimizeSegments(transform.position.z);
            
            if (!GetBridgeSettings.isCheckPoint) return;
            GameStatisticManager.instance.LogCompletePath(1, GameManager.instance.gamePlayMode);
            var position = transform.position;
            var savePosition = new Vector3(position.x, playerMaster.transform.position.y,
                position.z);
            playerMaster.SavePosition(savePosition);
        }

        
    }
}