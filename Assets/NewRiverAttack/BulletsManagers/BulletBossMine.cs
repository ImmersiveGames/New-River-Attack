using NewRiverAttack.ObstaclesSystems.EnemiesSystems;
using NewRiverAttack.PlayerManagers.PlayerSystems;

namespace NewRiverAttack.BulletsManagers
{
    public class BulletBossMine :Bullet
    {
        private MineMaster _mineMaster;

        private void OnEnable()
        {
            _mineMaster = GetComponent<MineMaster>();
            _mineMaster.EventObstacleDeath += MarkToReturn;
        }

        private void OnDisable()
        {
            _mineMaster.EventObstacleDeath -= MarkToReturn;
        }
        
        private void MarkToReturn(PlayerMaster playerMaster)
        {
            Pool?.MarkForReturn(gameObject);
        }
    }
}