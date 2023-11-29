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
            GamePlayManager.instance.EventOtherEnemiesKillPlayer += StopSoundArea;
        }
        void OnTriggerExit(Collider other)
        {
            if(m_PlayerMaster == null)
                m_PlayerMaster = other.GetComponentInParent<PlayerMaster>();
            if (!m_PlayerMaster) return;
            m_PlayerMaster.inEffectArea = false;
            if (!m_PlayerMaster.shouldPlayerBeReady) return;
            StopSoundAreaEffect();
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
            if(GamePlayManager.instance)
                GamePlayManager.instance.EventOtherEnemiesKillPlayer -= StopSoundArea;
        }

        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            m_EffectAreaMaster = GetComponent<EffectAreaMaster>();
        }

        void SoundAreaEffect()
        {
            if ((!audioSource && !effectAreaSound) || audioSource.isPlaying) return;
            effectAreaSound.Play(audioSource);
        }

        void StopSoundAreaEffect()
        {
            if ((!audioSource && !effectAreaSound) && !audioSource.isPlaying) return;
            effectAreaExitSound.Play(audioSource);
        }

        void StopSoundArea()
        {
            if ((!audioSource && !effectAreaSound) && !audioSource.isPlaying) return;
            audioSource.Stop();
        }
    }
}
