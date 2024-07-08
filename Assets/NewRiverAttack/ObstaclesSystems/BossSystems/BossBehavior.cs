using System;
using ImmersiveGames.BehaviorsManagers;
using ImmersiveGames.BehaviorsManagers.Interfaces;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.BossSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems
{
    public class BossBehavior : MonoBehaviour
    {
        private BehaviorManager _behaviorManager;
        private GamePlayManager _gamePlayManager;
        private GamePlayBossManager _gamePlayBossManager;

        public PlayerMaster PlayerMaster { get; private set; }
        public BossMaster BossMaster { get; private set; }
        
        

        private void OnEnable()
        {
            SetInitialReferences();
            _gamePlayManager.EventPlayerInitialize += GetPlayerMaster;
            _gamePlayBossManager.EventEnterBoss += BossGameReady;
        }

        private void Start()
        {
            _behaviorManager = new BehaviorManager(this);
            
            var cleanShootBehavior = new ShootBehavior(_behaviorManager,Array.Empty<IBehavior>());
            var multiMissilesBehavior = new MissileBehavior(_behaviorManager,Array.Empty<IBehavior>());
            var mineBehavior = new MineBehavior(_behaviorManager,Array.Empty<IBehavior>());
            var subs = new TesteSubs(_behaviorManager,Array.Empty<IBehavior>());
            var subs2 = new TesteSubs2(_behaviorManager,Array.Empty<IBehavior>());
            var enterSceneBehavior = new EnterSceneBehavior(_behaviorManager, Array.Empty<IBehavior>());
            
            var moveNorthBehavior = new MoveNorthBehavior(_behaviorManager, new IBehavior[]{cleanShootBehavior, multiMissilesBehavior});
            var moveSouthBehavior = new MoveSouthBehavior(_behaviorManager, new IBehavior[]{cleanShootBehavior, subs2});
            var moveEastBehavior = new MoveEastBehavior(_behaviorManager, new IBehavior[]{cleanShootBehavior, subs2});
            var moveWestBehavior = new MoveWestBehavior(_behaviorManager, new IBehavior[]{cleanShootBehavior, subs2});

            _behaviorManager.AddBehavior(enterSceneBehavior);
            _behaviorManager.AddBehavior(moveNorthBehavior);
            _behaviorManager.AddBehavior(moveSouthBehavior);
            _behaviorManager.AddBehavior(moveEastBehavior);
            _behaviorManager.AddBehavior(moveWestBehavior);
        }

        private async void Update()
        {
            if (Input.GetKey(KeyCode.K))
            {
                _behaviorManager.StopCurrentBehavior();
            }
            if (Input.GetKey(KeyCode.L))
            {
                _behaviorManager.StopCurrentSubBehavior();
            }
            if (Input.GetKey(KeyCode.J))
            {
                await _behaviorManager.ChangeBehaviorAsync(EnumNameBehavior.MoveNorthBehavior.ToString()).ConfigureAwait(false);
            }
            if (!BossMaster.ObjectIsReady) return;
            
            await _behaviorManager.UpdateAsync().ConfigureAwait(false);
        }

        private void OnDisable()
        {
            _gamePlayBossManager.EventEnterBoss -= BossGameReady;
        }

        private void OnApplicationQuit()
        {
            _behaviorManager.StopCurrentBehavior();
        }

        private void SetInitialReferences()
        {
            _gamePlayManager = GamePlayManager.instance;
            _gamePlayBossManager = GamePlayBossManager.instance;
            BossMaster = GetComponent<BossMaster>();
        }

        private void GetPlayerMaster(PlayerMaster playerMaster)
        {
            PlayerMaster = playerMaster;
        }

        private BossMissileShoot BossMissileShoot(int numMissile, float angle)
        {
            var bossShoot = GetComponent<BossMissileShoot>();
            bossShoot.SetMissiles(numMissile, angle);
            return bossShoot;
        }

        private async void BossGameReady()
        {
            await _behaviorManager.ChangeBehaviorAsync(EnumNameBehavior.EnterSceneBehavior.ToString()).ConfigureAwait(false);
        }
    }
}
