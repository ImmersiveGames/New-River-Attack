using ImmersiveGames.AudioEvents;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.CollectibleSystems;
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
        private GamePlayManager _gamePlayManager;

        #region UNity Methods

        protected override void OnEnable()
        {
            base.OnEnable();
            _areaEffectMaster.EventMasterAreaEffectEnter += EnterAreaSound;
            _areaEffectMaster.EventMasterAreaEffectExit += StopAreaSound;
            _areaEffectMaster.EventObstacleHit += StopAreaSound;
            _gamePlayManager.EventPlayerGetHit += StopAreaSound;

        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _areaEffectMaster.EventMasterAreaEffectEnter -= EnterAreaSound;
            _areaEffectMaster.EventMasterAreaEffectExit -= StopAreaSound;
            _areaEffectMaster.EventObstacleHit -= StopAreaSound;
            _gamePlayManager.EventPlayerGetHit -= StopAreaSound;
        }

        #endregion
        
        
        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            _areaEffectMaster = ObstacleMaster as AreaEffectMaster;
            _gamePlayManager = GamePlayManager.instance;
        }

        private void EnterAreaSound()
        {
            if (AudioSource != null && effectAreaSound != null)
                effectAreaSound.SimplePlay(AudioSource);
        }
        private void StopAreaSound(PlayerMaster playerMaster)
        {
            if(!_areaEffectMaster.InAreaEffect)return;
            StopAreaSound();
        }
        private void StopAreaSound()
        {
            if (AudioSource != null && effectAreaExitSound != null)
                if(!AudioSource.isPlaying && !_areaEffectMaster.InAreaEffect) return;
            _areaEffectMaster.InAreaEffect = false;
            AudioSource.Stop();
            effectAreaExitSound.PlayOnShot(AudioSource);
        }
    }
}