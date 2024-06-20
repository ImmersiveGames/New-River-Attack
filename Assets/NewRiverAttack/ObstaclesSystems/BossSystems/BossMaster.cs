using DG.Tweening;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.EnemiesSystems;
using NewRiverAttack.PlayerManagers.PlayerSystems;

namespace NewRiverAttack.ObstaclesSystems.BossSystems
{
    public sealed class BossMaster : EnemiesMaster
    {
        
        private PlayerMaster _playerMaster;
        
        private void Start()
        {
            GamePlayBossManager.instance.SetBoss(this);
        }
    }
}