using ImmersiveGames.CameraManagers;
using NewRiverAttack.GameManagers;
using NewRiverAttack.LevelBuilder.Abstracts;
using NewRiverAttack.PlayerManagers.PlayerSystems;
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
            if( playerMaster == null || playerMaster.IsDisable || !InFinisher) return;
            GamePlayManagerRef.FinisherGame();
            //GamePlayManagerRef.SendTo(GameManager.instance.gamePlayMode);

        }
    }
}