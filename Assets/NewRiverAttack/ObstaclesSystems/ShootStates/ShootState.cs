using ImmersiveGames.DebugManagers;
using ImmersiveGames.FiniteStateMachine;
using NewRiverAttack.ObstaclesSystems.EnemiesSystems;

namespace NewRiverAttack.ObstaclesSystems.ShootStates
{
    public class ShootState : IState
    {
        private readonly EnemiesShoot _enemiesShoot;
        private readonly EnemiesMaster _enemiesMaster;

        public ShootState(EnemiesShoot enemiesShoot)
        {
            _enemiesShoot = enemiesShoot;
            _enemiesMaster = enemiesShoot.GetComponent<EnemiesMaster>();
        }

        public void Tick()
        {
            // Enquanto o inimigo puder atirar (verificado por ShouldBeShoot), ele tentará atirar
            if (!_enemiesShoot.ShouldBeShoot) return;
            
            _enemiesShoot.AttemptShoot(_enemiesMaster);
        }

        public void OnEnter()
        {
            DebugManager.Log<ShootState>("Inimigo começou a atirar.");
        }

        public void OnExit()
        {
            DebugManager.Log<ShootState>("Inimigo parou de atirar.");
        }
    }
}