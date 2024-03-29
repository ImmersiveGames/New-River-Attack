﻿using UnityEngine;
namespace RiverAttack
{
    public class BossSounds : EnemiesSound
    {
        [SerializeField]
        AudioEventSample bossHitAudio;
        BossMaster m_BossMaster;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_BossMaster.EventBossHit += HitBossSound;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            m_BossMaster.EventBossHit -= HitBossSound;
        }

        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            m_BossMaster = obstacleMaster as BossMaster;
        }
        void HitBossSound()
        {
            if (audioSource != null && bossHitAudio != null)
                bossHitAudio.Play(audioSource);
        }
    }
}
