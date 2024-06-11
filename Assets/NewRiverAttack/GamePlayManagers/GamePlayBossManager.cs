using System;
using ImmersiveGames.Utils;
using NewRiverAttack.ObstaclesSystems.BossSystems;
using UnityEngine;

namespace NewRiverAttack.GamePlayManagers
{
    public class GamePlayBossManager : Singleton<GamePlayBossManager>
    {
        private BossMaster _bossMaster;
        private GamePlayManager _gamePlayManager;
        private void OnEnable()
        {
            SetInitialReferences();
            //_gamePlayManager.EventGameReady += PlayAbertura;
        }

        private void OnDisable()
        {
            //_gamePlayManager.EventGameReady += PlayAbertura;
        }

        private void SetInitialReferences()
        {
            _gamePlayManager = GamePlayManager.instance;
        }

        public void SetBoss(BossMaster bossMaster)
        {
            _bossMaster = bossMaster;
        }
        
        /*private void PlayAbertura()
        {
            Debug.Log("Play abertura");
            _bossMaster.InitializeBoss(_gamePlayManager.GetPlayerMaster(0));

        }*/
    }
}