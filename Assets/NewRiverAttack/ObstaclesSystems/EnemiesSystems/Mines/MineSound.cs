
using System;
using ImmersiveGames.AudioEvents;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.EnemiesSystems.Mines
{
    public class MineSound : EnemiesSound
    {
        private MineMaster _mineMaster;
        [SerializeField] private AudioEvent alertAudio;
        [SerializeField] private AudioEvent shootAudio;
        #region Unity Methods

        protected override void OnEnable()
        {
            base.OnEnable();
            _mineMaster.EventAlertApproach += AlertAudio;
            _mineMaster.EventShoot += ShootAudio;
            _mineMaster.EventAlertStop += StopAlertAudio;
            _mineMaster.EventDetonate += DetonateAudio;
            _mineMaster.EventObstacleDeath += StopAlertAudioOnDeath;
        }
        

        protected override void OnDisable()
        {
            base.OnDisable();
            _mineMaster.EventAlertApproach -= AlertAudio;
            _mineMaster.EventShoot -= ShootAudio;
            _mineMaster.EventAlertStop -= StopAlertAudio;
            _mineMaster.EventDetonate -= DetonateAudio;
            _mineMaster.EventObstacleDeath -= StopAlertAudioOnDeath;
        }

        #endregion
        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            _mineMaster = ObstacleMaster as MineMaster;
        }
        private void AlertAudio()
        {
            if (AudioSource != null && alertAudio != null && !AudioSource.isPlaying) 
            {
                alertAudio.SimplePlay(AudioSource);
            }
        }
        private void ShootAudio()
        {
            if (AudioSource != null && shootAudio != null && !AudioSource.isPlaying) 
            {
                shootAudio.SimplePlay(AudioSource);
            }
        }

        // Método para parar o áudio de alerta quando necessário
        private void StopAlertAudio()
        {
            if (AudioSource != null && AudioSource.isPlaying) 
            {
                AudioSource.Stop();
            }
        }

        // Parar o áudio de alerta quando a mina é destruída
        private void StopAlertAudioOnDeath(PlayerMaster playerMaster)
        {
            StopAlertAudio(); 
        }

        // Som de detonação, parando o som de alerta antes
        private void DetonateAudio()
        {
            StopAlertAudio(); 
            if (AudioSource != null && audioExplosion != null)
            {
                audioExplosion.SimplePlay(AudioSource);
            }
        }
    }
}
