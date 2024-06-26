using System;
using ImmersiveGames.BulletsManagers;
using ImmersiveGames.DebugManagers;
using NewRiverAttack.BulletsManagers;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.Abstracts
{
    public abstract class ObstacleCollider : MonoBehaviour
    {
        protected ObstacleMaster ObstacleMaster;
        private int _obstacleHp;
        #region Unity Methods

        private void Awake()
        {
            ObstacleMaster = GetComponent<ObstacleMaster>();
        }

        private void Start()
        {
            _obstacleHp = ObstacleMaster.objectDefault.hitPoints;
        }

        internal virtual void OnTriggerEnter(Collider other)
        {
            if (other == null || !ObstacleMaster.ObjectIsReady || !ObstacleMaster.objectDefault.canKilled) return;
            ComponentToKill(other.GetComponent<BulletPlayer>(), EnumCollisionType.Shoot);
            ComponentToKill(other.GetComponent<BulletBombPlayer>(), EnumCollisionType.Bomb);
        }

        #endregion
        
        protected virtual void ComponentToKill(Component other, EnumCollisionType collisionType)
        {
            if (other == null) return;
            if (other == null || !ObstacleMaster.ObjectIsReady || !ObstacleMaster.objectDefault.canKilled) return;
            var playerWhoHit = WhoHit<PlayerMaster>(other);
            if (playerWhoHit == null) return;
            ObstacleMaster.OnEventObstacleHit(playerWhoHit);
            var damage = SetDamage(other);
            DebugManager.Log<ObstacleCollider>($"DAMAGE! {damage}, HP {_obstacleHp} - {other}");
            _obstacleHp -= damage;
            if (_obstacleHp > 0) return;
            playerWhoHit.SetPlayerScore(ObstacleMaster.objectDefault.GetScore());
            ObstacleMaster.OnEventObstacleDeath(playerWhoHit);

            /*PlayerMasterOld = WhoHit(other);
            OnEventObstacleMasterHit();
            OnEventObstacleScore(PlayerMasterOld.getPlayerSettings);
            ShouldSavePoint(PlayerMasterOld.getPlayerSettings);
            GamePlayManager.AddResultList(gamePlayingLog.GetEnemiesResult(), PlayerMasterOld.getPlayerSettings, enemy, 1, collisionType);
            KillStatsHandle(collisionType);
            ShouldFinishGame();*/
        }

        private static T WhoHit<T>(Component other) where T : class
        {
            return other switch
            {
                Bullets bullet => bullet.GetBulletData.BulletOwner as T,
                ObjectMaster player => player as T,
                _ => null
            };
        }

        private int SetDamage(Component component)
        {
            var teste= component switch
            {
                Bullets bullet => bullet.GetBulletData.BulletDamage,
                PlayerMaster player => player.ActualSkin.colliderDamage,
                _ => 0
            };
            return  teste;

        }
    }
}