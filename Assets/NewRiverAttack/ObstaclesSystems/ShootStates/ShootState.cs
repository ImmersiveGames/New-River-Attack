using ImmersiveGames.DebugManagers;
using ImmersiveGames.FiniteStateMachine;
using NewRiverAttack.ObstaclesSystems.BossSystems.Helpers;
using NewRiverAttack.ObstaclesSystems.EnemiesSystems;

namespace NewRiverAttack.ObstaclesSystems.ShootStates
{
    public class ShootState : IState
    {
        private readonly EnemiesShoot _enemiesShoot;
        private readonly ForwardShotPattern _forwardShotPattern;

        public ShootState(EnemiesShoot enemiesShoot)
        {
            _enemiesShoot = enemiesShoot;
            _forwardShotPattern = new ForwardShotPattern(_enemiesShoot.GetCadenceShoot);
        }

        public void Tick()
        {
            if (!_enemiesShoot.ShouldBeReady) return;

            // Executa o padrão de tiro
            _forwardShotPattern.Execute(_enemiesShoot.SpawnPoint, _enemiesShoot);
        }

        public void OnEnter()
        {
            DebugManager.Log<ShootState>("Inimigo começou a atirar.");
        }

        public void OnExit()
        {
            DebugManager.Log<ShootState>("Inimigo parou de atirar.");
            _enemiesShoot.SetTarget(null); // Limpa o alvo quando sai do estado de tiro
        }
    }
}