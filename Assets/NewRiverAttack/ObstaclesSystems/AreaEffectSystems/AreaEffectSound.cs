using ImmersiveGames.AudioEvents;
using NewRiverAttack.ObstaclesSystems.EnemiesSystems;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.AreaEffectSystems
{
    public class AreaEffectSound : EnemiesSound
    {
        [SerializeField] private AudioEvent effectAreaSound;
        [SerializeField] private AudioEvent effectAreaExitSound;
        
        private AreaEffectMaster _areaEffectMaster;
     
        #region Unity Methods

        protected override void OnEnable()
        {
            base.OnEnable();
            _areaEffectMaster.EventMasterAreaEffectEnter += EnterAreaSound;
            _areaEffectMaster.EventMasterAreaEffectExit += StopAreaSound;
            _areaEffectMaster.EventObstacleDeath += StopAreaSound;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _areaEffectMaster.EventMasterAreaEffectEnter -= EnterAreaSound;
            _areaEffectMaster.EventMasterAreaEffectExit -= StopAreaSound;
            _areaEffectMaster.EventObstacleDeath -= StopAreaSound;
        }

        #endregion
        
        
        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            _areaEffectMaster = ObstacleMaster as AreaEffectMaster;
        }

        private void EnterAreaSound()
        {
            if (AudioSource != null && effectAreaSound != null)
                effectAreaSound.SimplePlay(AudioSource);
        }
        private void StopAreaSound(PlayerMaster playerMaster)
        {
            if(!_areaEffectMaster.IsInAreaEffect)return;
            StopAreaSound();
        }
        private void StopAreaSound()
        {
            if (AudioSource != null && effectAreaExitSound != null)
                if(!AudioSource.isPlaying && !_areaEffectMaster.IsInAreaEffect) return;
            AudioSource.Stop();
            effectAreaExitSound.PlayOnShot(AudioSource);
        }
    }
}