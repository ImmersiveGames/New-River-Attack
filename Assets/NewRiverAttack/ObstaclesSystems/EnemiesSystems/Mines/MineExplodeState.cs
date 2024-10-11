using ImmersiveGames.FiniteStateMachine;
using NewRiverAttack.VfxSystems;
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
            var effectPosition = new Vector3(_mineFuse.transform.position.x, 1f, _mineFuse.transform.position.z);
            var effect = Object.Instantiate(_mineFuse.detonationVfx, effectPosition, _mineFuse.transform.rotation);
            effect.GetComponent<BombVfx>().InitializeBomb(_mineFuse.radiusExpendSize,_mineFuse.expansionDuration,_mineFuse.shakeForce, _mineFuse.shakeTime);
        }

        // Realiza o reset ao sair do estado de explosão
        public void OnExit()
        {
            _mineFuse.ReturnBullet();
        }
    }
}
