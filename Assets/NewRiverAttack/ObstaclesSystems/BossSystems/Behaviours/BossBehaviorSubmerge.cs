using System;
using ImmersiveGames.BehaviorTreeSystem.Interface;
using ImmersiveGames.Utils;
using NewRiverAttack.ObstaclesSystems.BossSystems.Abstracts;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class BossBehaviorSubmerge : BossBehaviorAnimation, INodeFunctionProvider
    {
        [SerializeField] private string onSubmerge = "Submerge";

        private float _timerSubmerge;
        private float _elapsedTime;

        #region Unity Methods
        private void Start()
        {
            _timerSubmerge = AnimationDuration.GetAnimationDuration(Animator, onSubmerge);
        }

        #endregion

        public void OnEnter()
        {
            ResetBehavior();
            Invulnerability(true);
            AnimationSubmerge();
        }

        public Func<NodeState> GetNodeFunction()
        {
            return SubmergeBoss;
        }
        private NodeState SubmergeBoss()
        {
            _elapsedTime -= Time.deltaTime;
            //Debug.Log($"Sub: {_elapsedTime}");
            return _elapsedTime <= 0 ? NodeState.Success : NodeState.Running;
        }
        
        private void AnimationSubmerge()
        {
            if (Animator == null || string.IsNullOrEmpty(onSubmerge)) return;
            Animator.SetTrigger(onSubmerge);
        }

        private void ResetBehavior()
        {
            _elapsedTime = _timerSubmerge;
        }

        public string NodeName => "BossSubmerge";
        public int NodeID => 0;
    }
}