using ImmersiveGames.AudioEvents;
using NewRiverAttack.ObstaclesSystems.EnemiesSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.CollectibleSystems
{
    public class CollectibleSounds : EnemiesSound
    {
        [SerializeField] private AudioEvent collectSound;
        [SerializeField] private AudioEvent helpSound;

        private CollectibleMaster _collectibleMaster;

        #region Unity Methods

        protected override void OnEnable()
        {
            base.OnEnable();
            _collectibleMaster.EventMasterCollectCollect += MasterCollectSound;
        }

        private void OnBecameVisible()
        {
            if (_collectibleMaster.ObjectIsReady && _collectibleMaster.GetCollectibleSettings.obstacleTypes == ObstacleTypes.Refugee)
            {
                ShowSound();
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _collectibleMaster.EventMasterCollectCollect -= MasterCollectSound;
        }

        #endregion
        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            _collectibleMaster = ObstacleMaster as CollectibleMaster;
        }
        private void ShowSound()
        {
            if (AudioSource != null && helpSound != null)
                helpSound.SimplePlay(AudioSource);
        }
        private void MasterCollectSound()
        {
            if (AudioSource != null && collectSound != null)
                collectSound.PlayOnShot(AudioSource);
        }
    }
}