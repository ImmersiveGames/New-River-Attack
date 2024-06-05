using System;
using ImmersiveGames.CameraManagers;
using NewRiverAttack.GameManagers;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.HUBManagers;
using NewRiverAttack.LevelBuilder.Abstracts;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using NewRiverAttack.StateManagers;
using UnityEngine;

namespace NewRiverAttack.LevelBuilder
{
    public class GamePlayFinishLevel : LevelFinishers
    {
        public Transform endCameraPosition;
       
        protected override void Start()
        {
            base.Start();
            if(GetTilePosition.z > 0)
                CameraManager.RepositionEndCamera(endCameraPosition.position);
        }

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            var playerMaster = other.GetComponentInParent<PlayerMaster>();
            if( playerMaster == null || playerMaster.IsDisable || !inFinisher) return;
            CameraManager.ActiveEndCamera(true);
            //endVirtualCamera.gameObject.SetActive(true);
            gamePlayManagerRef.OnEventGameFinisher();
            Debug.Log($"FINISH GAME");
            
            switch (GameManager.instance.gamePlayMode)
            {
                case GamePlayModes.MissionMode:
                    GameManager.instance.ActiveLevel.hudPath.levelsStates = LevelsStates.Complete;
                    Invoke(nameof(SendToHub), 2f);
                    break;
                case GamePlayModes.ClassicMode:
                    Invoke(nameof(SendToCompleteGame), 2f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async void SendToHub()
        {
            await GameManager.StateManager.ChangeStateAsync(StatesNames.GameStateHub.ToString()).ConfigureAwait(false);
        }
        
        private async void SendToCompleteGame()
        {
            await GameManager.StateManager.ChangeStateAsync(StatesNames.GameStateEndGame.ToString()).ConfigureAwait(false);
        }
    }
}