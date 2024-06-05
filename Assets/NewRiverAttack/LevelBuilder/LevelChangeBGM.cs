using ImmersiveGames;
using NewRiverAttack.AudioManagers;
using NewRiverAttack.LevelBuilder.Abstracts;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.LevelBuilder
{
    public class LevelChangeBGM : LevelFinishers
    {
        public EnumBgmTypes newBgmTypes;

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            var playerMaster = other.GetComponentInParent<PlayerMaster>();
            if( playerMaster == null || playerMaster.IsDisable || !inFinisher) return;
            AudioManager.PlayBGM(newBgmTypes.ToString());
        }
    }
}
