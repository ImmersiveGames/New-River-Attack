using ImmersiveGames.DebugManagers;
using ImmersiveGames.ObjectManagers.Interfaces;
using NewRiverAttack.ObstaclesSystems.EnemiesSystems;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptables;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.ShootStates
{
    public class ShootStateShoot : IShoot
    {
        private readonly EnemiesShoot _enemiesShoot;
        private EnemiesMaster _enemiesMaster;
        private EnemiesScriptables _enemies;
        private bool _inTransition;
        public ShootStateShoot(EnemiesShoot enemiesShoot)
        {
            _enemiesShoot = enemiesShoot;
            
        }
        public void EnterState(EnemiesMaster enemiesMaster)
        {
            DebugManager.Log<EnemiesShoot>($" Entrando no Estado: Shoot");
            _inTransition = false;
            _enemiesMaster = enemiesMaster;
            _enemies = enemiesMaster.GetEnemySettings;
            _enemiesShoot.SetDataBullet(enemiesMaster);
            _enemiesShoot.UpdateCadenceShoot();
        }

        public void UpdateState(Transform referencePosition)
        {
            DebugManager.Log<EnemiesShoot>($" Atualizando o Estado: Shoot");
            if (_inTransition) return;
            if(!_enemiesShoot.ShouldBeShoot)
                _enemiesShoot.ChangeState(new ShootStatePatrol(_enemiesShoot, false));
            else
                _enemiesShoot.AttemptShoot(_enemiesMaster);
        }

        public void ExitState()
        {
            DebugManager.Log<EnemiesShoot>($" Saindo do Estado: Shoot");
            _inTransition = true;
        }
    }
}