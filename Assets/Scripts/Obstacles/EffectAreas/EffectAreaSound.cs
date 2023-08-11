using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RiverAttack
{
    public class EffectAreaSound : EnemiesSound
    {
        /*EffectAreaMaster m_EffectAreaMaster;
        [SerializeField]
        AudioEventSample effectAreaSound;
        [SerializeField]
        AudioEventSample effectAreaExitSound;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_EffectAreaMaster.EventEnterAreaEffect += SoundAreaEffect;
            m_EffectAreaMaster.EventExitAreaEffect += StopSoundSoundAreaEffect;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            m_EffectAreaMaster.EventEnterAreaEffect -= SoundAreaEffect;
            m_EffectAreaMaster.EventExitAreaEffect -= StopSoundSoundAreaEffect;
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

        void StopSoundSoundAreaEffect()
        {
            if ((!audioSource && !effectAreaSound) && !audioSource.isPlaying) return;
            effectAreaExitSound.Play(audioSource);
        }*/
    }
}

