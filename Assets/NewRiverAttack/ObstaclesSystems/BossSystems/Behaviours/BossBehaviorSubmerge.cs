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
            BossMaster.IsEmerge = false;
            Invulnerability(true);
            AnimationSubmerge();
        }

        public Func<NodeState> GetNodeFunction()
        {
            return SubmergeBoss;
        }
        private NodeState SubmergeBoss()
        {
            _elapsedTime += Time.deltaTime;
            return _elapsedTime >= _timerSubmerge ? NodeState.Success : NodeState.Running;
        }
        
        private void AnimationSubmerge()
        {
            if (Animator == null || string.IsNullOrEmpty(onSubmerge)) return;
            Animator.SetTrigger(onSubmerge);
        }
        public void ResetBehavior()
        {
            _elapsedTime = 0;
        }

        public string NodeName => "BossSubmerge";
        public int NodeID => 0;
    }
}