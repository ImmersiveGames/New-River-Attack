using ImmersiveGames.Utils;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems
{
    public class BossCollider : ObstacleCollider
    {
        private readonly PercentageTracker _percentageTracker = new PercentageTracker();

        #region Unity Methods

        protected override void OnEnable()
        {
            ObstacleMaster.EventObstacleHit += DamageProgression;
        }

        protected override void Start()
        {
            base.Start();
            _percentageTracker.AddTrack(50,Smoke);
            _percentageTracker.AddTrack(25,Smoke);
        }

        internal override void OnTriggerEnter(Collider other)
        {
            if (other == null || !ObstacleMaster.ObjectIsReady) return;
            base.OnTriggerEnter(other);
            ComponentToKill(other.GetComponentInParent<PlayerMaster>(), EnumCollisionType.Collider);
        }

        protected override void OnDisable()
        {
            ObstacleMaster.EventObstacleHit -= DamageProgression;
        }

        #endregion

        private void Smoke()
        {
            var bossAnimation = GetComponent<BossAnimation>();
            bossAnimation.SmokeBoss();
        }

        private void DamageProgression(PlayerMaster playerMaster)
        {
            var porcentagemAtual = (double)(ObstacleHp * 100) / ObstacleMaster.objectDefault.hitPoints;
            _percentageTracker.VerifyPercentage(porcentagemAtual);
        }
    }
    
}