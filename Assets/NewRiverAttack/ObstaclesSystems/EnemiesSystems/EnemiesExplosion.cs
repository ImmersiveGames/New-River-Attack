using ImmersiveGames.CameraManagers;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptables;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.EnemiesSystems
{
    public class EnemiesExplosion: MonoBehaviour
    {
        private ObstacleMaster _obstacleMaster;
        private ObjectsScriptable _enemy;
        private GamePlayManager _gamePlayManager;

        #region Unity Methods

        private void OnEnable()
        {
            SetInitialReferences();
            _obstacleMaster.EventObstacleDeath += ObstacleExplode;
        }

        private void OnDisable()
        {
            _obstacleMaster.EventObstacleDeath -= ObstacleExplode;
        }

        #endregion

        private void SetInitialReferences()
        {
            _obstacleMaster = GetComponent<ObstacleMaster>();
            _enemy = _obstacleMaster.objectDefault;
        }

        private void ObstacleExplode(PlayerMaster playerMaster)
        {
            if(!_enemy.canKilled) return;
            CameraShake.ShakeCamera(_enemy.shakeIntensity,_enemy.shakeTime);
            var go = Instantiate(_enemy.deadParticlePrefab, transform);
            go.transform.localScale = new Vector3(_enemy.explodeSize,_enemy.explodeSize,_enemy.explodeSize);
            Destroy(go, _enemy.timeoutDestroy);
        }
    }
}