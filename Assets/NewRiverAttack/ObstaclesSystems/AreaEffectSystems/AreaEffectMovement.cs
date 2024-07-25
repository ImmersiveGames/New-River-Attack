using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.MovementStates;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;

namespace NewRiverAttack.ObstaclesSystems.AreaEffectSystems
{
    public class AreaEffectMovement : ObstacleMovement
    {
        private AreaEffectMaster _areaEffectMaster;
        private AreaEffectAnimator _areaEffectAnimator;
        private AreaEffectScriptable _areaEffectScriptable;

        #region Unity Methods
        private void Start()
        {
            StartState = new MoveStateAreaEffect(this);
            ChangeState(StartState);
        }
        #endregion

        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            _areaEffectScriptable = ObstacleMaster.GetObjectScriptable<AreaEffectScriptable>();
            MoveVelocity = _areaEffectScriptable.moveVelocity;
        }

        protected override void ResetMovement()
        {
            SetVelocity(_areaEffectScriptable.moveVelocity);
            base.ResetMovement();
        }
        
    }
}