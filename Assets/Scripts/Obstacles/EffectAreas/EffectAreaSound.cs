using UnityEngine;
namespace RiverAttack
{
    public class EffectAreaSound : EnemiesSound
    {
        private EffectAreaMaster m_EffectAreaMaster;
        [SerializeField] private AudioEventSample effectAreaSound;
        [SerializeField] private AudioEventSample effectAreaExitSound;

        private PlayerMaster m_PlayerMaster;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_EffectAreaMaster.EventEnterAreaEffect += SoundAreaEffect;
            m_EffectAreaMaster.EventExitAreaEffect += StopSoundAreaEffect;
            GamePlayManager.instance.EventOtherEnemiesKillPlayer += StopSoundAreaEffect;
            GamePlayManager.instance.EventEnemiesMasterKillPlayer += StopSoundAreaEffect;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            m_EffectAreaMaster.EventEnterAreaEffect -= SoundAreaEffect;
            m_EffectAreaMaster.EventExitAreaEffect -= StopSoundAreaEffect;
            
        }

        private void OnDestroy()
        {
            m_EffectAreaMaster.EventEnterAreaEffect -= SoundAreaEffect;
            m_EffectAreaMaster.EventExitAreaEffect -= StopSoundAreaEffect;
            if (!GamePlayManager.instance)
                return;
            GamePlayManager.instance.EventOtherEnemiesKillPlayer -= StopSoundAreaEffect;
            GamePlayManager.instance.EventEnemiesMasterKillPlayer -= StopSoundAreaEffect;

        }

        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            m_EffectAreaMaster = GetComponent<EffectAreaMaster>();
        }

        private void SoundAreaEffect()
        {
            if (!audioSource || !effectAreaSound) return;
            if(effectAreaSound.IsPlaying(audioSource)) return;
            effectAreaSound.Play(audioSource);
        }

        private void StopSoundAreaEffect()
        {
            if (!audioSource || !effectAreaSound) return;
            if(!effectAreaSound.IsPlaying(audioSource)) return;
            effectAreaExitSound.Play(audioSource);
        }
    }
}
