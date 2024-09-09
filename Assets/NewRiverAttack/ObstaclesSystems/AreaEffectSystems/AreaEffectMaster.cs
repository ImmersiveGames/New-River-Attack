using ImmersiveGames.ObjectManagers.Interfaces;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;
using NewRiverAttack.PlayerManagers.PlayerSystems;

namespace NewRiverAttack.ObstaclesSystems.AreaEffectSystems
{
    public sealed class AreaEffectMaster : ObstacleMaster, IAreaEffect
    {
        private bool _inAreaEffect;

        public bool IsInAreaEffect => _inAreaEffect;

        internal event ObstacleGenericHandler EventMasterAreaEffectEnter;
        internal event ObstacleGenericHandler EventMasterAreaEffectExit;

        internal AreaEffectScriptable GetScriptableSettings => objectDefault as AreaEffectScriptable;

        protected override void ReadyObject()
        {
            IsDisable = false;
            _inAreaEffect = false;
        }

        protected override void AttemptKillObstacle(PlayerMaster playerMaster)
        {
            IsDisable = true;

            // Se o objeto pode ser destruído, aciona a lógica de saída da área de efeito
            if (_inAreaEffect)
            {
                ExitEffect(); // Atualiza o estado para indicar que está fora do efeito
            }

            if (!objectDefault.canKilled) return;

            IsDead = true;
        }

        protected override void TryReSpawn()
        {
            IsDisable = true;
            if (!objectDefault.canRespawn) return;
            IsDead = false;
            _inAreaEffect = false;
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
            _inAreaEffect = true;
            EventMasterAreaEffectEnter?.Invoke();
        }

        private void OnEventMasterAreaEffectExit()
        {
            _inAreaEffect = false;
            EventMasterAreaEffectExit?.Invoke();
        }
    }
}