using UnityEngine;
namespace RiverAttack
{
    public class BossSounds : EnemiesSound
    {
        [SerializeField] private AudioEventSample bossHitAudio;
        private BossMasterOld _mBossMasterOld;

        protected override void OnEnable()
        {
            base.OnEnable();
            _mBossMasterOld.EventBossHit += HitBossSound;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _mBossMasterOld.EventBossHit -= HitBossSound;
        }

        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            _mBossMasterOld = ObstacleMasterOld as BossMasterOld;
        }

        private void HitBossSound()
        {
            if (audioSource != null && bossHitAudio != null)
                bossHitAudio.Play(audioSource);
        }
    }
}
