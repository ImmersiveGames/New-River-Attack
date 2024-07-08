using System;
using ImmersiveGames.PoolManagers.Interface;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.EnemiesSystems;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems
{
    public sealed class BossMaster : EnemiesMaster, IHasPool
    {
        
        private PlayerMaster _playerMaster;
        
        #region Delagates & Events
        public delegate void BossGenericHandler();
        public event BossGenericHandler EventBossEmerge;
        public event BossGenericHandler EventBossSubmerge;

        #endregion
        
        private void Start()
        {
            GamePlayBossManager.instance.SetBoss(this);
        }
        
        internal void OnEventBossSubmerge()
        {
            EventBossSubmerge?.Invoke();
        }

        internal void OnEventBossEmerge()
        {
            EventBossEmerge?.Invoke();
        }
    }
}