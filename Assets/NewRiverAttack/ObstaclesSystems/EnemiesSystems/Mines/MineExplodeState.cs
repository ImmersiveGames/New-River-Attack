using ImmersiveGames.CameraManagers;
using ImmersiveGames.FiniteStateMachine;
using NewRiverAttack.PlayerManagers.Tags;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.EnemiesSystems.Mines
{
    public class MineExplodeState : IState
    {
        // Referências e variáveis principais
        private readonly MineFuse _mineFuse;

        // Construtor
        public MineExplodeState(MineFuse mineFuse)
        {
            _mineFuse = mineFuse;
        }

        // Método chamado a cada frame durante a explosão
        public void Tick()
        {
            
        }

        // Configura o estado inicial ao entrar no estado de explosão
        public void OnEnter()
        {
            Debug.Log("Enter EXPLODE");
            _mineFuse.ReturnBullet();
        }

        // Realiza o reset ao sair do estado de explosão
        public void OnExit()
        {
            Debug.Log("EXIT EXPLODE");
        }
    }
}
