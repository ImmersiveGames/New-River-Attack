using System;
using ImmersiveGames.BehaviorsManagers;
using ImmersiveGames.BehaviorsManagers.Interfaces;
using ImmersiveGames.Utils;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;
using Random = UnityEngine.Random;

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

            var testeA = new TestSubBehaviorsA(_behaviorManager, Array.Empty<IBehavior>());
            var testeB = new TestSubBehaviorsB(_behaviorManager, Array.Empty<IBehavior>());
            var testeC = new TestSubBehaviorsC(_behaviorManager, Array.Empty<IBehavior>());
            var testeD = new TestSubBehaviorsD(_behaviorManager, Array.Empty<IBehavior>());
            
            //var cleanShootBehavior = new ShootBehavior(_behaviorManager,Array.Empty<IBehavior>());
            var multiMissiles09 = new MissileBehavior(_behaviorManager,Array.Empty<IBehavior>(),9,90f,5);
            var multiMissiles05 = new MissileBehavior(_behaviorManager,Array.Empty<IBehavior>(),5,60f,3);
            
            var emergeBehavior = new EmergeBehavior(_behaviorManager,Array.Empty<IBehavior>());
            var submergeBehavior = new SubmergeBehavior(_behaviorManager,Array.Empty<IBehavior>());
            
            var mineBehavior = new MineBehavior(_behaviorManager,Array.Empty<IBehavior>());
            var enterSceneBehavior = new EnterSceneBehavior(_behaviorManager, Array.Empty<IBehavior>());
            
            var moveNorthBehavior = new MoveNorthBehavior(_behaviorManager, new IBehavior[]{multiMissiles05,multiMissiles09,mineBehavior});
            var moveSouthBehavior = new MoveSouthBehavior(_behaviorManager, new IBehavior[]{testeB,testeC,testeB});
            var moveEastBehavior = new MoveEastBehavior(_behaviorManager, new IBehavior[]{multiMissiles05,multiMissiles09});
            var moveWestBehavior = new MoveWestBehavior(_behaviorManager, new IBehavior[]{testeC,testeA,testeC});

            _behaviorManager.AddBehavior(enterSceneBehavior);
            _behaviorManager.AddBehavior(moveNorthBehavior);
            _behaviorManager.AddBehavior(moveSouthBehavior);
            _behaviorManager.AddBehavior(moveEastBehavior);
            _behaviorManager.AddBehavior(moveWestBehavior);
        }

        private async void Update()
        {
            if (Input.GetKey(KeyCode.I))
            {
                await _behaviorManager.ChangeBehaviorAsync("MoveNorthBehavior").ConfigureAwait(false);
            }
            if (Input.GetKey(KeyCode.J))
            {
                await _behaviorManager.ChangeBehaviorAsync("MoveWestBehavior").ConfigureAwait(false);
            }
            
            if (Input.GetKey(KeyCode.K))
            {
                await _behaviorManager.ChangeBehaviorAsync("MoveSouthBehavior").ConfigureAwait(false);
            }
            if (Input.GetKey(KeyCode.Alpha1))
            {
                await _behaviorManager.SubBehaviorManager.ChangeBehaviorAsync("TestSubBehaviorsA").ConfigureAwait(false);
            }
            if (Input.GetKey(KeyCode.Alpha2))
            {
                await _behaviorManager.SubBehaviorManager.ChangeBehaviorAsync("MineBehavior").ConfigureAwait(false);
            }
            if (Input.GetKey(KeyCode.Alpha3))
            {
                await _behaviorManager.SubBehaviorManager.ChangeBehaviorAsync("MissileBehavior_9_90_5").ConfigureAwait(false);
            }
            if (Input.GetKey(KeyCode.Alpha4))
            {
                await _behaviorManager.SubBehaviorManager.ChangeBehaviorAsync("MissileBehavior_5_60_3").ConfigureAwait(false);
            }
            if (!BossMaster.ObjectIsReady) return;
            
            _behaviorManager.UpdateAsync();
        }

        private void OnDisable()
        {
            _gamePlayBossManager.EventEnterBoss -= BossGameReady;
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
        

        private async void BossGameReady()
        {
            await _behaviorManager.ChangeBehaviorAsync(EnumNameBehavior.EnterSceneBehavior.ToString()).ConfigureAwait(false);
        }
    }
}
