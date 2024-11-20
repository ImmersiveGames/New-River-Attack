using ImmersiveGames.DebugManagers;
using ImmersiveGames.FiniteStateMachine;
using NewRiverAttack.ObstaclesSystems.EnemiesSystems;

namespace NewRiverAttack.ObstaclesSystems.ShootStates
{
    public class ShootState : IState
    {
        private readonly EnemiesShoot _enemiesShoot;

        public ShootState(EnemiesShoot enemiesShoot)
        {
            _enemiesShoot = enemiesShoot;
        }

        public void Tick()
        {
            // Checa se o inimigo está visível e se o cooldown está completo
            if (!_enemiesShoot.ShootIsReady) return;
            // Executa o padrão de tiro e reproduz o som
            _enemiesShoot.ExecuteShootPattern();
        }

        public void OnEnter() => DebugManager.Log<ShootState>("Inimigo entrou no estado de Tiro.");

        public void OnExit()
        {
            DebugManager.Log<ShootState>("Inimigo saiu do estado de Tiro.");
            _enemiesShoot.SetTarget(null); // Limpa o alvo ao sair do estado de tiro
        }
    }
}