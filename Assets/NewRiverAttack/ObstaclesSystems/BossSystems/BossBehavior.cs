using System;
using System.Threading;
using ImmersiveGames.BehaviorsManagers;
using ImmersiveGames.BehaviorsManagers.Interfaces;
using NewRiverAttack.GamePlayManagers;
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

            var enterSceneBehavior = new EnterSceneBehavior(Array.Empty<IBehavior>(), this);

            var specificSubBehavior = new SpecificBehavior(Array.Empty<IBehavior>(), this);

            var moveNorthBehavior = new MoveNorthBehavior(new IBehavior[] { specificSubBehavior }, this);
            var moveSouthBehavior = new MoveSouthBehavior(new IBehavior[] { specificSubBehavior }, this);
            var moveEastBehavior = new MoveEastBehavior(new IBehavior[] { specificSubBehavior }, this);
            var moveWestBehavior = new MoveWestBehavior(new IBehavior[] { specificSubBehavior }, this);

            _behaviorManager.AddBehavior(enterSceneBehavior);
            _behaviorManager.AddBehavior(moveNorthBehavior);
            _behaviorManager.AddBehavior(moveSouthBehavior);
            _behaviorManager.AddBehavior(moveEastBehavior);
            _behaviorManager.AddBehavior(moveWestBehavior);
        }

        private async void Update()
        {
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

        internal BehaviorManager GetBehaviorManager => _behaviorManager;

        private async void BossGameReady()
        {
            await _behaviorManager.ChangeBehaviorAsync("EnterSceneBehavior").ConfigureAwait(false);
        }
    }
}