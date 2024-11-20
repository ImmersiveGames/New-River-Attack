using ImmersiveGames.DebugManagers;
using NewRiverAttack.BulletsManagers;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.GameStatisticsSystem;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.Abstracts
{
    public abstract class ObstacleCollider : MonoBehaviour
    {
        protected ObstacleMaster ObstacleMaster;
        protected int ObstacleHp;

        protected GamePlayManager GamePlayManager;
        #region Unity Methods

        private void Awake()
        {
            SetInitialReferences();
        }

        protected virtual void OnEnable()
        {
            
            GamePlayManager.EventGameResetClear += ReloadHp;
            GamePlayManager.EventObstacleReload += ReloadHp;
        }

        protected virtual void Start()
        {
            ReloadHp();
        }

        protected virtual void OnDisable()
        {
            GamePlayManager.EventGameResetClear -= ReloadHp;
            GamePlayManager.EventObstacleReload -= ReloadHp;
        }

        internal virtual void OnTriggerEnter(Collider other)
        {
            if (other == null || !ObstacleMaster.ObjectIsReady || !ObstacleMaster.objectDefault.canKilled) return;
            ComponentToKill(other.GetComponent<BulletPlayer>(), EnumCollisionType.Shoot);
            ComponentToKill(other.GetComponent<BulletBombPlayer>(), EnumCollisionType.Bomb);
        }

        #endregion

        private void SetInitialReferences()
        {
            GamePlayManager = GamePlayManager.Instance;
            ObstacleMaster = GetComponent<ObstacleMaster>();
        }
        public int GetHp() => ObstacleHp;
        private void ReloadHp()
        {
            ObstacleHp = ObstacleMaster.objectDefault.hitPoints;
        }
        protected void ComponentToKill(Component other, EnumCollisionType typeCollision)
        {
            if (other == null) return;
            if (other == null || !ObstacleMaster.ObjectIsReady || !ObstacleMaster.objectDefault.canKilled) return;
            var playerWhoHit = WhoHit<PlayerMaster>(other);
            if (playerWhoHit == null) return;
            var damage = SetDamage(other);
            DebugManager.Log<ObstacleCollider>($"DAMAGE! {damage}, HP {ObstacleHp} - {other}");
            ObstacleHp -= damage;
            ObstacleMaster.OnEventObstacleHit(playerWhoHit);
            if (ObstacleHp > 0) return;
            playerWhoHit.SetPlayerScore(ObstacleMaster.objectDefault.GetScore());
            ObstacleMaster.OnEventObstacleDeath(playerWhoHit);
            DebugManager.Log<ObstacleCollider>($"setting {playerWhoHit.GetPlayerSettings}, Default {ObstacleMaster.objectDefault}");
            GameStatisticManager.instance.LogEnemiesHit(playerWhoHit.GetPlayerSettings,ObstacleMaster.objectDefault,1,typeCollision);
        }

        private static T WhoHit<T>(Component other) where T : class
        {
            return other switch
            {
                BulletPlayer bullet => bullet.GetData.Owner as T,
                ObjectMaster player => player as T,
                _ => null
            };
        }

        private int SetDamage(Component component)
        {
            var teste= component switch
            {
                BulletPlayer bullet => bullet.GetData.Damage,
                PlayerMaster player => player.ActualSkin.colliderDamage,
                _ => 0
            };
            return  teste;

        }
    }
}