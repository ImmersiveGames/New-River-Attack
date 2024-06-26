using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems
{
    public class BossSkin : ObstacleSkin
    {
        protected override void DesativeSkin(PlayerMaster playerMaster)
        {
            //base.DesativeSkin(playerMaster);
            Debug.Log("Ativa o comportamento de morte do boss");
            
        }
    }
}