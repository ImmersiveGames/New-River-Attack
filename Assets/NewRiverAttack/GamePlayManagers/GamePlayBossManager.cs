using ImmersiveGames.Utils;
using NewRiverAttack.ObstaclesSystems.BossSystems;

namespace NewRiverAttack.GamePlayManagers
{
    public class GamePlayBossManager : Singleton<GamePlayBossManager>
    {
        private BossMaster _bossMaster;
        public delegate void GamePlayBossFightHandle();

        internal event GamePlayBossFightHandle EventEnterBoss;

        public virtual void OnEventEnterBoss()
        {
            EventEnterBoss?.Invoke();
        }

        public void SetBoss(BossMaster bossMaster)
        {
            _bossMaster = bossMaster;
        }
    }
}