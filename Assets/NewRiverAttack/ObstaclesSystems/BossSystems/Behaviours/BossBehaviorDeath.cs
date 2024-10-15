using System;
using ImmersiveGames.BehaviorTreeSystem.Interface;
using ImmersiveGames.Utils;
using NewRiverAttack.GameManagers;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.BossSystems.Abstracts;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class BossBehaviorDeath : BossBehaviorAnimation, INodeFunctionProvider
    {
        [SerializeField] private string onBossDeath = "Death";
        [SerializeField] private float extendTime = 2f;
        
        private float _timerDeath;
        private float _elapsedTime;
        
        
        #region Unity Methods
        
        private void Start()
        {
            _timerDeath = AnimationDuration.GetAnimationDuration(Animator, onBossDeath) + extendTime;
        }

        #endregion
        public void OnEnter()
        {
            Invulnerability(true);
            AnimationBossDeath();
        }
        
        private NodeState BossDeath()
        {
            _elapsedTime += Time.deltaTime;
            return _elapsedTime >= _timerDeath ? NodeState.Success : NodeState.Running;
        }
        private void AnimationBossDeath()
        {
            if (Animator == null || string.IsNullOrEmpty(onBossDeath)) return;
            Animator.SetTrigger(onBossDeath);
        }
        public Func<NodeState> GetNodeFunction()
        {
            return BossDeath;
        }

        public void ResetBehavior()
        {
            _elapsedTime = 0;
        }

        public string NodeName => "BossDeath";
        public int NodeID => 0;
    }
}