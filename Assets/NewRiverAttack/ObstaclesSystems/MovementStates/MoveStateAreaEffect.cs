using ImmersiveGames.DebugManagers;
using ImmersiveGames.ObjectManagers.Interfaces;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.MovementStates
{
    public class MoveStateAreaEffect : IMove
    {
        private float _speedMovement;
        private float _elapsedTime;
        private bool _inTransition;
        
        private readonly ObstacleMovement _obstacleMovement;
        private readonly AreaEffectScriptable _areaEffectScriptable;
        private readonly ObstacleMaster _obstacleMaster;
        public MoveStateAreaEffect(ObstacleMovement obstacleMovement)
        {
            _obstacleMovement = obstacleMovement;
            _areaEffectScriptable = _obstacleMovement.GetObjectScriptable<AreaEffectScriptable>();
            _obstacleMaster = _obstacleMovement.GetComponent<ObstacleMaster>();
        }
        public void EnterState()
        {
            DebugManager.Log<ObstacleMovement>($" Entrando no Estado: Move");
            _speedMovement = _obstacleMovement.GetVelocity;
            
            _elapsedTime = 0;
            _inTransition = false;
        }

        public void UpdateState(Transform transform, Vector3 direction)
        {
            if (!_obstacleMaster.ObjectIsReady) return;
            if (!_obstacleMovement.ShouldBeMove || _inTransition) return;
            Move(transform, direction, _speedMovement);
        }

        public void ExitState()
        {
            _inTransition = true;
            DebugManager.Log<ObstacleMovement>($" Saindo do Estado: Move");
        }
        
        private void Move(Transform objMove, Vector3 direction, float velocity)
        {
            var curveValue = 1f;
            if (_areaEffectScriptable.animationCurve != null && _areaEffectScriptable.animationDuration != 0)
            {
                curveValue = MoveCurveAnimation(_areaEffectScriptable.animationDuration, _areaEffectScriptable.animationCurve);
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