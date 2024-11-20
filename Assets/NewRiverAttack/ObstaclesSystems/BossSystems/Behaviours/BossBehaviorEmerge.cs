using System;
using ImmersiveGames.BehaviorTreeSystem.Interface;
using ImmersiveGames.Utils;
using NewRiverAttack.ObstaclesSystems.BossSystems.Abstracts;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class BossBehaviorEmerge : BossBehaviorAnimation, INodeFunctionProvider
    {
        [SerializeField] private string onEmerge = "Emerge";
        private float _timerEmerge;
        private float _elapsedTime;

        #region Unity Methods
        private void Start()
        {
            //Aqui precisa ser o nome do Submerge porque o emerge é só a animação tocando ao contrario
            _timerEmerge = AnimationDuration.GetAnimationDuration(Animator, "Submerge");
        }

        #endregion
        public void OnEnter()
        {
            ResetBehavior();
            AnimationEmerge();
        }

        public void OnExit()
        {
            Invulnerability(false);
        }

        public Func<NodeState> GetNodeFunction()
        {
            return EmergeBoss;
        }
        private NodeState EmergeBoss()
        {
            _elapsedTime -= Time.deltaTime;
            //Debug.Log($"Emerge: {_elapsedTime}");
            return _elapsedTime <= 0 ? NodeState.Success : NodeState.Running;
        }
        private void AnimationEmerge()
        {
            if (Animator == null || string.IsNullOrEmpty(onEmerge)) return;
            Animator.SetTrigger(onEmerge);
        }
        public void ResetBehavior()
        {
            _elapsedTime = _timerEmerge;
        }

        public string NodeName => "BossEmerge";
        public int NodeID => 0;
    }
}