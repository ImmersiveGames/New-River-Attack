using ImmersiveGames.BulletsManagers;
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

        private GamePlayManager _gamePlayManager;
        #region Unity Methods

        private void Awake()
        {
            ObstacleMaster = GetComponent<ObstacleMaster>();
        }

        private void OnEnable()
        {
            _gamePlayManager = GamePlayManager.Instance;
            _gamePlayManager.EventGameReload += ReloadHp;
            _gamePlayManager.EventGameRestart += ReloadHp;
        }

        protected virtual void Start()
        {
            ReloadHp();
        }

        private void OnDisable()
        {
            _gamePlayManager.EventGameReload -= ReloadHp;
            _gamePlayManager.EventGameRestart -= ReloadHp;
        }

        internal virtual void OnTriggerEnter(Collider other)
        {
            if (other == null || !ObstacleMaster.ObjectIsReady || !ObstacleMaster.objectDefault.canKilled) return;
            ComponentToKill(other.GetComponent<BulletPlayer>(), EnumCollisionType.Shoot);
            ComponentToKill(other.GetComponent<BulletBombPlayer>(), EnumCollisionType.Bomb);
        }

        #endregion

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