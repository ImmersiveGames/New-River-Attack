﻿using ImmersiveGames.DebugManagers;
using ImmersiveGames.ObjectManagers.Interfaces;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.EnemiesSystems;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.MovementStates
{
    public class MoveStateMove : IMove
    {
        private readonly ObstacleMovement _obstacleMovement;
        private readonly EnemiesScriptable _enemiesScriptable;
        private readonly ObstacleMaster _obstacleMaster;
        private EnemiesAnimation _enemiesAnimation;
        private float _speedMovement;
        private float _elapsedTime;
        private bool _inTransition;
        public MoveStateMove(ObstacleMovement obstacleMovement)
        {
            _obstacleMovement = obstacleMovement;
            _enemiesScriptable = _obstacleMovement.GetObjectScriptable<EnemiesScriptable>();
            _obstacleMaster = _obstacleMovement.GetComponent<ObstacleMaster>();
        }
        public void EnterState()
        {
            DebugManager.Log<EnemiesMovement>($" Entrando no Estado: Move");
            _speedMovement = _obstacleMovement.GetVelocity;
            
            _elapsedTime = 0;
            _inTransition = false;
            _enemiesAnimation = _obstacleMovement.GetComponent<EnemiesAnimation>();
        }

        public void UpdateState(Transform transform, Vector3 direction)
        {
            if (!_obstacleMaster.ObjectIsReady) return;
            if (!_inTransition && _obstacleMovement.GetVelocity == 0)
            {
                _obstacleMovement.ChangeState(new MoveStateHold(_obstacleMovement));
            }
            if (!_obstacleMovement.ShouldBeMove || _inTransition) return;
            _enemiesAnimation.AnimationMove(true);
            Move(transform, direction, _speedMovement);
        }

        public void ExitState()
        {
            _inTransition = true;
            _enemiesAnimation.AnimationMove(false);
            DebugManager.Log<ObstacleMovement>($" Saindo do Estado: Move");
            
        }
        
        private void Move(Transform objMove, Vector3 direction, float velocity)
        {
            var curveValue = 1f;
            if (_enemiesScriptable.animationCurve != null && _enemiesScriptable.animationDuration != 0)
            {
                curveValue = MoveCurveAnimation(_enemiesScriptable.animationDuration, _enemiesScriptable.animationCurve);
            }
            objMove.Translate(direction * (curveValue * (velocity * Time.deltaTime)));
        }

        private float MoveCurveAnimation(float duration, AnimationCurve curve)
        {
            _elapsedTime += Time.deltaTime;

            // Verifica se a animação terminou e reinicia se necessário
            if (_elapsedTime >= duration) { _elapsedTime = 0.0f; }
            var curveFactor = Mathf.Clamp01(_elapsedTime / duration);

            // Usa a curva de animação para obter a interpolação de movimento
            return curve.Evaluate(curveFactor);
        }
    }
}