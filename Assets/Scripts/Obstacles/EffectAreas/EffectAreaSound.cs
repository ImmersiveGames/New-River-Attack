using UnityEngine;
namespace RiverAttack
{
    public class EffectAreaSound : EnemiesSound
    {
        private EffectAreaMasterOld _mEffectAreaMasterOld;
        [SerializeField] private AudioEventSample effectAreaSound;
        [SerializeField] private AudioEventSample effectAreaExitSound;

        private PlayerMasterOld _mPlayerMasterOld;

        protected override void OnEnable()
        {
            base.OnEnable();
            _mEffectAreaMasterOld.EventEnterAreaEffect += SoundAreaEffect;
            _mEffectAreaMasterOld.EventExitAreaEffect += StopSoundAreaEffect;
            GamePlayManager.instance.EventOtherEnemiesKillPlayer += StopSoundAreaEffect;
            GamePlayManager.instance.EventEnemiesMasterKillPlayer += StopSoundAreaEffect;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _mEffectAreaMasterOld.EventEnterAreaEffect -= SoundAreaEffect;
            _mEffectAreaMasterOld.EventExitAreaEffect -= StopSoundAreaEffect;
            
        }

        private void OnDestroy()
        {
            _mEffectAreaMasterOld.EventEnterAreaEffect -= SoundAreaEffect;
            _mEffectAreaMasterOld.EventExitAreaEffect -= StopSoundAreaEffect;
            if (!GamePlayManager.instance)
                return;
            GamePlayManager.instance.EventOtherEnemiesKillPlayer -= StopSoundAreaEffect;
            GamePlayManager.instance.EventEnemiesMasterKillPlayer -= StopSoundAreaEffect;

        }

        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            _mEffectAreaMasterOld = GetComponent<EffectAreaMasterOld>();
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
