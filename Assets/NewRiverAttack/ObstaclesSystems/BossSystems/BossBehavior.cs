using System;
using ImmersiveGames.StateManagers;
using ImmersiveGames.StateManagers.Interfaces;
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

        public PlayerMaster PlayerMaster { get; private set; }

        public BossMaster BossMaster { get; private set; }

        private void OnEnable()
        {
            SetInitialReferences();
            _gamePlayManager.EventPlayerInitialize += GetPlayerMaster;
            _gamePlayManager.EventGameReady += BossGameReady;
        }

        private void Start()
        {
            _behaviorManager = new BehaviorManager(this);
            var comportamento01 = new EnterSceneBehavior();
            var comportamento02 = new MoveNorthBehavior();
            //var behavior2 = new BehaviorWithoutBehaviors();

            _behaviorManager.AddBehavior(comportamento01);
            _behaviorManager.AddBehavior(comportamento02);
        }

        private void Update()
        {
            _ = _behaviorManager.UpdateAsync();
        }

        private void OnDisable()
        {
            _gamePlayManager.EventGameReady -= BossGameReady;
        }

        void OnApplicationQuit()
        {
            _behaviorManager.StopCurrentBehavior();
        }
        
        private void SetInitialReferences()
        {
            _gamePlayManager = GamePlayManager.instance;
            BossMaster = GetComponent<BossMaster>();
        }
        
        private void GetPlayerMaster(PlayerMaster playerMaster)
        {
            PlayerMaster = playerMaster;
        }
        
        private void BossGameReady()
        {
            Debug.Log($"Game Ready - {PlayerMaster}");
            _ = _behaviorManager.ChangeBehaviorAsync(EnumNameBehavior.EnterSceneBehavior.ToString());
        }
    }
}