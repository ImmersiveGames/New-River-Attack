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
        #region Unity Methods

        private void Awake()
        {
            ObstacleMaster = GetComponent<ObstacleMaster>();
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
            if (other == null || !ObstacleMaster.ObjectIsReady || !ObstacleMaster.objectDefault.canKilled) return;
            var playerWhoHit = WhoHit<PlayerMaster>(other);
            if (playerWhoHit == null) return;
            var damage = MakeDamage(other);
            var hp = ObstacleMaster.objectDefault.hitPoints;
            DebugManager.Log<ObstacleCollider>($"DAMAGE! {damage}, HP {hp} - {other}");
            if (hp - damage <= 0)
            {
                playerWhoHit.SetPlayerScore(ObstacleMaster.objectDefault.GetScore());
                ObstacleMaster.OnEventObstacleHit(playerWhoHit); 
            }

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

        private int MakeDamage(Component component)
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