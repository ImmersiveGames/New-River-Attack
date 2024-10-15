using System;
using ImmersiveGames.BehaviorTreeSystem.Interface;
using NewRiverAttack.GameManagers;
using NewRiverAttack.GamePlayManagers;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class BossBehaviorFinishGame : MonoBehaviour, INodeFunctionProvider
    {
        [SerializeField] private float extendTime = 3f;

        private GamePlayManager _gamePlayManager;
        
        private float _timerFinish;
        private float _elapsedTime;

        #region Unity Methods

        private void Start()
        {
            _gamePlayManager = GamePlayManager.Instance;
            _timerFinish = extendTime;
        }

        #endregion
        public void OnEnter()
        {
            _gamePlayManager.FinisherGame();
        }

        public void OnExit()
        {
            _gamePlayManager.SendTo(GameManager.instance.gamePlayMode);
        }
        public Func<NodeState> GetNodeFunction()
        {
            return FinishGame;
        }

        private NodeState FinishGame()
        {
            _elapsedTime += Time.deltaTime;
            return _elapsedTime >= _timerFinish ? NodeState.Success : NodeState.Running;
        }
        public void ResetBehavior()
        {
            _elapsedTime = 0;
        }

        public string NodeName => "FinishGame";
        public int NodeID => 0;
    }
}