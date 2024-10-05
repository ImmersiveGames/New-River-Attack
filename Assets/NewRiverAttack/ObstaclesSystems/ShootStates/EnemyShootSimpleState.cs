using ImmersiveGames.DebugManagers;
using ImmersiveGames.ObjectManagers.Interfaces;
using NewRiverAttack.ObstaclesSystems.EnemiesSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.ShootStates
{
    public class EnemyShootSimpleState : IShoot
    {
        private readonly EnemiesShootOld _enemiesShootOld;
        private EnemiesMaster _enemiesMaster;
        private bool _inTransition;
        public EnemyShootSimpleState(EnemiesShootOld enemiesShootOld)
        {
            _enemiesShootOld = enemiesShootOld;
            
        }
        public void EnterState(EnemiesMaster enemiesMaster)
        {
            DebugManager.Log<EnemiesShootOld>($" Entrando no Estado: Shoot");
            _inTransition = false;
            _enemiesMaster = enemiesMaster;
            _enemiesShootOld.SetDataBullet(enemiesMaster);
            _enemiesShootOld.UpdateCadenceShoot();
        }

        public void UpdateState(Transform referencePosition)
        {
            DebugManager.Log<EnemiesShootOld>($" Atualizando o Estado: Shoot");
            if (_inTransition) return;
            if(!_enemiesShootOld.ShouldBeShoot)
                _enemiesShootOld.ChangeState(new EnemyShootStatePatrol(_enemiesShootOld, false));
            else
                _enemiesShootOld.AttemptShoot(_enemiesMaster);
        }

        public void ExitState()
        {
            DebugManager.Log<EnemiesShootOld>($" Saindo do Estado: Shoot");
            _inTransition = true;
        }
    }
}