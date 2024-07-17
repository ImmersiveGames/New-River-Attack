using ImmersiveGames.AudioEvents;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.EnemiesSystems
{
    public class MineSound : EnemiesSound
    {
        private MineMaster _mineMaster;
        [SerializeField] private AudioEvent alertAudio;

        protected override void OnEnable()
        {
            base.OnEnable();
            _mineMaster.EventAlertApproach += AlertAudio;
            _mineMaster.EventAlertStop += StopAlertAudio;
            _mineMaster.EventDetonate += DetonateAudio;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _mineMaster.EventAlertApproach -= AlertAudio;
            _mineMaster.EventAlertStop -= StopAlertAudio;
            _mineMaster.EventDetonate -= DetonateAudio;
        }

        protected override void SetInitialReferences()
        {
            ObstacleMaster = _mineMaster = GetComponent<MineMaster>();
            AudioSource = GetComponent<AudioSource>();
        }
        
        private void AlertAudio()
        {
            if (AudioSource != null && alertAudio != null)
                alertAudio.SimplePlay(AudioSource);
        }
        private void StopAlertAudio()
        {
            if (AudioSource != null && alertAudio != null && AudioSource.isPlaying)
                alertAudio.Stop(AudioSource);
        }
        private void DetonateAudio()
        {
            StopAlertAudio();
            if (AudioSource != null && audioExplosion != null)
                audioExplosion.SimplePlay(AudioSource);
        }
    }
}