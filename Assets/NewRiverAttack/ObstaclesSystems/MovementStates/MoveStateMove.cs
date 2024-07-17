using ImmersiveGames.DebugManagers;
using ImmersiveGames.ObjectManagers.Interfaces;
using NewRiverAttack.ObstaclesSystems.EnemiesSystems;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.MovementStates
{
    public class MoveStateMove : IMove
    {
        private readonly EnemiesMovement _enemiesMovement;
        private readonly EnemiesScriptable _enemiesScriptable;
        private readonly EnemiesMaster _enemiesMaster;
        private EnemiesAnimation _enemiesAnimation;
        private float _speedMovement;
        private float _elapsedTime;
        private bool _inTransition;
        public MoveStateMove(EnemiesMovement enemiesMovement)
        {
            _enemiesMovement = enemiesMovement;
            _enemiesScriptable = _enemiesMovement.GetEnemySettings;
            _enemiesMaster = _enemiesMovement.GetComponent<EnemiesMaster>();
        }
        public void EnterState()
        {
            DebugManager.Log<EnemiesMovement>($" Entrando no Estado: Move");
            _speedMovement = _enemiesMovement.GetVelocity;
            
            _elapsedTime = 0;
            _inTransition = false;
            _enemiesAnimation = _enemiesMovement.GetComponent<EnemiesAnimation>();
        }

        public void UpdateState(Transform transform, Vector3 direction)
        {
            if (!_enemiesMaster.ObjectIsReady) return;
            if (!_inTransition && _enemiesMovement.GetVelocity == 0)
            {
                _enemiesMovement.ChangeState(new MoveStateHold(_enemiesMovement));
            }
            if (!_enemiesMovement.ShouldBeMove || _inTransition) return;
            _enemiesAnimation.AnimationMove(true);
            Move(transform, direction, _speedMovement);
            //DebugManager.Log<EnemiesMovement>($" Atualizando o Estado: Move");
            //DebugManager.Log<EnemiesMovement>($"Move {direction},{_speedMovement}");
        }

        public void ExitState()
        {
            _inTransition = true;
            _enemiesAnimation.AnimationMove(false);
            DebugManager.Log<EnemiesMovement>($" Saindo do Estado: Move");
            
        }
        
        private void Move(Transform objMove, Vector3 direction, float velocity)
        {
            var curveValue = 1f;
            if (_enemiesScriptable.animationCurve != null && _enemiesScriptable.animationDuration != 0)
            {
                curveValue = MoveCurveAnimation(_enemiesScriptable.animationDuration, _enemiesScriptable.animationCurve);
            }
            //Debug.Log("MOVE:" + curveValue);
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