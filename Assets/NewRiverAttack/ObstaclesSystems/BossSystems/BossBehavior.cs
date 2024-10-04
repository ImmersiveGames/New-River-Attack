using System;
using ImmersiveGames.BehaviorsManagers;
using ImmersiveGames.BehaviorsManagers.Interfaces;
using ImmersiveGames.BehaviorTreeSystem;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems
{
    public class BossBehavior : MonoBehaviour
    {
        private BossBehaviorHandle _behavior;

        #region Unity Methods

        private void Awake()
        {
            //_behavior = new BossBehaviorHandle();
        }

        private void OnEnable()
        {
            throw new NotImplementedException();
        }

        private void Start()
        {
            throw new NotImplementedException();
        }

        private void OnDisable()
        {
            throw new NotImplementedException();
        }

        #endregion
        
        private void SetInitialReferences()
        {
            
        }
        /*public GameObject gasStation;
        
        private BehaviorManager _behaviorManager;
        private GamePlayManager _gamePlayManager;
        private GamePlayBossManager _gamePlayBossManager;

        public PlayerMaster PlayerMaster { get; private set; }
        public BossMaster BossMaster { get; private set; }

        private void OnEnable()
        {
            SetInitialReferences();
            _gamePlayManager.EventPlayerInitialize += GetPlayerMaster;
            BossMaster.EventObstacleDeath += DeathBoss;
            _gamePlayBossManager.EventEnterBoss += BossGameReady;
        }

        private async void DeathBoss(PlayerMaster playerMaster)
        {
            if( playerMaster == null || !BossMaster.IsDead) return;
            await _behaviorManager.ChangeBehaviorAsync("DeathBehavior").ConfigureAwait(false);
        }

        private void Start()
        {
            StartBehavior();
        }
        internal void StartBehavior()
        {
            _behaviorManager = new BehaviorManager(this);
            
            //var cleanShootBehavior = new CleanShootBehavior(_behaviorManager,Array.Empty<IBehavior>(), 0.5f,8);
            var multiMissiles09 = new MissileBehavior(_behaviorManager,Array.Empty<IBehavior>(),9,90f,1f,2);
            var multiMissiles05 = new MissileBehavior(_behaviorManager,Array.Empty<IBehavior>(),5,60f,1f,3);
            var mineBehavior = new MineBehavior(_behaviorManager,Array.Empty<IBehavior>(),10,1f);
            
            var enterSceneBehavior = new EnterSceneBehavior(_behaviorManager, Array.Empty<IBehavior>());
            
            var moveNorthBehavior = new MoveNorthBehavior(_behaviorManager, new IBehavior[]{multiMissiles05, mineBehavior,multiMissiles09});
            var moveSouthBehavior = new MoveSouthBehavior(_behaviorManager, new IBehavior[]{multiMissiles05,multiMissiles09});
            var moveEastBehavior = new MoveEastBehavior(_behaviorManager, new IBehavior[]{multiMissiles09,multiMissiles05,multiMissiles09});
            var moveWestBehavior = new MoveWestBehavior(_behaviorManager, new IBehavior[]{multiMissiles09,multiMissiles05,multiMissiles09});
            var deathBehavior = new DeathBehavior(_gamePlayManager, Array.Empty<IBehavior>());

            _behaviorManager.AddBehavior(enterSceneBehavior);
            _behaviorManager.AddBehavior(moveNorthBehavior);
            _behaviorManager.AddBehavior(moveSouthBehavior);
            _behaviorManager.AddBehavior(moveEastBehavior);
            _behaviorManager.AddBehavior(moveWestBehavior);
            _behaviorManager.AddBehavior(deathBehavior);
        }

        private void Update()
        {
            if (!BossMaster.ObjectIsReady) return;
            
            _behaviorManager.UpdateAsync();
        }

        private void OnDisable()
        {
            _gamePlayManager.EventPlayerInitialize -= GetPlayerMaster;
            BossMaster.EventObstacleDeath -= DeathBoss;
            _gamePlayBossManager.EventEnterBoss -= BossGameReady;
        }

        private void SetInitialReferences()
        {
            _gamePlayManager = GamePlayManager.Instance;
            _gamePlayBossManager = GamePlayBossManager.instance;
            BossMaster = GetComponent<BossMaster>();
        }
        private void GetPlayerMaster(PlayerMaster playerMaster)
        {
            PlayerMaster = playerMaster;
        }
        private async void BossGameReady()
        {
            await _behaviorManager.ChangeBehaviorAsync(EnumNameBehavior.EnterSceneBehavior.ToString()).ConfigureAwait(false);
        }*/
    }
}
