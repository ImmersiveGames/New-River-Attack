using UnityEngine;
namespace RiverAttack
{
    public class EffectAreaSound : EnemiesSound
    {
        EffectAreaMaster m_EffectAreaMaster;
        [SerializeField]
        AudioEventSample effectAreaSound;
        [SerializeField]
        AudioEventSample effectAreaExitSound;
        
        PlayerMaster m_PlayerMaster;

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
        void OnDestroy()
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

        void SoundAreaEffect()
        {
            if (!audioSource || !effectAreaSound) return;
            if(effectAreaSound.IsPlaying(audioSource)) return;
            effectAreaSound.Play(audioSource);
        }

        void StopSoundAreaEffect()
        {
            if (!audioSource || !effectAreaSound) return;
            if(!effectAreaSound.IsPlaying(audioSource)) return;
            effectAreaExitSound.Play(audioSource);
        }
    }
}
