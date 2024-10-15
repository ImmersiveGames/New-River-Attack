﻿using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.EnemiesSystems;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems
{
    public sealed class BossMaster : EnemiesMaster
    {
        public bool IsEmerge { get; set; }
        public PlayerMaster PlayerMaster { get; private set; }

        #region Delagates & Events

        public delegate void BossGenericHandler();
        public event BossGenericHandler EventBossResetForEnter;

        #endregion

        #region Unity Methods

        protected override void OnEnable()
        {
            base.OnEnable();
            GamePlayManagerRef.EventGameReload += ReloadBoss;
            GamePlayManagerRef.EventPlayerInitialize += GetPlayerMaster;
        }

        private void Start()
        {
            GamePlayBossManager.instance.SetBoss(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            GamePlayManagerRef.EventGameReload -= ReloadBoss;
            GamePlayManagerRef.EventPlayerInitialize -= GetPlayerMaster;
        }

        #endregion


        

        private void GetPlayerMaster(PlayerMaster playerMaster)
        {
            PlayerMaster = playerMaster;
        }

        private void ReloadBoss()
        {
            GamePlayBossManager.instance.SetBoss(this);
            var behaviors = GetComponent<BossBehaviorHandle>();
            behaviors.ResetAll();
            gameObject.transform.localScale = Vector3.one;
        }

        protected override void AttemptKillObstacle(PlayerMaster playerMaster)
        {
            base.AttemptKillObstacle(playerMaster);
            playerMaster.OnEventPlayerMasterStopDecoyFuel(true);
        }
        internal void OnEventBossResetForEnter()
        {
            EventBossResetForEnter?.Invoke();
        }
    }
}