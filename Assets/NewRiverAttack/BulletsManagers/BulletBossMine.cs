using NewRiverAttack.ObstaclesSystems.EnemiesSystems.Mines;
using NewRiverAttack.PlayerManagers.PlayerSystems;

namespace NewRiverAttack.BulletsManagers
{
    public class BulletBossMine : Bullet
    {
        private MineMaster _mineMaster;

        private void Awake()
        {
            _mineMaster = GetComponent<MineMaster>();
        }

        private void OnEnable()
        {
            _mineMaster.EventObstacleDeath += MarkToReturn;
        }

        private void OnDisable()
        {
            _mineMaster.EventObstacleDeath += MarkToReturn;
        }

        //Marcado para retornar ao pool quando destruído;
        private void MarkToReturn(PlayerMaster playerMaster)
        {
            Pool?.MarkForReturn(gameObject);
        }
    }
}