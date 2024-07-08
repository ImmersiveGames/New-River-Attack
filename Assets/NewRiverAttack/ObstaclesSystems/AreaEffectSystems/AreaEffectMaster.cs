using ImmersiveGames.ObjectManagers.Interfaces;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;
using NewRiverAttack.PlayerManagers.PlayerSystems;

namespace NewRiverAttack.ObstaclesSystems.AreaEffectSystems
{
    public sealed class AreaEffectMaster : ObstacleMaster, IAreaEffect
    {
        internal bool InAreaEffect;
        internal event ObstacleGenericHandler EventMasterAreaEffectEnter;
        internal event ObstacleGenericHandler EventMasterAreaEffectExit;
        
        internal AreaEffectScriptable GetScriptableSettings => objectDefault as AreaEffectScriptable;
        protected override void ReadyObject()
        {
            IsDisable = false;
        }

        protected override void AttemptKillObstacle(PlayerMaster playerMaster)
        {
            IsDisable = true;
            if(!objectDefault.canKilled) return;
            IsDead = true;
        }

        protected override void TryReSpawn()
        {
            IsDisable = true;
            if(!objectDefault.canRespawn) return;
            IsDead = false;
            RepositionObject();
        }

        public void EnterEffect()
        {
            OnEventMasterAreaEffectEnter();
        }

        public void ExitEffect()
        {
            OnEventMasterAreaEffectExit();
        }

        private void OnEventMasterAreaEffectEnter()
        {
            InAreaEffect = true;
            EventMasterAreaEffectEnter?.Invoke();
        }
        private void OnEventMasterAreaEffectExit()
        {
            InAreaEffect = false;
            EventMasterAreaEffectExit?.Invoke();
        }
    }
}