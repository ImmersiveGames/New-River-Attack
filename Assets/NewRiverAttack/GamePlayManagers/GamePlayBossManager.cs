using ImmersiveGames.Utils;
using NewRiverAttack.ObstaclesSystems.BossSystems;
using UnityEngine;

namespace NewRiverAttack.GamePlayManagers
{
    public sealed class GamePlayBossManager : Singleton<GamePlayBossManager>
    {
        public Vector2 bossAreaX = new Vector2(-70, 65);
        public Vector2 bossAreaZ = new Vector2(15, 100);
        private BossMaster _bossMaster;
        public delegate void GamePlayBossFightHandle();

        internal event GamePlayBossFightHandle EventEnterBoss;

        public void OnEventEnterBoss()
        {
            EventEnterBoss?.Invoke();
        }

        public void SetBoss(BossMaster bossMaster)
        {
            _bossMaster = bossMaster;
        }
    }
}